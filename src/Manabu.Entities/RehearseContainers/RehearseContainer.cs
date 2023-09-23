using Corelibs.Basic.DDD;
using Manabu.Entities.Users;
using System.Security.Cryptography;
using System.Text;

namespace Manabu.Entities.RehearseContainers;

public class RehearseContainer : Entity<RehearseContainerId>, IAggregateRoot<RehearseContainerId>
{
    public const string DefaultCollectionName = "rehearseContainers";

    public UserId Owner { get; private set; }
    public string ItemId { get; private set; }
    public string ItemType { get; private set; }
    public DateTime CreatedUtcTime { get; init; }

    public RehearseContainer(
        RehearseContainerId id,
        UserId owner,
        string itemId,
        string itemType) : base(id)
    {
        Owner = owner;
        ItemId = itemId;
        ItemType = itemType;
        CreatedUtcTime = DateTime.UtcNow;
    }
}

public class RehearseContainerId : EntityId
{
    public RehearseContainerId(string userId, string itemId) : base(GenerateGuidHash(userId, itemId))
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
