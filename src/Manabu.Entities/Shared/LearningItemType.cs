namespace Manabu.Entities.Flashcards;

public record LearningItemType(string Value)
{
    public static readonly LearningItemType Conversation = new("conversation");
    public static readonly LearningItemType Lesson = new("lesson");
    public static readonly LearningItemType Phrase = new("phrase");
    public static readonly LearningItemType Word = new("word");
    public static readonly LearningItemType WordMeaning = new("word-meaning");
    public static readonly LearningItemType WordFormation = new("word-formation");
    public static readonly LearningItemType Radical = new("radical");
    public static readonly LearningItemType Kana = new("kana");
    public static readonly LearningItemType Kanji = new("kanji");
    public static readonly LearningItemType Context = new("context");
    public static readonly LearningItemType Audio = new("audio");

    public bool IsContainerItem() =>
        this == Conversation ||
        this == Lesson;

    public bool IsLearningItem() =>
        this == Phrase ||
        this == WordMeaning ||
        this == WordFormation ||
        this == Kana ||
        this == Kanji;
}
