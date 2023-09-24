using Corelibs.Basic.Blocks;
using Corelibs.Basic.DDD;
using Corelibs.Basic.UseCases;
using Corelibs.MongoDB;
using Manabu.Entities.Content.Events;
using Manabu.Infrastructure.Contexts.Rehearse.EventHandlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace Manabu.Infrastructure.Contexts.Rehearse._EventHandlers;

public class EventStoreListenerWorker : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(1));
    private readonly Dictionary<string, Func<IServiceProvider, BaseDomainEvent, Task<Result>>> _handlers = new();

    public EventStoreListenerWorker(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;

        AddEventHandler<LearningObjectAddedForRehearseEvent>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var client = scope.ServiceProvider.GetRequiredService<MongoClient>();
        var connection = scope.ServiceProvider.GetRequiredService<MongoConnection>();
        connection.Database = client.GetDatabase(connection.DatabaseName);

        var store = scope.ServiceProvider.GetRequiredService<IEventStore>();

        while (await _timer.WaitForNextTickAsync(stoppingToken) &&
            !stoppingToken.IsCancellationRequested)
        {
            var result = await store.Peek(20);
            if (!result.ValidateSuccessAndValues())
                continue;

            var events = result.Get();
            foreach (var ev in events)
            {
                var name = ev.GetType().Name;
                if (!_handlers.TryGetValue(name, out var handler))
                    continue;

                var handlerResult = await handler(scope.ServiceProvider, ev);
                if (!handlerResult.IsSuccess)
                    continue;

                await store.Delete(ev);

                Console.WriteLine(ev);
            }
        }
    }

    private void AddEventHandler<T>() where T : BaseDomainEvent
    {
        _handlers.Add(
            typeof(T).Name,
            (sp, ev) =>
            {
                var handler = sp.GetRequiredService<IEventHandler<T>>();
                return handler.Handle((T) ev);
            });
    }
}
