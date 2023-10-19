using Corelibs.Basic.DDD;
using Manabu.Entities.Content.Words;

namespace Manabu.Entities.Content.WordMeanings;

public class WordMeaning : Entity<WordMeaningId>, IAggregateRoot<WordMeaningId>
{
    public static string DefaultCollectionName { get; } = "wordMeanings";

    public WordId WordId { get; init; }
    public string Original { get; private set; }
    public List<string> Translations { get; private set; }
    public List<HiraganaWriting>? HiraganaWritings { get; private set; }
    public string? PitchAccent { get; set; }
    public bool? KanjiWritingPreferred { get; set; }

    public WordMeaning(
        WordMeaningId id,
        WordId wordId,
        string original,
        List<string> translations,
        List<HiraganaWriting> hiraganaWritings) : base(id)
    {
        WordId = wordId;
        Original = original;
        Translations = translations;
        HiraganaWritings = hiraganaWritings;
    }

    public record HiraganaWriting(string Value, List<Persona>? Properties = null);
    public record Persona(List<PersonaProperty>? Properties = null);

    private static WordMeaning WatashiMeaning = new WordMeaning(
        id: new WordMeaningId(""),
        wordId: new WordId(""),
        original: "私",
        translations: new() { "I", "me" },
        hiraganaWritings: new()
        {
            new HiraganaWriting("わたし", new()
            {
                new Persona(Properties: new()
                {
                    Gender.Female,
                }),
                new Persona(Properties: new()
                {
                    Gender.Male,
                    Formality.Formal,
                })
            }),
            new HiraganaWriting("わたくし", new()
            {
                new Persona(Properties: new()
                {
                    Gender.Female,
                    Age.Young,
                    Formality.Informal
                })
            })
        });
}

public class WordMeaningId : EntityId { public WordMeaningId(string value) : base(value) {} }
