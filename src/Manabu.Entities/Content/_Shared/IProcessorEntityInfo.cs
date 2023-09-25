using Corelibs.Basic.DDD;
using Manabu.Entities.Shared;

namespace Manabu.Entities.Content._Shared;

public interface IProcessorEntityInfo
{
    bool IsLearningItem { get; }
    LearningMode[] LearningModes { get; }
    LearningObjectType LearningObjectType { get; }
    public Type EntityType { get; }
    public Type IdType { get; }
}

public interface IProcessorEntityInfo<TEntity, TEntityId> : IProcessorEntityInfo
    where TEntity : IEntity<TEntityId>
    where TEntityId : EntityId
{
    EntityId[] GetChildLearningItemIds(TEntity learningEntity);
    public TEntityId CreateId(LearningObjectId learningObjectId);
}
