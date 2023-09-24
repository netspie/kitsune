using Corelibs.Basic.Blocks;
using Manabu.Entities.Content.Events;
using Manabu.UseCases.Rehearse;

namespace Manabu.Infrastructure.Contexts.Rehearse.EventHandlers;

public interface IEventHandler<TEvent>
{
    Task<Result> Handle(TEvent ev);
}

public class LearningObjectAddedForRehearseEventHandler : IEventHandler<LearningObjectAddedForRehearseEvent>
{
    public LearningObjectAddedForRehearseEventHandler()
    {
    }

    public async Task<Result> Handle(
        LearningObjectAddedForRehearseEvent ev)
    {
        // TO DO: GET PROCESSOR SOMEHOW!!!
        //var processor = _processorMaster.Get(ev.ObjectType);
        //return await processor.Process(ev.Owner, ev.ObjectId);

        return Result.Success();
    }
}
