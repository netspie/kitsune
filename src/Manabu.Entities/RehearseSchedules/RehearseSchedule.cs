using Corelibs.Basic.DDD;
using Manabu.Entities.Users;

namespace Manabu.Entities.RehearseSchedules;

public class RehearseSchedule : Entity<RehearseScheduleId>, IAggregateRoot<RehearseScheduleId>
{
    public const string DefaultCollectionName = "rehearseSchedules";

    public UserId Owner { get; private set; }
    public bool IsOfficial { get; private set; }
    public List<RepetitionDay> Days { get; private set; }

    public RehearseSchedule(
        bool isOfficial = false,
        UserId owner = null)
    {
        IsOfficial = isOfficial;
        Owner = owner;
    }

    public record RepetitionDay(
        int Day, int Sessions);
}

public class RehearseScheduleId : EntityId { public RehearseScheduleId(string value) : base(value) { } }
