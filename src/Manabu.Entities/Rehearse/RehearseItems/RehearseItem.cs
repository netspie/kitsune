using Corelibs.Basic.DDD;
using Corelibs.Basic.Encryption;
using Manabu.Entities.Content.Users;
using Manabu.Entities.Shared;

namespace Manabu.Entities.Rehearse.RehearseItems;

public class RehearseItem : Entity<RehearseItemId>, IAggregateRoot<RehearseItemId>
{
    public const string DefaultCollectionName = "rehearseItems";
    public const float DefaultEFactor = 0.25f;

    public UserId Owner { get; init; }
    public LearningObjectId ItemId { get; init; }
    public LearningItemType ItemType { get; init; }
    public LearningMode Mode { get; init; }
    public DateTime CreatedUtcTime { get; init; }

    public Difficulty Difficulty { get; private set; }
    public int RepsTotal { get; private set; }
    public int RepsInternal { get; private set; }
    public int RepsInterval { get; private set; }
    public float EFactor { get; private set; } = DefaultEFactor; // default
    public DateTime LastRehearsedUtcTime { get; private set; }

    public RehearseItem(
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
        CreatedUtcTime = DateTime.UtcNow;
    }

    public void Answer(Difficulty difficulty)
    {

    }
}

public class RehearseItemId : EntityId
{
    public RehearseItemId(string id) : base(id)
    {
    }

    public RehearseItemId(UserId userId, LearningObjectId itemId)
        : base(EncryptionFunctions.GenerateGuidHash(userId.Value, itemId.Value))
    {
    }
}
