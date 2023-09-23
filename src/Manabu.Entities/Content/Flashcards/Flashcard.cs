using Corelibs.Basic.DDD;
using Manabu.Entities.Shared;

namespace Manabu.Entities.Flashcards;

public class Flashcard : Entity<FlashcardId>, IAggregateRoot<FlashcardId>
{
    public const string DefaultCollectionName = "flashcards";

    public string RefId { get; private set; }
    public List<FlashcardText> Questions { get; private set; }
    public List<FlashcardItem> Answers { get; private set; }

    public List<int> LevelIndexes { get; private set; }

    public Flashcard() {}

    public record FlashcardItem(List<FlashcardText> Texts);

    public record FlashcardText(string Value, string Link);
}

public class FlashcardId : LearningItemId { public FlashcardId(string value) : base(value) { } }
