namespace Manabu.Entities.Shared;

public record LearningMode(string Value)
{
    public static readonly LearningMode Reading = new("reading");
    public static readonly LearningMode Listening = new("listening");
    public static readonly LearningMode Speaking = new("speaking");
    public static readonly LearningMode Writing = new("writing");
}

public static class LearningModeExtensions
{
    public static LearningMode[] GetLearningModes(
        this LearningItemType type)
    {
        if (type == LearningItemType.Phrase)
            return new[]
            {
                LearningMode.Reading,
                LearningMode.Listening,
                LearningMode.Speaking
            };

        return Array.Empty<LearningMode>();
    }
}
