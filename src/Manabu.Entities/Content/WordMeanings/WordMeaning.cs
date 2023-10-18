using Corelibs.Basic.DDD;
using Manabu.Entities.Content.Words;

namespace Manabu.Entities.Content.WordMeanings;

public class WordMeaning : Entity<WordMeaningId>, IAggregateRoot<WordMeaningId>
{
    public static string DefaultCollectionName { get; } = "wordMeanings";

    public string Original { get; private set; }
    public List<string> Translations { get; private set; }
    public List<HiraganaWriting>? HiraganaWritings { get; private set; }
    public List<Persona>? Personas { get; private set; }
    public string? PitchAccent { get; set; }
    public bool? KanjiWritingPreferred { get; set; }

    public WordMeaning(
        WordMeaningId id,
        string original,
        List<string> translations,
        List<Persona> personas,
        List<HiraganaWriting> hiraganaWritings) : base(id)
    {
        Original = original;
        Translations = translations;
        Personas = personas;
        HiraganaWritings = hiraganaWritings;
    }

    public record HiraganaWriting(string Value, List<Persona>? Properties = null);
    public record Persona(List<PersonaProperty>? Properties = null);

    private static WordMeaning WatashiMeaning = new WordMeaning(
        id: new WordMeaningId(""),
        original: "私",
        translations: new() { "I", "me" },
        personas: new()
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
        },
        hiraganaWritings: new()
        {
            new HiraganaWriting("わたし"),
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
