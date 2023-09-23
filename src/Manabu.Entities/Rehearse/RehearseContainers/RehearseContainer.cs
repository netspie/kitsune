using Corelibs.Basic.DDD;
using Corelibs.Basic.Encryption;
using Manabu.Entities.Content.Users;

namespace Manabu.Entities.Rehearse.RehearseContainers;

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
    public RehearseContainerId(string userId, string itemId) : base(EncryptionFunctions.GenerateGuidHash(userId, itemId))
    {
    }
}
