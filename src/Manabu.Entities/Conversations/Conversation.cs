using Corelibs.Basic.DDD;
using Manabu.Entities.Phrases;

namespace Manabu.Entities.Conversations;

public class Conversation : Entity<ConversationId>, IAggregateRoot<ConversationId>
{
    public const string DefaultCollectionName = "conversations";

    public List<PhraseId> Phrases { get; private set; }
    public List<string> Speakers { get; private set; }
    public List<int> SpeakersOrder { get; private set; }

    public string Name { get; private set; }
    public string Description { get; private set; }

    public Conversation(
        string name,
        string description)
    {
        Name = name;
        Description = description;
    }

    public Conversation(
        ConversationId id,
        uint version,
        string name,
        string description,
        List<string> speakers,
        List<int> speakersOrder) : base(id, version)
    {
        Name = name;
        Description = description;
        Speakers = speakers;
        SpeakersOrder = speakersOrder;
    }
}

public class ConversationId : EntityId { public ConversationId(string value) : base(value) {} }
