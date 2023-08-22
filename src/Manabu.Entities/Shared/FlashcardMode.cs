namespace Manabu.Entities.Flashcards;

public record FlashcardMode(string Value)
{
    public static readonly FlashcardMode Reading = new("reading");
    public static readonly FlashcardMode Listening = new("listening");
    public static readonly FlashcardMode Speaking = new("speaking");
    public static readonly FlashcardMode Writing = new("writing");
}
