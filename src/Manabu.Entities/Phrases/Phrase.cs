using Corelibs.Basic.DDD;
using Manabu.Entities.Audios;
using Manabu.Entities.Words;

namespace Manabu.Entities.Phrases;

public class Phrase : Entity<PhraseId>, IAggregateRoot<PhraseId>
{
    public const string DefaultCollectionName = "phrases";

    public string Original { get; private set; }
    public List<string> Translations { get; private set; }
    public List<AudioId> Audios { get; private set; }
    public List<string> Contexts { get; private set; }
    public List<WordId> Words { get; private set; }

    public Phrase(
        string original,
        List<string> translations,
        List<AudioId> audios,
        List<string> contexts)
    {
        Original = original;
        Translations = translations;
        Audios = audios;
        Contexts = contexts;
    }

    public Phrase(
        PhraseId id,
        uint version,
        string original,
        List<string> translations,
        List<AudioId> audios,
        List<string> contexts,
        List<WordId> words) : base(id, version)
    {
        Original = original;
        Translations = translations;
        Audios = audios;
        Contexts = contexts;
        Words = words;
    }
}

public class PhraseId : EntityId { public PhraseId(string value) : base(value) { } }

