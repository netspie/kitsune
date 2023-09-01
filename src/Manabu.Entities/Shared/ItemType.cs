namespace Manabu.Entities.Flashcards;

public record ItemType(string Value)
{
    public static readonly ItemType Conversation = new("conversation");
    public static readonly ItemType Lesson = new("lesson");
    public static readonly ItemType Phrase = new("phrase");
    public static readonly ItemType Word = new("word");
    public static readonly ItemType WordMeaning = new("word-meaning");
    public static readonly ItemType WordFormation = new("word-formation");
    public static readonly ItemType Radical = new("radical");
    public static readonly ItemType Kana = new("kana");
    public static readonly ItemType Kanji = new("kanji");
    public static readonly ItemType Context = new("context");
    public static readonly ItemType Audio = new("audio");
}
