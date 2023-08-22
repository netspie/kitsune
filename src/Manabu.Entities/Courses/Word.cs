using Corelibs.Basic.DDD;

namespace Manabu.Entities.Courses;

public class Word : Entity<WordId>, IAggregateRoot<WordId>
{
    public string Value { get; private set; }
    public List<WordMeaningId> Meanings { get; private set; }

    public Word(string value)
    {
        Value = value;
    }

    public Word(
        WordId id,
        uint version,
        string value,
        List<WordMeaningId> meanings) : base(id, version)
    {
        Value = value;
        Meanings = meanings;
    }
}

public class WordId : EntityId { public WordId(string value) : base(value) { } }
