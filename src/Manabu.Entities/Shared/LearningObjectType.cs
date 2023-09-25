namespace Manabu.Entities.Shared;

public class LearningObjectType
{
    public string Value { get; }

    public LearningObjectType(string value) =>
        Value = value;

    public bool IsContainerItem() =>
        Value == LearningContainerType.Conversation.Value ||
        Value == LearningContainerType.Lesson.Value;

    public bool IsLearningItem() =>
        Value == LearningItemType.Phrase.Value ||
        Value == LearningItemType.WordMeaning.Value ||
        Value == LearningItemType.WordFormation.Value ||
        Value == LearningItemType.Kana.Value ||
        Value == LearningItemType.Kanji.Value;

    public bool IsProperty() =>
        Value == LearningPropertyType.Audio.Value ||
        Value == LearningPropertyType.Context.Value ||
        Value == LearningPropertyType.Original.Value ||
        Value == LearningPropertyType.Translation.Value;

    public override bool Equals(object obj)
    {
        if (obj is LearningObjectType other)
            return Value == other.Value;
        
        return false;
    }

    public override int GetHashCode() =>
        Value.GetHashCode();

    public static bool operator ==(LearningObjectType left, LearningObjectType right) =>
        left.Equals(right);

    public static bool operator !=(LearningObjectType left, LearningObjectType right) =>
        !(left == right);
}

public class LearningContainerType
{
    public string Value { get; }

    public static readonly LearningContainerType Conversation = new LearningContainerType("conversation");
    public static readonly LearningContainerType Lesson = new LearningContainerType("lesson");

    public LearningContainerType(string value)
    {
        Value = value;
    }

    public LearningObjectType ToObjectType() => new(Value);
}

public class LearningPropertyType
{
    public string Value { get; }

    public static readonly LearningPropertyType Audio = new LearningPropertyType("audio");
    public static readonly LearningPropertyType Context = new LearningPropertyType("context");
    public static readonly LearningPropertyType Original = new LearningPropertyType("original");
    public static readonly LearningPropertyType Translation = new LearningPropertyType("translation");

    public LearningPropertyType(string value)
    {
        Value = value;
    }

    public LearningObjectType ToObjectType() => new(Value);
}

public class LearningItemType
{
    public string Value { get; }

    public static readonly LearningItemType Phrase = new("phrase");
    public static readonly LearningItemType Word = new("word");
    public static readonly LearningItemType WordMeaning = new("word-meaning");
    public static readonly LearningItemType WordFormation = new("word-formation");
    public static readonly LearningItemType Radical = new("radical");
    public static readonly LearningItemType Kana = new("kana");
    public static readonly LearningItemType Kanji = new("kanji");

    public LearningItemType(string value)
    {
        Value = value;
    }

    public LearningObjectType ToObjectType() => new(Value);
}
