using Corelibs.Basic.Collections;
using Corelibs.Basic.DDD;
using Manabu.Entities.Content.Audios;
using Manabu.Entities.Content.Conversations;
using Manabu.Entities.Content.Users;
using Manabu.Entities.Content.WordMeanings;

namespace Manabu.Entities.Content.Phrases;

public class Phrase : Entity<PhraseId>, IAggregateRoot<PhraseId>
{
    public static string DefaultCollectionName { get; } = "phrases";

    public UserId Owner { get; private set; }
    public string Original { get; set; }
    public List<string>? Translations { get; set; }
    public List<AudioId>? Audios { get; set; }
    public List<string>? Contexts { get; set; }
    public List<WordLink>? WordMeanings { get; private set; }
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

    public bool AddWord(WordLink word)
    {
        if (!WordMeanings!.IsNullOrEmpty() && WordMeanings!.Contains(word)) 
            return false;

        WordMeanings ??= new();
        WordMeanings.Add(word);

        return true;
    }

    public bool RemoveWord(WordLink word)
    {
        if (WordMeanings!.IsNullOrEmpty() || !WordMeanings!.Contains(word))
            return false;

        return WordMeanings.Remove(word);
    }
}

public class PhraseId : EntityId { public PhraseId(string value) : base(value) { } }

public record WordLink(
    WordMeaningId WordMeaningId,
    string? WordInflectionId = null,
    string? Reading = null,
    WritingMode? WritingMode = null,
    string? CustomWriting = null);

public record WritingMode(string Value)
{
    public static readonly WritingMode Hiragana = new("hiragana");
    public static readonly WritingMode Katakana = new("katakana");
    public static readonly WritingMode Custom = new("custom");
}

public record ValueIndex(string Value, int Index);
