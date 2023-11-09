using Corelibs.Basic.DDD;
using Manabu.Entities.Content.Words;

namespace Manabu.Entities.Content.WordLexemes;

public class WordLexeme : Entity<WordLexemeId>, IAggregateRoot<WordLexemeId>
{
    public static string DefaultCollectionName { get; } = "wordLexemes";

    public PartOfSpeech PartOfSpeech { get; private set; }
    public string Lemma { get; private set; }
    public List<WordInflectionPair> Inflections { get; private set; }

    public WordLexeme(
        WordLexemeId id,
        PartOfSpeech partOfSpeech,
        string lemma,
        List<WordInflectionPair> inflections) : base(id)
    {
        PartOfSpeech = partOfSpeech;
        Lemma = lemma;
        Inflections = inflections;
    }
}

public class WordLexemeId : EntityId { public WordLexemeId(string value) : base(value) {} }

public record WordInflectionPair(
    InflectionType Type,
    WordInflectionForm Informal,
    WordInflectionForm? Formal = null);

public record WordInflectionForm(
    WordInflection Positive,
    WordInflection? Negative = null);

public record WordInflection(
    string Value,
    string? Translation = null);
