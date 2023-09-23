using Corelibs.Basic.DDD;
using Manabu.Entities.RehearseSchedules;
using Manabu.Entities.Users;

namespace Manabu.Entities.RehearseSettings;

public class RehearseSetting : Entity<RehearseSettingId>, IAggregateRoot<RehearseSettingId>
{
    public const string DefaultCollectionName = "rehearseSettings";

    public UserId Owner { get; private set; }

    public RehearseScheduleId RehearseSchedule { get; set; }

    public RehearseSetting(
        UserId owner)
    {
        Owner = owner;
    }
}

public class RehearseSettingId : EntityId { public RehearseSettingId(string value) : base(value) { } }
