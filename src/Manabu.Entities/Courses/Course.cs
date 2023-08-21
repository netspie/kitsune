using Corelibs.Basic.DDD;

namespace Manabu.Entities.Courses;

public class CourseId : EntityId { public CourseId(string value) : base(value) {} }

public class Course : Entity<CourseId>, IAggregateRoot<CourseId>
{
    public string Name { get; private set; }

    public Course(string name)
    {
        Name = name;
    }

    public Course(
        CourseId id, 
        uint version,
        string name) : base(id, version)
    {
        Name = name;
    }
}


public class LessonId : EntityId { public LessonId(string value) : base(value) { } }

public class Lesson : Entity<LessonId>, IAggregateRoot<LessonId>
{
    public string Name { get; private set; }

    public Lesson(string name)
    {
        Name = name;
    }

    public Lesson(
        LessonId id,
        uint version,
        string name) : base(id, version)
    {
        Name = name;
    }
}

public class PhraseId : EntityId { public PhraseId(string value) : base(value) { } }

public class Phrase : Entity<PhraseId>, IAggregateRoot<PhraseId>
{
    public string Original { get; private set; }
    public List<string> Translations { get; private set; }
    public List<string> Contexts { get; private set; }
    public List<WordId> Words { get; private set; }

    public Phrase(
        string original,
        List<string> translations,
        List<string> contexts)
    {
        Original = original;
        Translations = translations;
        Contexts = contexts;
    }

    public Phrase(
        PhraseId id,
        uint version,
        string original,
        List<string> translations,
        List<string> contexts,
        List<WordId> words) : base(id, version)
    {
        Original = original;
        Translations = translations;
        Contexts = contexts;
        Words = words;
    }
}

public class WordId : EntityId { public WordId(string value) : base(value) { } }

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

public class WordMeaningId : EntityId { public WordMeaningId(string value) : base(value) { } }

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
