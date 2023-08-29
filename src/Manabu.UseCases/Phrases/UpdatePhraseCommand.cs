using Corelibs.Basic.Auth;
using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Conversations;
using Manabu.Entities.Lessons;
using Manabu.Entities.Phrases;
using Manabu.Entities.Users;
using Mediator;
using System.Drawing;
using System.Security.Claims;

namespace Manabu.UseCases.Phrases;

public class UpdatePhraseCommandHandler : ICommandHandler<UpdatePhraseCommand, Result>
{
    private readonly IAccessorAsync<ClaimsPrincipal> _userAccessor;
    private readonly IRepository<Conversation, ConversationId> _conversationRepository;
    private readonly IRepository<Phrase, PhraseId> _phraseRepository;

    public UpdatePhraseCommandHandler(
        IAccessorAsync<ClaimsPrincipal> userAccessor,
        IRepository<Conversation, ConversationId> conversationRepository,
        IRepository<Phrase, PhraseId> phraseRepository)
    {
        _userAccessor = userAccessor;
        _conversationRepository = conversationRepository;
        _phraseRepository = phraseRepository;
    }

    public async ValueTask<Result> Handle(UpdatePhraseCommand command, CancellationToken ct)
    {
        var result = Result.Success();

        var phrase = await _phraseRepository.Get(new PhraseId(command.PhraseId), result);
        if (!result.ValidateSuccessAndValues())
            return result.Fail();

        phrase.Original = command.Original ?? phrase.Original;
        phrase.Translations = command.Translations.ToListOrDefault() ?? phrase.Translations;
        phrase.Contexts = command.Contexts.ToListOrDefault() ?? phrase.Contexts;

        await _phraseRepository.Save(phrase, result);

        return result;
    }
}

public record UpdatePhraseCommand(
    string PhraseId,
    string? Original = null,
    string[]? Translations = null,
    string[]? Contexts = null) : ICommand<Result>;

public class UpdatePhraseCommandValidator : AbstractValidator<UpdatePhraseCommand> {}
