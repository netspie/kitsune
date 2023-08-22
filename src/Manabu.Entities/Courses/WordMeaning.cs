using Corelibs.Basic.DDD;

namespace Manabu.Entities.Courses;

public class WordMeaning : Entity<WordMeaningId>, IAggregateRoot<WordMeaningId>
{
    public string Original { get; private set; }
    public string Translation { get; private set; }

    public WordMeaning(string name)
    {

    }

    public WordMeaning(
        WordMeaningId id,
        uint version,
        string original,
        string translation) : base(id, version)
    {
        Original = original;
        Translation = translation;
    }
}

public class WordMeaningId : EntityId { public WordMeaningId(string value) : base(value) { } }
