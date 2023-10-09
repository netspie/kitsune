using Blazored.LocalStorage;
using Corelibs.Basic.DDD;
using Corelibs.Basic.Reflection;
using Corelibs.Basic.Repository;
using Corelibs.Basic.Storage;
using Corelibs.Basic.UseCases;
using Corelibs.Basic.UseCases.Events;
using Corelibs.MongoDB;
using FluentValidation;
using FluentValidation.AspNetCore;
using Manabu.Entities.Content._Shared;
using Manabu.Entities.Content.Audios;
using Manabu.Entities.Content.Events;
using Manabu.Infrastructure;
using Manabu.Infrastructure.Contexts.Rehearse;
using Manabu.Infrastructure.Contexts.Rehearse._EventHandlers;
using Manabu.Infrastructure.Contexts.Rehearse.EventHandlers;
using Manabu.Infrastructure.CQRS.Content.Flashcards;
using Manabu.UI.Common;
using Manabu.UI.Common.Auth;
using Manabu.UI.Common.Extensions;
using Manabu.UI.Common.Operations;
using Manabu.UI.Common.Storage;
using Manabu.UI.Server.Data;
using Manabu.UseCases.Content;
using Manabu.UseCases.Content.Flashcards;
using Mediator;
using System.Reflection;
using System.Security.Claims;

namespace Manabu.UI.Server;

public static class Startup
{
    public static void InitializeApp(this IServiceCollection services, IWebHostEnvironment environment)
    {
        var commonUIAssembly = typeof(App).Assembly;
        var entitiesAssembly = typeof(Manabu.Entities.Content.Users.User).Assembly;
        var useCasesAssembly = typeof(Manabu.UseCases.Content.Users.CreateUserCommand).Assembly;

        var mongoConnectionString = MongoConfig.ConnectionString;

        services.AddBlazoredLocalStorage();

        if (environment.IsDevelopment())
        {
            services.AddScoped<IAuthenticationService, MockAuthenticationService>();
            services.AddScoped<IAccessorAsync<ClaimsPrincipal>, MockPrincipalAccessor>();
        }
        else
        {
            services.AddScoped<IAccessorAsync<ClaimsPrincipal>, ClaimsPrincipalAccessor>();
            services.AddScoped<IAuthenticationService, AzureB2CAuthenticationService>();
        }

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(useCasesAssembly);

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        if (environment.IsDevelopment() && !MongoDbExtensions.CanHaveTransactions(mongoConnectionString))
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(MongoDbCommandNoTransactionBehaviour<,>));
        else
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(MongoDbCommandTransactionBehaviour<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(MongoDbQueryBehaviour<,>));

        services.AddScoped<IQueryExecutor, MediatorQueryExecutor>();
        services.AddScoped<ICommandExecutor, MediatorCommandExecutor>();

        services.AddMediator(opts => opts.ServiceLifetime = ServiceLifetime.Scoped);

        services.AddRepositories(environment, entitiesAssembly, mongoConnectionString);
        services.AddSingleton<IMediaStorage<Audio>>(sp => new LocalMediaStorage<Audio>(
            $"/media/audio", $"/media/audio"));

        services.AddTypes(commonUIAssembly.GetTypesInFolder("State"), ServiceLifetime.Scoped);

        services.AddScoped<IFlashcardResolver, JapaneseFlashcardResolver>();

        // ------ UI / CLIENT ------

        services.AddScoped<IStorage, BlazoredJsonLocalStorage>();

        // Operations
        services.AddScoped<CutCopyPhraseOperation>();

        // ------ EVENTS ------

        // Event Store
        var eventTypes = AssemblyExtensionsEx.GetCurrentDomainTypesImplementing<BaseDomainEvent>(entitiesAssembly);
        var eventTypesDict = eventTypes.ToDictionary(t => t.Name, t => t);
        services.AddScoped<IEventStore>(sp =>
            new MongoDbEventStore(sp.GetRequiredService<MongoConnection>(), eventTypesDict));

        // Event Listener
        services.AddHostedService(sp => new EventStoreListenerWorker(
                sp.GetRequiredService<IServiceScopeFactory>()));

        // Event Handlers
        services.AddScoped<IEventHandler<LearningObjectAddedForRehearseEvent>, LearningObjectAddedForRehearseEventHandler>();

        // ------ PROCESSORS - LEARNING ENTITIES TO REHEARSE ITEMS ------

        AddRehearseProcessors(services, entitiesAssembly);
    }

    public static void AddRepositories(
        this IServiceCollection services, 
        IWebHostEnvironment environment, 
        Assembly assembly,
        string connectionString)
    {
        var databaseName = environment.IsDevelopment() ? "Kitsune_dev" : "Kitsune_prod";

        MongoConventionPackExtensions.AddIgnoreConventionPack();

        services.AddMongoRepositories(assembly, connectionString, databaseName);
    }

    private static void AddRehearseProcessors(IServiceCollection services, Assembly entitiesAssembly)
    {
        var pim = new ProcessorEntitiesInfoMaster();
        services.AddSingleton(pim);

        var infoTypes = AssemblyExtensionsEx.GetCurrentDomainTypesImplementing<IProcessorEntityInfo>(entitiesAssembly);
        foreach (var type in infoTypes)
        {
            var info = pim.Add(Activator.CreateInstance(type) as IProcessorEntityInfo);

            var infoInterfaceType = TypeCreateExtensions.CreateInterface(
                typeof(IProcessorEntityInfo<,>), info.EntityType, info.IdType);

            services.AddSingleton(infoInterfaceType, info);

            var processorInterfaceType = TypeCreateExtensions.CreateInterface(
                typeof(ILearningObjectToRehearseProcessor<,>), info.EntityType, info.IdType);

            var processorConcreteType = TypeCreateExtensions.CreateClass(
                typeof(LearningObjectToRehearseProcessor<,>), info.EntityType, info.IdType);

            services.AddScoped(processorInterfaceType, processorConcreteType);
        }
    }
}
