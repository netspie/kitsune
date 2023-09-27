using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.Basic.DDD;
using Corelibs.Basic.Repository;
using Corelibs.Basic.UseCases.Events;
using Corelibs.MongoDB;
using Manabu.Entities.Content._Shared;
using Manabu.Entities.Content.Events;
using Manabu.Entities.Content.Users;
using Manabu.Entities.Rehearse.RehearseEntities;
using Manabu.Entities.Rehearse.RehearseItems;
using Manabu.Entities.Shared;
using Manabu.UseCases.Content;
using MongoDB.Driver;

namespace Manabu.Infrastructure.Contexts.Rehearse;

public sealed class LearningObjectToRehearseProcessor<TEntity, TId> :
    ILearningObjectToRehearseProcessor<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : EntityId
{
    private readonly MongoClient _mongoClient;
    private readonly MongoConnection _mongoConnection;
    private readonly ProcessorEntitiesInfoMaster _processorEntitiesInfoMaster;
    private readonly IEventStore _eventStore;
    private readonly IRepository<RehearseItem, RehearseItemId> _rehearseItemRepository;
    private readonly IRepository<RehearseEntity, RehearseEntityId> _rehearseEntityRepository;
    private readonly IRepository<TEntity, TId> _entityRepository;
    private readonly IProcessorEntityInfo<TEntity, TId> _entityInfo;

    public LearningObjectToRehearseProcessor(
        MongoClient mongoClient,
        MongoConnection mongoConnection,
        ProcessorEntitiesInfoMaster processorEntitiesInfoMaster,
        IEventStore eventStore,
        IRepository<RehearseItem, RehearseItemId> rehearseItemRepository,
        IProcessorEntityInfo<TEntity, TId> entityInfo,
        IRepository<TEntity, TId> entityRepository,
        IRepository<RehearseEntity, RehearseEntityId> rehearseEntityRepository)
    {
        _mongoClient = mongoClient;
        _mongoConnection = mongoConnection;
        _processorEntitiesInfoMaster = processorEntitiesInfoMaster;
        _eventStore = eventStore;
        _rehearseItemRepository = rehearseItemRepository;
        _entityInfo = entityInfo;
        _entityRepository = entityRepository;
        _rehearseEntityRepository = rehearseEntityRepository;
    }

    public async Task<Result> Process(UserId owner, LearningObjectId learningObjectId)
    {
        var result = Result.Success();

        using var session = await _mongoClient.StartSessionAsync();
        _mongoConnection.Session = session;
        _mongoConnection.Database = _mongoClient.GetDatabase(_mongoConnection.DatabaseName);
        session.StartTransaction();

        var id = _entityInfo.CreateId(learningObjectId);
        var learningObject = await _entityRepository.Get(id, result);

        var childrenIds = _entityInfo.GetChildLearningItemIds(learningObject);
        var events = ToAddEvents(childrenIds, owner);

        await _eventStore.Save(events);

        var rehearseEntityId = new RehearseEntityId(learningObjectId.Value);
        var rehearseEntityResult = await _rehearseEntityRepository.GetBy(rehearseEntityId);
        if (!rehearseEntityResult.ValidateSuccessAndValues())
            result += await _rehearseEntityRepository.Create(
                new RehearseEntity(rehearseEntityId, owner, _entityInfo.LearningObjectType, _entityInfo.IsLearningItem));

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

        result += _rehearseItemRepository.Create(rehearseItems);
        if (result.IsSuccess)
            await session.CommitTransactionAsync();
        else
            await session.AbortTransactionAsync();

        return result;
    }

    private BaseDomainEvent[] ToAddEvents(IEnumerable<EntityId> ids, UserId owner) =>
        ids.Select(id =>
        {
            var type = id.GetType();
            var objectType = _processorEntitiesInfoMaster.GetType(id);

            return new LearningObjectAddedForRehearseEvent()
            {
                Id = Guid.NewGuid().ToString(),
                Timestamp = DateTime.UtcNow.Ticks,
                ObjectType = objectType,
                ObjectId = new LearningObjectId(id.Value),
                Owner = owner
            };
        })
        .ToArray();
}

