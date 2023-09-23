using Corelibs.Basic.Collections;
using Corelibs.Basic.DDD;
using Manabu.Entities.Users;
using System.Security.Cryptography;
using System.Text;

namespace Manabu.Entities.RehearseItems;

public class RehearseItem : Entity<RehearseItemId>, IAggregateRoot<RehearseItemId>
{
    public const string DefaultCollectionName = "rehearseItems";

    public UserId Owner { get; private set; }
    public string ItemId { get; private set; }
    public string ItemType { get; private set; }
    public string Mode { get; private set; }
    public DateTime CreatedUtcTime { get; init; }
    public int SessionsToNextRehearse { get; set; }

    public List<RehearseItemPerMode> ItemsPerMode { get; private set; }

    public RehearseItem(
        RehearseItemId id,
        UserId owner,
        string itemId,
        string itemType) : base(id)
    {
        Owner = owner;
        ItemId = itemId;
        ItemType = itemType;
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

    public RehearseItemId(string userId, string itemId) : base(GenerateGuidHash(userId, itemId))
    {
    }

    public static string GenerateGuidHash(string userId, string itemId)
    {
        string combinedValue = $"{userId}{itemId}";

        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedValue));
            return new Guid(hashBytes).ToString();
        }
    }
}
