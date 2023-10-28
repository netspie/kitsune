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
        if (persona.Key == nameof(Age))
            return new Age(persona.Value);

        if (persona.Key == nameof(Gender))
            return new Gender(persona.Value);

        if (persona.Key == nameof(Dialect))
            return new Dialect(persona.Value);

        if (persona.Key == nameof(Formality))
            return new Formality(persona.Value);

        return null;
    }
}
