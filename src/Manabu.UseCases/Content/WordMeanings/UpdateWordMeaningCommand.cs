using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Content.WordMeanings;
using Manabu.Entities.Content.Words;
using Mediator;

namespace Manabu.UseCases.Content.Courses;

public class UpdateWordMeaningCommandHandler : ICommandHandler<UpdateWordMeaningCommand, Result>
{
    private readonly IRepository<WordMeaning, WordMeaningId> _wordMeaningRepository;
    private readonly IRepository<Word, WordId> _wordRepository;

    public UpdateWordMeaningCommandHandler(
        IRepository<WordMeaning, WordMeaningId> wordMeaningRepository, 
        IRepository<Word, WordId> wordRepository)
    {
        _wordMeaningRepository = wordMeaningRepository;
        _wordRepository = wordRepository;
    }

    public async ValueTask<Result> Handle(UpdateWordMeaningCommand command, CancellationToken ct)
    {
        var result = Result.Success();

        var wordMeaning = await _wordMeaningRepository.Get(new WordMeaningId(command.WordMeaningId), result);

        wordMeaning.Translations = command.Translations.ToListOrDefault() ?? wordMeaning.Translations;
        wordMeaning.PitchAccent = command.PitchAccent ?? wordMeaning.PitchAccent;
        wordMeaning.KanjiWritingPreferred = command.KanjiWritingPreference ?? wordMeaning.KanjiWritingPreferred;

        if (command.Readings is not null)
            wordMeaning.HiraganaWritings = 
                command.Readings.SelectOrEmpty(r => 
                    new WordMeaning.HiraganaWriting(
                        r.Value, 
                        r.Personas.SelectOrEmpty(p => 
                            new WordMeaning.Persona(
                                p.Properties.ToPersonaProperties()))
                        .ToList()))
                .ToList();


        foreach (var reading in wordMeaning?.HiraganaWritings)
        {
            if (reading.Properties is null)
                break;

            foreach (var persona in reading.Properties)
            {
                if (persona is null || persona.Properties is null)
                    return result.Fail();

                foreach (var item in persona.Properties)
                {
                    if (item is null)
                        return result.Fail();
                }
            }
        }

        await _wordMeaningRepository.Save(wordMeaning, result);

        if (command.PartOfSpeeches is null && command.WordProperties is null)
            return result;

        var word = await _wordRepository.Get(wordMeaning.WordId, result);

        word.PartsOfSpeech = command.PartOfSpeeches.ToListOrDefault().ToPartOfSpeeches() ?? word.PartsOfSpeech;
        word.Properties = command.WordProperties.ToListOrDefault().ToWordProperties() ?? word.Properties;

        if (command.PartOfSpeeches is not null && word.PartsOfSpeech.Count != command.PartOfSpeeches.Length)
            return result.Fail();

        if (command.WordProperties is not null && word.Properties.Count != command.WordProperties.Length)
            return result.Fail();

        await _wordRepository.Save(word, result);

        return result;
    }
}

public record UpdateWordMeaningCommand(
    string WordMeaningId,
    string[]? Translations = null,
    string[]? PartOfSpeeches = null,
    string[]? WordProperties = null,
    string? PitchAccent = null,
    bool? KanjiWritingPreference = null,
    ReadingArg[]? Readings = null) : ICommand<Result>;

public record ReadingArg(
    string Value,
    PersonaArg[] Personas);

public record PersonaArg(PersonaItemArg[] Properties);
public record PersonaItemArg(string Key, string Value);

public class UpdateWordMeaningCommandValidator : AbstractValidator<UpdateWordMeaningCommand> {}

public static class PersonaPropertyConverter
{
    public static List<PersonaProperty?>? ToPersonaProperties(this PersonaItemArg[]? persona) =>
        persona.SelectOrDefault(p => p.ToPersonaProperty()).WhereOrDefault(p => p is not null).ToListOrDefault();

    public static PersonaProperty? ToPersonaProperty(this PersonaItemArg persona)
    {
        var value = persona.Value.ToLower();

        if (value == Age.Old.Value ||
            value == Age.Young.Value)
            return new Age(value);

        if (value == Gender.Male.Value ||
            value == Gender.Female.Value)
            return new Gender(value);

        if (value == Dialect.Kansai.Value ||
            value == Dialect.Kanto.Value)
            return new Dialect(value);

        if (value == Formality.Rude.Value ||
            value == Formality.Formal.Value ||
            value == Formality.Informal.Value ||
            value == Formality.HumbleHonorific.Value)
            return new Formality(value);

        return null;
    }
}

public static class PartOfSpeechConverter
{
    public static List<PartOfSpeech?>? ToPartOfSpeeches(this IEnumerable<string>? partOfSpeech) =>
        partOfSpeech.SelectOrDefault(p => p.ToPartOfSpeech()).WhereOrDefault(p => p is not null).ToListOrDefault();

    public static PartOfSpeech? ToPartOfSpeech(this string value)
    {
        if (value == PartOfSpeech.Particle.Value)
            return PartOfSpeech.Particle;
        if (value == PartOfSpeech.Pronoun.Value)
            return PartOfSpeech.Pronoun;
        if (value == PartOfSpeech.Noun.Value)
            return PartOfSpeech.Noun;
        if (value == PartOfSpeech.Verb.Value)
            return PartOfSpeech.Verb;
        if (value == PartOfSpeech.Adjective.Value)
            return PartOfSpeech.Adjective;
        if (value == PartOfSpeech.Adverb.Value)
            return PartOfSpeech.Adverb;
        if (value == PartOfSpeech.Conjunction.Value)
            return PartOfSpeech.Conjunction;
        if (value == PartOfSpeech.AuxilaryVerb.Value)
            return PartOfSpeech.AuxilaryVerb;
        if (value == PartOfSpeech.Numeral.Value)
            return PartOfSpeech.Numeral;
        if (value == PartOfSpeech.Classifier.Value)
            return PartOfSpeech.Classifier;
        if (value == PartOfSpeech.Counter.Value)
            return PartOfSpeech.Counter;
        if (value == PartOfSpeech.Preposition.Value)
            return PartOfSpeech.Preposition;
        if (value == PartOfSpeech.Interjection.Value)
            return PartOfSpeech.Interjection;
        if (value == PartOfSpeech.Name.Value)
            return PartOfSpeech.Name;

        return null;
    }
}

public static class WordPropertyConverter
{
    public static List<WordProperty?>? ToWordProperties(this IEnumerable<string>? wordProperties) =>
        wordProperties.SelectOrDefault(p => p.ToWordProperty()).WhereOrDefault(p => p is not null).ToListOrDefault();

    public static WordProperty? ToWordProperty(this string value)
    {
        if (value == VerbTransitivity.Transitive.Value)
            return VerbTransitivity.Transitive;
        if (value == VerbTransitivity.Intransitive.Value)
            return VerbTransitivity.Intransitive;

        if (value == VerbConjugationType.Godan.Value)
            return VerbConjugationType.Godan;
        if (value == VerbConjugationType.Ichidan.Value)
            return VerbConjugationType.Ichidan;
        if (value == VerbConjugationType.Irregular.Value)
            return VerbConjugationType.Irregular;
        if (value == VerbConjugationType.Suru.Value)
            return VerbConjugationType.Suru;

        if (value == AdjectiveConjugationType.I.Value)
            return AdjectiveConjugationType.I;
        if (value == AdjectiveConjugationType.Na.Value)
            return AdjectiveConjugationType.Na;
        if (value == AdjectiveConjugationType.No.Value)
            return AdjectiveConjugationType.No;

        if (value == NounType.Verbal.Value)
            return NounType.Verbal;

        return null;
    }
}
