using Corelibs.Basic.Collections;
using Corelibs.Basic.DDD;
using Manabu.Entities.Lessons;
using Manabu.Entities.Phrases;
using Manabu.Entities.Users;
using System;

namespace Manabu.Entities.Conversations;

public class Conversation : Entity<ConversationId>, IAggregateRoot<ConversationId>
{
    public const string DefaultCollectionName = "conversations";

    public UserId Owner { get; private set; }
    public List<PhraseData> Phrases { get; private set; }
    public List<LessonId> Lessons { get; private set; }

    public string Name { get; set; }
    public string Description { get; set; }

    public Conversation(
        string name,
        LessonId lessonId,
        UserId owner)
    {
        Name = name;
        Lessons = new() { lessonId };
        Owner = owner;
    }

    public void AddPhrase(
        PhraseId phrase, 
        int index = int.MaxValue)
    {
        Phrases ??= new();
        Phrases.InsertClamped(new("", phrase), index);
    }

    public bool ChangeSpeaker(string speaker, PhraseId phrase, int index = -1)
    {
        var phraseData = Phrases.Get(p => p.Phrase == phrase, index, out var foundIndex);
        if (phraseData is null || speaker is null || speaker == phraseData.Speaker)
            return false;
        
        Phrases[foundIndex] = phraseData with { Speaker = speaker };
        return true;
    }

    public bool MovePhrases(IEnumerable<PhraseId> phrases, Conversation newConversation, int index = int.MaxValue)
    {
        foreach (var phrase in phrases)
        {
            if (!Phrases.RemoveIf(p => p.Phrase == phrase))
                return false;

            newConversation.AddPhrase(phrase, index);
        }

        return true;
    }

    public record PhraseData(string Speaker, PhraseId Phrase);
}

public class ConversationId : EntityId { public ConversationId(string value) : base(value) {} }
