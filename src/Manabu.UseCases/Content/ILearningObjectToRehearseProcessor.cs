using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.Basic.DDD;
using Corelibs.Basic.Repository;
using Corelibs.Basic.UseCases;
using Manabu.Entities.Content.Events;
using Manabu.Entities.Content.Users;
using Manabu.Entities.Rehearse.RehearseItems;
using Manabu.Entities.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Manabu.UseCases.Content;

public interface ILearningObjectToRehearseProcessor
{
    Task<Result> Process(UserId owner, LearningObjectId id);
}

public abstract class BaseLearningObjectToRehearseProcessor<TObject, TId> :
    ILearningObjectToRehearseProcessor
    where TObject : Entity<TId>
    where TId : EntityId
{
    private readonly IEventStore _eventStore;
    private readonly IRepository<RehearseItem, RehearseItemId> _rehearseItemRepository;
    private readonly IRepository<TObject, TId> _entityRepository;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IProcessorEntityInfo<TObject, TId> _entityInfo;

    protected BaseLearningObjectToRehearseProcessor(
        IEventStore eventStore,
        IRepository<RehearseItem, RehearseItemId> rehearseItemRepository,
        IServiceScopeFactory serviceScopeFactory,
        IProcessorEntityInfo<TObject, TId> entityInfo,
        IRepository<TObject, TId> entityRepository)
    {
        _eventStore = eventStore;
        _rehearseItemRepository = rehearseItemRepository;
        _serviceScopeFactory = serviceScopeFactory;
        _entityInfo = entityInfo;
        _entityRepository = entityRepository;
    }

    public async Task<Result> Process(UserId owner, LearningObjectId learningObjectId)
    {
        var result = Result.Success();

        using var scope = _serviceScopeFactory.CreateScope();

        var id = _entityInfo.CreateId(learningObjectId);
        var learningObject = await _entityRepository.Get(id, result);

        var childrenIds = _entityInfo.GetChildLearningItemIds(learningObject);
        var events = ToAddEvents(childrenIds, owner);

        await _eventStore.Save(events);

        var modes = _entityInfo.LearningModes;
        if (!_entityInfo.IsLearningItem || modes.IsNullOrEmpty())
            return result;

        var rehearseItems = new List<RehearseItem>();
        foreach (var mode in modes)
        {
            var rehearseItemId = new RehearseItemId(owner, learningObjectId, mode);
            var rehearseItem = await _rehearseItemRepository.Get(rehearseItemId, result);
            if (rehearseItem is not null)
                continue;

            var learningItemType = new LearningItemType(_entityInfo.LearningObjectType.Value);
            rehearseItems.Add(new RehearseItem(
                rehearseItemId, owner, learningObjectId, learningItemType, mode));
        };

        // TO DO: Add entity of rehearse history? or data?

        result += _rehearseItemRepository.Create(rehearseItems);

        return result;
    }

    private BaseDomainEvent[] ToAddEvents(IEnumerable<EntityId> ids, UserId owner) =>
        ids.Select(id =>
        {
            var type = id.GetType();

            return new LearningObjectAddedForRehearseEvent()
            {
                Id = Guid.NewGuid().ToString(),
                Timestamp = DateTime.UtcNow.Ticks,
                ObjectType = _entityInfo.LearningObjectType,
                ObjectId = new LearningObjectId(id.Value),
                Owner = owner
            };
        })
        .ToArray();
}

public interface IProcessorEntityInfo<TEntity, TEntityId>
    where TEntity : IEntity<TEntityId>
    where TEntityId : EntityId
{
    bool IsLearningItem { get; }
    LearningMode[] LearningModes { get; }
    LearningObjectType LearningObjectType { get; }
    EntityId[] GetChildLearningItemIds(TEntity learningEntity) => Array.Empty<EntityId>();
    public TEntityId CreateId(LearningObjectId learningObjectId);
}
