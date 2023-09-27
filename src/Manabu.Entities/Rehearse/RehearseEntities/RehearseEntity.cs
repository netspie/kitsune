using Corelibs.Basic.DDD;
using Manabu.Entities.Content.Users;
using Manabu.Entities.Shared;

namespace Manabu.Entities.Rehearse.RehearseEntities;

public class RehearseEntity : Entity<RehearseEntityId>, IAggregateRoot<RehearseEntityId>
{
    public const string DefaultCollectionName = "rehearseEntities";

    public UserId Owner { get; init; }
    public LearningObjectType ObjectType { get; init; }
    public bool IsItem { get; init; }

    public RehearseEntity(
        RehearseEntityId id,
        UserId owner,
        LearningObjectType objectType,
        bool isItem) : base(id)
    {
        Owner = owner;
        ObjectType = objectType;
        IsItem = isItem;
    }
}

public class RehearseEntityId : EntityId { public RehearseEntityId(string value) : base(value) { } }
