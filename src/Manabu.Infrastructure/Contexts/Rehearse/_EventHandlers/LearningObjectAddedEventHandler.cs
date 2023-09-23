using Corelibs.Basic.Repository;
using Manabu.Entities.Content.Events;
using Manabu.Entities.Rehearse.RehearseItems;
using Mediator;

namespace Manabu.Infrastructure.Contexts.Rehearse.EventHandlers;

public class LearningObjectAddedEventHandler : INotificationHandler<LearningObjectAddedEvent>
{
    private readonly IRepository<RehearseItem, RehearseItemId> _rehearseItemRepository;
    private readonly IRepository<RehearseItemAsap, RehearseItemId> _rehearseItemAsapRepository;

    public LearningObjectAddedEventHandler(
        IRepository<RehearseItem, RehearseItemId> rehearseItemRepository, 
        IRepository<RehearseItemAsap, RehearseItemId> rehearseItemAsapRepository)
    {
        _rehearseItemRepository = rehearseItemRepository;
        _rehearseItemAsapRepository = rehearseItemAsapRepository;
    }

    public async ValueTask Handle(
        LearningObjectAddedEvent notification, CancellationToken ct)
    {

    }
}
