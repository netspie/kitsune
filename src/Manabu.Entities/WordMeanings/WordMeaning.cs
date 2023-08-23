using Corelibs.Basic.DDD;

namespace Manabu.Entities.WordMeanings;

public class WordMeaning : Entity<WordMeaningId>, IAggregateRoot<WordMeaningId>
{
    public string Original { get; private set; }
    public List<string> Translations { get; private set; }
    public string HiraganaWriting { get; private set; }
    public string PitchAccent { get; private set; }
    public WritingUsualness? WritingUsualness { get; private set; }

    public WordMeaning(string name)
    {

    }

    public WordMeaning(
        WordMeaningId id,
        uint version,
        string original,
        List<string> translations) : base(id, version)
    {
        Original = original;
        Translations = translations;
    }
}

public class WordMeaningId : EntityId { public WordMeaningId(string value) : base(value) { } }

public record WritingUsualness(string Value)
{
    public static readonly WritingUsualness KanaAlone = new("kana");
    public static readonly WritingUsualness Kanji = new("kanji");
}
