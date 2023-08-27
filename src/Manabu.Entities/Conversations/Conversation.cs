using Corelibs.Basic.DDD;
using Manabu.Entities.Phrases;
using Manabu.Entities.Users;
using Manabu.Entities.Lessons;
using Corelibs.Basic.Collections;

namespace Manabu.Entities.Conversations;

public class Conversation : Entity<ConversationId>, IAggregateRoot<ConversationId>
{
    public const string DefaultCollectionName = "conversations";

    public UserId Owner { get; private set; }
    public List<PhraseId> Phrases { get; private set; }
    public List<LessonId> Lessons { get; private set; }
    public List<string> Speakers { get; private set; }
    public List<int> SpeakersOrder { get; private set; }

    public string Name { get; private set; }
    public string Description { get; private set; }

    public Conversation(
        string name,
        LessonId lessonId,
        UserId owner)
    {
        Name = name;
        Lessons = new() { lessonId };
        Owner = owner;
    }

    public void AddPhrase(PhraseId phrase, 
        int index = 0)
    {
        Phrases ??= new();
        Phrases.InsertClamped(phrase, index);
    }
}

public class ConversationId : EntityId { public ConversationId(string value) : base(value) {} }
