using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Content.Conversations;
using Manabu.Entities.Content.Phrases;
using Mediator;

namespace Manabu.UseCases.Content.Conversations;

public class ReorderConversationPhraseCommandHandler : ICommandHandler<ReorderConversationPhraseCommand, Result>
{
    private readonly IRepository<Conversation, ConversationId> _conversationRepository;

    public ReorderConversationPhraseCommandHandler(
        IRepository<Conversation, ConversationId> conversationRepository)
    {
        _conversationRepository = conversationRepository;
    }

    public async ValueTask<Result> Handle(ReorderConversationPhraseCommand command, CancellationToken ct)
    {
        var result = Result.Success();

        var conversation = await _conversationRepository.Get(new ConversationId(command.ConversationId), result);
        if (!result.ValidateSuccessAndValues())
            return result.Fail();

        if (!conversation.ReorderPhrase(new PhraseId(command.PhraseId), command.Index))
            return result.Fail();

        await _conversationRepository.Save(conversation, result);

        return result;
    }
}

public record ReorderConversationPhraseCommand(
    string ConversationId,
    string PhraseId,
    int Index) : ICommand<Result>;

public class ReorderConversationPhraseCommandValidator : AbstractValidator<ReorderConversationPhraseCommand> {}
