using Corelibs.Basic.DDD;
using Manabu.Entities.Content.Words;
using Manabu.Entities.Shared;

namespace Manabu.Entities.Content.WordLexemes;

public class WordLexeme : Entity<WordLexemeId>, IAggregateRoot<WordLexemeId>
{
    public const string DefaultCollectionName = "wordLexemes";

    public PartOfSpeech PartOfSpeech { get; private set; }
    public string Lemma { get; private set; }
    public List<WordInflectionPair> Inflections { get; private set; }

    public WordLexeme(
        PartOfSpeech partOfSpeech, string lemma)
    {
        PartOfSpeech = partOfSpeech;
        Lemma = lemma;
    }

    public WordLexeme(
        WordLexemeId id,
        uint version,
        PartOfSpeech partOfSpeech,
        string lemma) : base(id, version)
    {
        PartOfSpeech = partOfSpeech;
        Lemma = lemma;
    }
}

public class WordLexemeId : EntityId { public WordLexemeId(string value) : base(value) { } }

public record WordInflectionPair(
    string Name,
    WordInflectionForm Informal,
    WordInflectionForm? Formal = null);

public record WordInflectionForm(
    WordInflection Positive,
    WordInflection? Negative = null);

public record WordInflection(
    string Value,
    string Description,
    string? Translation);
