using Manabu.Entities.RehearseSchedules;
using Manabu.Entities.Users;
using System.Globalization;

namespace Manabu.Entities.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public void CalculateValue()
    {
        var userId = new UserId("");
        var schedule = new RehearseSchedule(userId, isOfficial: true)
        {
            Days = new()
            {
                new(DayNumber: 0, Sessions: 1, DayBreakCount: -1)
            }
        };

        DateTime.TryParseExact(
            "2023-09-20 14:30:00",
            "yyyy-MM-dd HH:mm:ss",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out DateTime lastTimeResearched);

        var value = schedule.CalculateValue(
            Difficulty.Normal,
            itemPivotIndex: 0,
            lastTimeResearched,
            dayHourCount: 0.001953125f); // 0.1171875m
    }
}
