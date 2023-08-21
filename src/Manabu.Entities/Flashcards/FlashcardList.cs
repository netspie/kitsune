using Corelibs.Basic.DDD;

namespace Manabu.Entities.Flashcards;

public class FlashcardListId : EntityId { public FlashcardListId(string value) : base(value) {} }

public class FlashcardList : Entity<FlashcardListId>, IAggregateRoot<FlashcardListId>
{
    public string Type { get; private set; }
    public string Mode { get; private set; }
    public List<FlashcardId> Flashcards { get; private set; }
}

public class FlashcardId : EntityId { public FlashcardId(string value) : base(value) {} }

public class Flashcard : Entity<FlashcardId>, IAggregateRoot<FlashcardId>
{
    public List<FlashcardText> Questions { get; private set; }
    public List<FlashcardItem> Answers { get; private set; }

    public List<int> LevelIndexes { get; private set; }


    public Flashcard() {}

    public record FlashcardItem(List<FlashcardText> Texts);

    public record FlashcardText(string Value, string Link);
}
