using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Content.Phrases;
using Manabu.Entities.Content.WordMeanings;
using Mediator;

namespace Manabu.UseCases.Content.Phrases;

public class RemovePhraseWordCommandHandler : ICommandHandler<RemovePhraseWordCommand, Result>
{
    private readonly IRepository<Phrase, PhraseId> _phraseRepository;

    public RemovePhraseWordCommandHandler(
        IRepository<Phrase, PhraseId> phraseRepository)
    {
        _phraseRepository = phraseRepository;
    }

    public async ValueTask<Result> Handle(RemovePhraseWordCommand command, CancellationToken ct)
    {
        var result = Result.Success();

        var phrase = await _phraseRepository.Get(new PhraseId(command.PhraseId), result);
        if (!result.ValidateSuccessAndValues())
            return result.Fail();

        if (!phrase.RemoveWord(new WordLink(
            new WordMeaningId(command.WordMeaningId),
                command.WordInflectionId,
                command.Reading,
                command.WritingMode is null ? null : new WritingMode(command.WritingMode),
                command.CustomWriting)))
            return result.Fail();

        await _phraseRepository.Save(phrase, result);

        return result;
    }
}

public record RemovePhraseWordCommand(
    string PhraseId,
    string WordMeaningId,
    string? WordInflectionId = null,
    string? Reading = null,
    string? WritingMode = null,
    string? CustomWriting = null) : ICommand<Result>;

public class RemovePhraseWordCommandValidator : AbstractValidator<RemovePhraseWordCommand> {}
