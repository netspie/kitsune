using Corelibs.Basic.Blocks;
using Corelibs.Basic.Reflection;
using Corelibs.Basic.UseCases.Events;
using Manabu.Entities.Content._Shared;
using Manabu.Entities.Content.Events;
using Manabu.UseCases.Content;
using Microsoft.Extensions.DependencyInjection;

namespace Manabu.Infrastructure.Contexts.Rehearse.EventHandlers;

public class LearningObjectAddedForRehearseEventHandler : IEventHandler<LearningObjectAddedForRehearseEvent>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ProcessorEntitiesInfoMaster _processorEntitiesInfoMaster;

    public LearningObjectAddedForRehearseEventHandler(
        IServiceScopeFactory serviceScopeFactory, 
        ProcessorEntitiesInfoMaster processorEntitiesInfoMaster)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _processorEntitiesInfoMaster = processorEntitiesInfoMaster;
    }

    public async Task<Result> Handle(
        LearningObjectAddedForRehearseEvent ev)
    {
        var result = Result.Success();

        var processorInfo = _processorEntitiesInfoMaster.Get(ev.ObjectType);

        var processorInterfaceType = TypeCreateExtensions.CreateInterface(
            typeof(ILearningObjectToRehearseProcessor<,>), processorInfo.EntityType, processorInfo.IdType);

        using var scope = _serviceScopeFactory.CreateScope();
        var processorObject = scope.ServiceProvider.GetRequiredService(processorInterfaceType);
        var processor = processorObject as ILearningObjectToRehearseProcessor;
        if (processor is null)
            return result.Fail();

        await processor.Process(ev.Owner, ev.ObjectId);

        return result;
    }
}
