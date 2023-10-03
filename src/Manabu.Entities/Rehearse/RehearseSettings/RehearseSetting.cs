using Corelibs.Basic.DDD;
using Manabu.Entities.Content.Users;

namespace Manabu.Entities.Rehearse.RehearseSettings;

public class RehearseSetting : Entity<RehearseSettingId>, IAggregateRoot<RehearseSettingId>
{
    public static string DefaultCollectionName { get; } = "rehearseSettings";

    public UserId Owner { get; private set; }

    public int MaxSpacedItemsPerDay { get; set; }
    public int CurrentSpacedItemsInDay { get; set; }

    public RehearseSetting(
        UserId owner)
    {
        Owner = owner;
    }
}

public class RehearseSettingId : EntityId { public RehearseSettingId(string value) : base(value) { } }
