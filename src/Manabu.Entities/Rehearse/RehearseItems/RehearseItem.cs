using Corelibs.Basic.DDD;
using Corelibs.Basic.Encryption;
using Manabu.Entities.Content.Users;
using Manabu.Entities.Shared;

namespace Manabu.Entities.Rehearse.RehearseItems;

public class RehearseItem : Entity<RehearseItemId>, IAggregateRoot<RehearseItemId>
{
    public const string DefaultCollectionName = "rehearseItems";
    public const float DefaultEFactor = 0.25f;

    public UserId Owner { get; init; }
    public LearningObjectId ItemId { get; init; }
    public LearningItemType ItemType { get; init; }
    public LearningMode Mode { get; init; }
    public DateTime CreatedUtcTime { get; init; }

    public Difficulty Difficulty { get; private set; }
    public int RepsTotal { get; private set; }
    public int RepsInternal { get; private set; }
    public int RepsInterval { get; private set; }
    public float EFactor { get; private set; } = DefaultEFactor; // default
    public DateTime LastRehearsedUtcTime { get; private set; }

    public RehearseItem(
        RehearseItemId id,
        UserId owner,
        LearningObjectId itemId,
        LearningItemType itemType,
        LearningMode mode) : base(id)
    {
        Owner = owner;
        ItemId = itemId;
        ItemType = itemType;
        Mode = mode;
        CreatedUtcTime = DateTime.UtcNow;
    }

    public void Answer(Difficulty difficulty)
    {

    }
}

public class RehearseItemId : EntityId
{
    public RehearseItemId(string id) : base(id)
    {
    }

    public RehearseItemId(UserId userId, LearningObjectId itemId, LearningMode mode)
        : base(EncryptionFunctions.GenerateGuidHash(userId.Value, itemId.Value, mode.Value))
    {
    }
}

public static class SpacedRepetitionFunctions
{
    public static float CalculateEFactorAndNextDayInterval(
        Difficulty difficulty,
        float ef,
        ref int repsDone,
        ref int repsInterval,
        out bool shouldReviewAsap)
    {
        shouldReviewAsap = difficulty < Difficulty.Easy;
        if (difficulty < Difficulty.Challenging)
        {
            repsDone = 1;
            return ef;
        }
        else
        {
            repsInterval = CalculateNextDayInterval(repsDone, ef);
            repsDone++;
            return CalculateEFactor(difficulty, ef);
        }
    }

    /// <summary>
    /// Calculate next e-factor value based on given difficulty and previous e-factor.
    /// </summary>
    /// <param name="q">Given difficulty of rehearse item</param>
    /// <param name="ef">Previous e-factor of the rehearse item</param>
    /// <returns></returns>
    private static float CalculateEFactor(Difficulty q, float ef)
    {
        int maxQ = Difficulty.Obvious.Value;
        var diff = (0.1f - (maxQ - q) * (0.08f + (maxQ - q) * 0.02f));
        ef += diff;
        return ef < 1.3f ? 1.3f : ef;
    }

    private static int CalculateNextDayInterval(int repsDone, float eFactor) =>
        CalculateNextDayInterval(repsDone, (int)Math.Round(eFactor));

    private static int CalculateNextDayInterval(int repsDone, int eFactor)
    {
        return repsDone switch
        {
            1 => 1,
            2 => 1,
            3 => 2,
            4 => 2,
            5 => 2,
            6 => 3,
            7 => 4,
            _ when repsDone > 2 => CalculateNextDayInterval(repsDone - 1, eFactor) * eFactor,
            _ => 1
        };
    }
}