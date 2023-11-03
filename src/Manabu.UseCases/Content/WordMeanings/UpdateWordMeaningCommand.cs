using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.Basic.Functional;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Content.WordMeanings;
using Manabu.Entities.Content.Words;
using Mediator;

namespace Manabu.UseCases.Content.Courses;

public class UpdateWordMeaningCommandHandler : ICommandHandler<UpdateWordMeaningCommand, Result>
{
    private readonly IRepository<WordMeaning, WordMeaningId> _wordMeaningRepository;

    public UpdateWordMeaningCommandHandler(
        IRepository<WordMeaning, WordMeaningId> wordMeaningRepository)
    {
        _wordMeaningRepository = wordMeaningRepository;
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

        return result;
    }
}

public record UpdateWordMeaningCommand(
    string WordMeaningId,
    string[]? Translations = null,
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
    public static List<PersonaProperty>? ToPersonaProperties(this PersonaItemArg[] persona) =>
        persona.Select(p => p.ToPersonaProperty()).ToList();

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
