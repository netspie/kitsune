using Corelibs.Basic.DDD;
using Manabu.Entities.Content.Users;
using Manabu.Entities.Flashcards;

namespace Manabu.Entities.Rehearse.RehearseItems;

public class RehearseItemAsap : Entity<RehearseItemId>, IAggregateRoot<RehearseItemId>
{
    public const string DefaultCollectionName = "rehearseItemsAsap";

    public UserId Owner { get; init; }
    public LearningItemType ItemType { get; init; }
    public LearningMode Mode { get; init; }

    public RehearseItemAsap(
        RehearseItemId id,
        UserId owner,
        LearningItemType itemType,
        LearningMode mode) : base(id)
    {
        Owner = owner;
        ItemType = itemType;
        Mode = mode;
    }
}
