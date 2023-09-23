using Corelibs.Basic.DDD;
using Manabu.Entities.Content.WordLexemes;
using Manabu.Entities.Content.WordMeanings;
using Manabu.Entities.Shared;

namespace Manabu.Entities.Content.Words;

public class Word : Entity<WordId>, IAggregateRoot<WordId>
{
    public const string DefaultCollectionName = "words";

    public string Value { get; private set; }
    public PartOfSpeech PartOfSpeech { get; private set; }
    public List<WordMeaningId> Meanings { get; private set; }
    public WordLexemeId? Lexeme {  get; private set; }

    public Word(string value)
    {
        Value = value;
    }

    public Word(
        WordId id,
        uint version,
        string value,
        PartOfSpeech partOfSpeech,
        List<WordMeaningId> meanings,
        WordLexemeId lexeme) : base(id, version)
    {
        Value = value;
        PartOfSpeech = partOfSpeech;
        Meanings = meanings;
        Lexeme = lexeme;
    }
}

public class WordId : LearningItemId { public WordId(string value) : base(value) {} }

public record PartOfSpeech(string value)
{
    public static readonly PartOfSpeech Pronoun = new("pronoun");
    public static readonly PartOfSpeech Noun = new("noun");
    public static readonly PartOfSpeech Verb = new("verb");
    public static readonly PartOfSpeech Adjective = new("adjective");
}

public record InflectionType(string value)
{
    public static readonly InflectionType Present = new("present");
    public static readonly InflectionType Past = new("past");
}
