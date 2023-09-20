using Corelibs.Basic.DDD;
using Manabu.Entities.Users;

namespace Manabu.Entities.RehearseContainers;

public class RehearseContainer : Entity<RehearseContainerId>, IAggregateRoot<RehearseContainerId>
{
    public const string DefaultCollectionName = "rehearseContainers";

    public UserId Owner { get; private set; }
    public string ItemId { get; private set; }

    public RehearseContainer(
        UserId owner,
        string itemId)
    {
        Owner = owner;
        ItemId = itemId;
    }
}

public class RehearseContainerId : EntityId { public RehearseContainerId(string value) : base(value) { } }
