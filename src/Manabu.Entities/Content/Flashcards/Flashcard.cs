using Corelibs.Basic.DDD;

namespace Manabu.Entities.Content.Flashcards;

public class Flashcard : Entity<FlashcardId>, IAggregateRoot<FlashcardId>
{
    public static string DefaultCollectionName { get; } = "flashcards";

    public string RefId { get; private set; }
    public List<FlashcardText> Questions { get; private set; }
    public List<FlashcardItem> Answers { get; private set; }

    public List<int> LevelIndexes { get; private set; }

    public Flashcard() {}

    public record FlashcardItem(List<FlashcardText> Texts);

    public record FlashcardText(string Value, string Link);
}

public class FlashcardId : EntityId { public FlashcardId(string value) : base(value) { } }
