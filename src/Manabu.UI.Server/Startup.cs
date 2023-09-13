using Blazored.LocalStorage;
using Corelibs.Basic.Reflection;
using Corelibs.Basic.Repository;
using Corelibs.Basic.Storage;
using Corelibs.Basic.UseCases;
using Corelibs.MongoDB;
using FluentValidation;
using FluentValidation.AspNetCore;
using Manabu.Entities.Audios;
using Manabu.Infrastructure.CQRS.Flashcards;
using Manabu.UI.Common;
using Manabu.UI.Common.Extensions;
using Manabu.UI.Common.Operations;
using Manabu.UI.Common.State;
using Manabu.UI.Common.Storage;
using Manabu.UI.Server.Data;
using Manabu.UseCases.Flashcards;
using Mediator;
using System.Reflection;
using System.Security.Claims;

namespace Manabu.UI.Server;

public static class Startup
{
    public static void InitializeApp(this IServiceCollection services, IWebHostEnvironment environment)
    {
        var commonUIAssembly = typeof(App).Assembly;
        var entitiesAssembly = typeof(Entities.Users.User).Assembly;
        var useCasesAssembly = typeof(UseCases.Users.CreateUserCommand).Assembly;

        services.AddBlazoredLocalStorage();

        services.AddScoped<IAccessorAsync<ClaimsPrincipal>, ClaimsPrincipalAccessor>();
        
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(useCasesAssembly);

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(MongoDbCommandTransactionBehaviour<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(MongoDbQueryBehaviour<,>));

        services.AddScoped<IQueryExecutor, MediatorQueryExecutor>();
        services.AddScoped<ICommandExecutor, MediatorCommandExecutor>();

        services.AddMediator(opts => opts.ServiceLifetime = ServiceLifetime.Scoped);

        services.AddRepositories(environment, entitiesAssembly);
        services.AddSingleton<IMediaStorage<Audio>>(sp => new LocalMediaStorage<Audio>(
            "../Manabu.UI.Common/wwwroot/media/audio", "_content/Manabu.UI.Common/media/audio"));

        services.AddScoped<IFlashcardResolver, JapaneseFlashcardResolver>();

        services.AddTypes(commonUIAssembly.GetTypesInFolder("State"), ServiceLifetime.Scoped);

        services.AddScoped<IStorage, BlazoredJsonLocalStorage>();
        services.AddScoped<CutCopyPhraseOperation>();
    }

    public static void AddRepositories(this IServiceCollection services, IWebHostEnvironment environment, Assembly assembly)
    {
        var mongoConnectionString = Environment.GetEnvironmentVariable("HackStudyDatabaseConn");
        var databaseName = environment.IsDevelopment() ? "HackStudy_dev" : "HackStudy_prod";

        MongoConventionPackExtensions.AddIgnoreConventionPack();

        services.AddMongoRepositories(assembly, mongoConnectionString, databaseName);
    }
}
