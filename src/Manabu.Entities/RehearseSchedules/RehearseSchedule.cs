using Corelibs.Basic.DDD;
using Manabu.Entities.Users;

namespace Manabu.Entities.RehearseSchedules;

public class RehearseSchedule : Entity<RehearseScheduleId>, IAggregateRoot<RehearseScheduleId>
{
    public const string DefaultCollectionName = "rehearseSchedules";

    public UserId Owner { get; private set; }
    public bool IsOfficial { get; private set; }
    public List<RepetitionCheckpoint> Days { get; set; }

    public RehearseSchedule(
        UserId owner,
        bool isOfficial)
    {
        IsOfficial = isOfficial;
        Owner = owner;
    }

    public record RepetitionCheckpoint(
        int DayNumber, int Sessions, int DayBreakCount = -1);
}

public static class RehearseScheduleExtensions
{
    public static float CalculateValue(
       this RehearseSchedule schedule,
       Difficulty difficulty,
       int itemPivotIndex,
       DateTime lastTimeResearched,
       float dayHourCount = 24)
    {
        float difficultyFactor = 1.0f;

        if (difficulty == Difficulty.Easy)
            difficultyFactor = 0.2f;
        else if (difficulty == Difficulty.Normal)
            difficultyFactor = 0.5f;
        else if (difficulty == Difficulty.Hard)
            difficultyFactor = 0.8f;

        var timeSinceLastReview = DateTime.Now - lastTimeResearched;
        float timeFactor = (float) timeSinceLastReview.TotalHours;

        float customDays = timeFactor / dayHourCount;

        float value = difficultyFactor / customDays;

        value = Math.Clamp(value, 0.0f, 1.0f);

        return value;
    }
}

public record Difficulty(string Value)
{
    public static readonly Difficulty Impossible = new(nameof(Impossible));
    public static readonly Difficulty Hard = new(nameof(Hard));
    public static readonly Difficulty Challenging = new(nameof(Challenging));
    public static readonly Difficulty Normal = new(nameof(Normal));
    public static readonly Difficulty Easy = new(nameof(Easy));
    public static readonly Difficulty Obvious = new(nameof(Obvious));
}

public class RehearseScheduleId : EntityId { public RehearseScheduleId(string value) : base(value) { } }
