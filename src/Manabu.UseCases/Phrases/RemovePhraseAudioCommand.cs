using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.Basic.Repository;
using FluentValidation;
using Mediator;
using Manabu.Entities.Content.Audios;
using Manabu.Entities.Content.Phrases;

namespace Manabu.UseCases.Phrases;

public class RemovePhraseAudioCommandHandler : ICommandHandler<RemovePhraseAudioCommand, Result>
{
    private readonly IRepository<Phrase, PhraseId> _phraseRepository;
    private readonly IRepository<Audio, AudioId> _audioRepository;

    public RemovePhraseAudioCommandHandler(
        IRepository<Phrase, PhraseId> phraseRepository, 
        IRepository<Audio, AudioId> audioRepository)
    {
        _phraseRepository = phraseRepository;
        _audioRepository = audioRepository;
    }

    public async ValueTask<Result> Handle(RemovePhraseAudioCommand command, CancellationToken ct)
    {
        var result = Result.Success();

        var phrase = await _phraseRepository.Get(new PhraseId(command.PhraseId), result);
        var audio = await _audioRepository.Get(new AudioId(command.AudioId), result);
        if (!result.ValidateSuccessAndValues())
            return result.Fail();
        
        if (!phrase.Audios.Remove(audio.Id))
            return result.Fail();

        await _phraseRepository.Save(phrase, result);
        result += await _audioRepository.Delete(audio.Id);

        return result;
    }
}

public record RemovePhraseAudioCommand(
    string PhraseId,
    string AudioId) : ICommand<Result>;

public class RemovePhraseAudioCommandValidator : AbstractValidator<RemovePhraseAudioCommand> {}
