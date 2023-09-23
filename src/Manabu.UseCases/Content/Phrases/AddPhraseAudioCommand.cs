using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Content.Audios;
using Manabu.Entities.Content.Phrases;
using Mediator;

namespace Manabu.UseCases.Content.Phrases;

public class AddPhraseAudioCommandHandler : ICommandHandler<AddPhraseAudioCommand, Result>
{
    private readonly IRepository<Phrase, PhraseId> _phraseRepository;
    private readonly IRepository<Audio, AudioId> _audioRepository;

    public AddPhraseAudioCommandHandler(
        IRepository<Phrase, PhraseId> phraseRepository, 
        IRepository<Audio, AudioId> audioRepository)
    {
        _phraseRepository = phraseRepository;
        _audioRepository = audioRepository;
    }

    public async ValueTask<Result> Handle(AddPhraseAudioCommand command, CancellationToken ct)
    {
        var result = Result.Success();

        var phrase = await _phraseRepository.Get(new PhraseId(command.PhraseId), result);
        if (!result.ValidateSuccessAndValues())
            return result.Fail();

        var audio = new Audio(command.Href);
        phrase.AddAudio(audio.Id);

        await _audioRepository.Save(audio, result);
        await _phraseRepository.Save(phrase, result);

        return result;
    }
}

public record AddPhraseAudioCommand(
    string PhraseId,
    string Href) : ICommand<Result>;

public class AddPhraseAudioCommandValidator : AbstractValidator<AddPhraseAudioCommand> {}
