using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Conversations;
using Manabu.Entities.Phrases;
using Mediator;
using Microsoft.VisualBasic;

namespace Manabu.UseCases.Conversations;

public class UpdateConversationPhraseCommandHandler : ICommandHandler<UpdateConversationPhraseCommand, Result>
{
    private readonly IRepository<Phrase, PhraseId> _phraseRepository;
    private readonly IRepository<Conversation, ConversationId> _conversationRepository;

    public UpdateConversationPhraseCommandHandler(
        IRepository<Conversation, ConversationId> conversationRepository, IRepository<Phrase, PhraseId> phraseRepository)
    {
        _conversationRepository = conversationRepository;
        _phraseRepository = phraseRepository;
    }

    public async ValueTask<Result> Handle(UpdateConversationPhraseCommand command, CancellationToken ct)
    {
        var result = Result.Success();

        var phrase = await _phraseRepository.Get(new PhraseId(command.ConversationId), result);
        if (!result.ValidateSuccessAndValues())
            return result.Fail();

        if (command.Speaker is not null)
        {
            var conversation = await _conversationRepository.Get(new ConversationId(command.ConversationId), result);
            if (conversation.ChangeSpeaker(command.Speaker, phrase.Id, command.PhraseIndex))
                await _conversationRepository.Save(conversation, result);
        }

        if (!command.Original.IsNullOrEmpty())
        {
            phrase.Original = command.Original;
            await _phraseRepository.Save(phrase, result);
        }

        return result;
    }
}

public record UpdateConversationPhraseCommand(
    string PhraseId,
    string Original,
    string ConversationId,
    int PhraseIndex = -1,
    string? Speaker = null) : ICommand<Result>;

public class UpdateConversationPhraseCommandValidator : AbstractValidator<UpdateConversationPhraseCommand> {}
