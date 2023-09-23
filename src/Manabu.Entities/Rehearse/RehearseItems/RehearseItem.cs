using Corelibs.Basic.Collections;
using Corelibs.Basic.DDD;
using Corelibs.Basic.Encryption;
using Manabu.Entities.Content.Users;
using Manabu.Entities.Flashcards;
using Manabu.Entities.Rehearse.RehearseItems;
using Manabu.Entities.Shared;

namespace Manabu.Entities.RehearseItems;

public class RehearseItem : Entity<RehearseItemId>, IAggregateRoot<RehearseItemId>
{
    public const string DefaultCollectionName = "rehearseItems";

    public UserId Owner { get; private set; }
    public LearningItemId ItemId { get; private set; }
    public LearningItemType ItemType { get; private set; }
    public LearningMode Mode { get; private set; }
    public DateTime CreatedUtcTime { get; init; }
    public int SessionsToNextRehearse { get; set; }

    public Difficulty Difficulty { get; set; }
    public int RepsTotal { get; set; }
    public int RepsInternal { get; set; }
    public int RepsInterval { get; set; }
    public float EFactor { get; set; } = 0.25f; // default
    public DateTime LastRehearsedUtcTime { get; init; }

    public List<RehearseItemPerMode> ItemsPerMode { get; private set; }

    public RehearseItem(
        RehearseItemId id,
        UserId owner,
        LearningItemId itemId,
        LearningItemType itemType,
        LearningMode mode) : base(id)
    {
        Owner = owner;
        ItemId = itemId;
        ItemType = itemType;
        Mode = mode;
        CreatedUtcTime = DateTime.UtcNow;
    }

    //public void Answer(Difficulty difficulty)

    public DateTime LastTimeRehearsedUtcTime =>
        ItemsPerMode.SelectOrDefault(i => i.LastTimeRehearsedUtcTime).Order().LastOrDefault();

    public int ReviewTotalCount => ItemsPerMode
        .SelectOrDefault(i => i.ReviewTotalCount)
        .AggregateOrDefault((x, y) => x + y);

    public class RehearseItemPerMode
    {
        public DateTime CreatedUtcTime { get; init; }
        public DateTime LastTimeRehearsedUtcTime { get; set; }
        public int ReviewTotalCount { get; set; }
        public int ScheduleDayPivotIndex { get; set; }
    }
}

public class RehearseItemId : EntityId
{
    public RehearseItemId(string id) : base(id)
    {
    }

    public RehearseItemId(string userId, string itemId) : base(EncryptionFunctions.GenerateGuidHash(userId, itemId))
    {
    }
}
