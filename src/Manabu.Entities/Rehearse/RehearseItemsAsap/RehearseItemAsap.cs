using Corelibs.Basic.DDD;
using Manabu.Entities.Content.Users;
using Manabu.Entities.Shared;

namespace Manabu.Entities.Rehearse.RehearseItems;

public class RehearseItemAsap : Entity<RehearseItemId>, IAggregateRoot<RehearseItemId>
{
    public static string DefaultCollectionName { get; } = "rehearseItemsAsap";

    public UserId Owner { get; init; }
    public LearningObjectId ItemId { get; init; }
    public LearningItemType ItemType { get; init; }
    public LearningMode Mode { get; init; }

    public RehearseItemAsap(
        RehearseItemId id,
        UserId owner,
        LearningObjectId itemId,
        LearningItemType itemType,
        LearningMode mode) : base(id)
    {
        Owner = owner;
        ItemId = itemId;
        ItemType = itemType;
        Mode = mode;
    }
}
