using Manabu.Entities.Rehearse.RehearseItems;

namespace Manabu.Entities.Tests;

public class SpacedRepetitionFunctionsTests
{
    [Test]
    public void Test()
    {
        float ef = 2.5f;
        int repsDone = 0;
        int repsInterval = 0;

        AssertEFactor(2.5f, 1, 1, ref ef, ref repsDone, ref repsInterval);
        AssertEFactor(2.5f, 2, 1, ref ef, ref repsDone, ref repsInterval);
        AssertEFactor(2.5f, 3, 1, ref ef, ref repsDone, ref repsInterval);
        AssertEFactor(2.5f, 4, 2, ref ef, ref repsDone, ref repsInterval);
        AssertEFactor(2.5f, 5, 2, ref ef, ref repsDone, ref repsInterval);
        AssertEFactor(2.5f, 6, 2, ref ef, ref repsDone, ref repsInterval);
        AssertEFactor(2.5f, 7, 3, ref ef, ref repsDone, ref repsInterval);
        AssertEFactor(2.5f, 8, 4, ref ef, ref repsDone, ref repsInterval);
        AssertEFactor(2.5f, 9, 8, ref ef, ref repsDone, ref repsInterval);
        AssertEFactor(2.5f, 10, 16, ref ef, ref repsDone, ref repsInterval);
    }

    private void AssertEFactor(
        float ef, int repsDone, int repsInterval,
        ref float efResult, ref int repsDoneResult, ref int repsIntervalResult)
    {
        efResult = SpacedRepetitionFunctions.CalculateEFactorAndNextDayInterval(
           Difficulty.Easy, efResult, ref repsDoneResult, ref repsIntervalResult, out bool asap);

        Assert.That(efResult, Is.EqualTo(ef));
        Assert.That(repsDoneResult, Is.EqualTo(repsDone));
        Assert.That(repsIntervalResult, Is.EqualTo(repsInterval));
    }
}
