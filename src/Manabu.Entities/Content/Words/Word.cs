using Corelibs.Basic.DDD;
using Manabu.Entities.Content.WordLexemes;
using Manabu.Entities.Content.WordMeanings;

namespace Manabu.Entities.Content.Words;

public class Word : Entity<WordId>, IAggregateRoot<WordId>
{
    public static string DefaultCollectionName { get; } = "words";

    public string Value { get; private set; }
    public List<PartOfSpeech> PartsOfSpeech { get; init; }
    public List<WordMeaningId> Meanings { get; private set; }
    public List<WordProperty>? Properties { get; private set; }
    public WordLexemeId? Lexeme { get; private set; }

    public Word(
        WordId id,
        string value,
        List<PartOfSpeech> partsOfSpeech,
        List<WordMeaningId> meanings,
        List<WordProperty>? properties = null,
        WordLexemeId? lexeme = null) : base(id)
    {
        Value = value;
        PartsOfSpeech = partsOfSpeech;
        Meanings = meanings;
        Properties = properties;
        Lexeme = lexeme;
    }
}

public class WordId : EntityId { public WordId(string value) : base(value) {} }

public abstract record WordProperty(string Value);

public record PartOfSpeech(string Value)
{
    public static readonly PartOfSpeech Particle = new("particle");
    public static readonly PartOfSpeech Pronoun = new("pronoun");
    public static readonly PartOfSpeech Noun = new("noun");
    public static readonly PartOfSpeech Verb = new("verb");
    public static readonly PartOfSpeech Adjective = new("adjective");
    public static readonly PartOfSpeech Adverb = new("adverb");
    public static readonly PartOfSpeech Conjunction = new("conjunction");
    public static readonly PartOfSpeech AuxilaryVerb = new("auxilary-verb");
    public static readonly PartOfSpeech Numeral = new("numeral");
    public static readonly PartOfSpeech Classifier = new("classifier");
    public static readonly PartOfSpeech Counter = new("counter");
    public static readonly PartOfSpeech Preposition = new("preposition");
    public static readonly PartOfSpeech Interjection = new("interjection");
    public static readonly PartOfSpeech Name = new("name");
}

public record VerbTransitivity(string Value) : WordProperty(Value)
{
    public static readonly VerbTransitivity Transitive = new("transitive");
    public static readonly VerbTransitivity Intransitive = new("intransitive");
}

public record VerbConjugationType(string Value) : WordProperty(Value)
{
    public static readonly VerbConjugationType Godan = new("present");
    public static readonly VerbConjugationType Ichidan = new ("ichidan");
    public static readonly VerbConjugationType Irregular = new ("irregular");
    public static readonly VerbConjugationType Suru = new ("suru");
}

public record AdjectiveConjugationType(string Value) : WordProperty(Value)
{
    public static readonly AdjectiveConjugationType I = new("i-adjective");
    public static readonly AdjectiveConjugationType Na = new("na-adjective");
    public static readonly AdjectiveConjugationType No = new("no-adjective");
}

public record NounType(string Value) : WordProperty(Value)
{
    public static readonly NounType Verbal = new("verbal-noun");
}

public record InflectionType(string Value)
{
    public static readonly InflectionType Present = new("present");
    public static readonly InflectionType Past = new("past");
}

public record PersonaProperty(string Value)
{
    public string Name => GetType().Name;
}

public record Age(string Value) : PersonaProperty(Value)
{
    public static readonly Age Young = new("young");
    public static readonly Age Old = new("old");
}

public record Gender(string Value) : PersonaProperty(Value)
{
    public static readonly Gender Female = new("female");
    public static readonly Gender Male = new("male");
}

public record Dialect(string Value) : PersonaProperty(Value)
{
    public static readonly Gender Kansai = new("kansai");
    public static readonly Gender Kanto = new("kanto");
}

public record Formality(string Value) : PersonaProperty(Value)
{
    public static readonly Gender Rude = new("rude");
    public static readonly Gender Informal = new("informal");
    public static readonly Gender Formal = new("formal");
    public static readonly Gender HumbleHonorific = new("humble-honorific");
}
