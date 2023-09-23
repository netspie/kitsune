using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Content.Conversations;
using Mediator;

namespace Manabu.UseCases.Content.Conversations;

public class UpdateConversationCommandHandler : ICommandHandler<UpdateConversationCommand, Result>
{
    private readonly IRepository<Conversation, ConversationId> _conversationRepository;

    public UpdateConversationCommandHandler(
        IRepository<Conversation, ConversationId> conversationRepository)
    {
        _conversationRepository = conversationRepository;
    }

    public async ValueTask<Result> Handle(UpdateConversationCommand command, CancellationToken ct)
    {
        var result = Result.Success();

        var conversation = await _conversationRepository.Get(new ConversationId(command.ConversationId), result);
        if (!result.ValidateSuccessAndValues())
            return result.Fail();

        conversation.Name = command.Name;
        conversation.Description = command.Description;

        await _conversationRepository.Save(conversation, result);

        return result;
    }
}

public record UpdateConversationCommand(
    string ConversationId,
    string Name,
    string Description,
    string[]? Speakers = null) : ICommand<Result>;

public class UpdateConversationCommandValidator : AbstractValidator<UpdateConversationCommand> {}
