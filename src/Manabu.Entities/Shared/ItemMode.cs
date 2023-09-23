namespace Manabu.Entities.Shared;

public record class LearningItemProperty(string Value)
{
    public static readonly LearningItemProperty Original = new("original");
    public static readonly LearningItemProperty Translation = new("translation");
}
