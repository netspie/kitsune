using Corelibs.Basic.Auth;
using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using FluentValidation;
using Mediator;
using System.Security.Claims;
using Manabu.Entities.Content.Conversations;
using Manabu.Entities.Content.Phrases;
using Manabu.Entities.Content.Users;

namespace Manabu.UseCases.Phrases;

public class CreatePhraseCommandHandler : ICommandHandler<CreatePhraseCommand, Result>
{
    private readonly IAccessorAsync<ClaimsPrincipal> _userAccessor;
    private readonly IRepository<Conversation, ConversationId> _conversationRepository;
    private readonly IRepository<Phrase, PhraseId> _phraseRepository;

    public CreatePhraseCommandHandler(
        IAccessorAsync<ClaimsPrincipal> userAccessor,
        IRepository<Conversation, ConversationId> conversationRepository,
        IRepository<Phrase, PhraseId> phraseRepository)
    {
        _userAccessor = userAccessor;
        _conversationRepository = conversationRepository;
        _phraseRepository = phraseRepository;
    }

    public async ValueTask<Result> Handle(CreatePhraseCommand command, CancellationToken ct)
    {
        var result = Result.Success();

        var userId = await _userAccessor.GetUserID<UserId>();
        var conversation = await _conversationRepository.Get(new ConversationId(command.ConversationId), result);
        
        var phrase = new Phrase(userId, command.Original, conversation?.Id);
        await _phraseRepository.Save(phrase, result);

        if (conversation is not null)
        {
            conversation.AddPhrase(phrase.Id, command.Index);
            await _conversationRepository.Save(conversation, result);
        }

        return result;
    }
}

public record CreatePhraseCommand(
    string Original,
    string? ConversationId = null,
    int Index = 0) : ICommand<Result>;

public class CreatePhraseCommandValidator : AbstractValidator<CreatePhraseCommand> {}
