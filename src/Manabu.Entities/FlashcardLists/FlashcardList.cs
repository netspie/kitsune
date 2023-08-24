using Corelibs.Basic.DDD;
using Manabu.Entities.Flashcards;

namespace Manabu.Entities.FlashcardLists;

public class FlashcardListId : EntityId { public FlashcardListId(string value) : base(value) {} }

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
