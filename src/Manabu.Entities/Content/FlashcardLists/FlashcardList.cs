using Corelibs.Basic.DDD;
using Manabu.Entities.Flashcards;
using Manabu.Entities.Shared;
using Manabu.Entities.Content.FlashcardLists;
using Manabu.Entities.Content.Flashcards;

namespace Manabu.Entities.Content.FlashcardLists;

public class FlashcardList : Entity<FlashcardListId>, IAggregateRoot<FlashcardListId>
{
    public const string DefaultCollectionName = "flashcardLists";

    public string ParentItemId { get; private set; }

    public string Type { get; private set; }
    public string Mode { get; private set; }
    public List<FlashcardId> Flashcards { get; private set; }

    public FlashcardList(string type, string mode, List<FlashcardId> flashcards)
    {
        Type = type;
        Mode = mode;
        Flashcards = flashcards;
    }
}

public class FlashcardListId : LearningItemId { public FlashcardListId(string value) : base(value) { } }
