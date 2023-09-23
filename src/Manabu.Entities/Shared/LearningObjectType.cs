namespace Manabu.Entities.Shared;

public record LearningObjectType(string Value)
{
    public bool IsContainerItem() =>
        this == LearningContainerType.Conversation ||
        this == LearningContainerType.Lesson;

    public bool IsLearningItem() =>
        this == LearningItemType.Phrase ||
        this == LearningItemType.WordMeaning ||
        this == LearningItemType.WordFormation ||
        this == LearningItemType.Kana ||
        this == LearningItemType.Kanji;

    public bool IsPropertyType() =>
        this == LearningPropertyType.Audio ||
        this == LearningPropertyType.Context ||
        this == LearningPropertyType.Original ||
        this == LearningPropertyType.Translation;
}

public record LearningContainerType(string Value) : LearningObjectType(Value)
{
    public static readonly LearningContainerType Conversation = new("conversation");
    public static readonly LearningContainerType Lesson = new("lesson");
}

public record LearningPropertyType(string Value) : LearningObjectType(Value)
{
    public static readonly LearningPropertyType Audio = new("audio");
    public static readonly LearningPropertyType Context = new("context");
    public static readonly LearningPropertyType Original = new("original");
    public static readonly LearningPropertyType Translation = new("translation");
}

public record LearningItemType(string Value) : LearningObjectType(Value)
{
    public static readonly LearningItemType Phrase = new("phrase");
    public static readonly LearningItemType Word = new("word");
    public static readonly LearningItemType WordMeaning = new("word-meaning");
    public static readonly LearningItemType WordFormation = new("word-formation");
    public static readonly LearningItemType Radical = new("radical");
    public static readonly LearningItemType Kana = new("kana");
    public static readonly LearningItemType Kanji = new("kanji");
}
