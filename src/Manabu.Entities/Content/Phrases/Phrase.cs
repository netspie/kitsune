using Corelibs.Basic.DDD;
using Manabu.Entities.Content.Audios;
using Manabu.Entities.Content.Conversations;
using Manabu.Entities.Content.Users;
using Manabu.Entities.Content.WordMeanings;
using Manabu.Entities.Shared;

namespace Manabu.Entities.Content.Phrases;

public class Phrase : Entity<PhraseId>, IAggregateRoot<PhraseId>
{
    public const string DefaultCollectionName = "phrases";

    public UserId Owner { get; private set; }
    public string Original { get; set; }
    public List<string> Translations { get; set; }
    public List<AudioId> Audios { get; set; }
    public List<string> Contexts { get; set; }
    public List<WordMeaningId> WordMeanings { get; private set; }
    public List<ConversationId>? Conversations { get; private set; }

    public Phrase(
        UserId owner,
        string original,
        ConversationId? conversation = null)
    {
        Owner = owner;
        Original = original;
        Conversations = conversation is not null ? 
            new() { conversation } : null;
    }

    public void AddAudio(AudioId audio)
    {
        Audios ??= new();
        Audios.Add(audio);
    }

    public bool Move(ConversationId from, ConversationId to)
    {
        if (Conversations.Contains(to))
            return false;

        if (!Conversations.Remove(from))
            return false;

        Conversations.Add(to);

        return true;
    }
}

public class PhraseId : LearningItemId { public PhraseId(string value) : base(value) { } }
