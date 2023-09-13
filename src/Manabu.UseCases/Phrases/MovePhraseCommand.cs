using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Conversations;
using Manabu.Entities.Phrases;
using Mediator;

namespace Manabu.UseCases.Phrases;

public class MovePhraseCommandHandler : ICommandHandler<MovePhraseCommand, Result>
{
    private readonly IRepository<Conversation, ConversationId> _conversationRepository;
    private readonly IRepository<Phrase, PhraseId> _phraseRepository;

    public MovePhraseCommandHandler(
        IRepository<Conversation, ConversationId> conversationRepository,
        IRepository<Phrase, PhraseId> phraseRepository)
    {
        _conversationRepository = conversationRepository;
        _phraseRepository = phraseRepository;
    }

    public async ValueTask<Result> Handle(MovePhraseCommand command, CancellationToken ct)
    {
        var result = Result.Success();

        var phrase = await _phraseRepository.Get(new PhraseId(command.PhraseId), result);
        var conversationCurrent = await _conversationRepository.Get(new ConversationId(command.CurrentConversationId), result);
        var conversationNew = await _conversationRepository.Get(new ConversationId(command.NewConversationId), result);
        if (!result.ValidateSuccessAndValues())
            return result.Fail();

        if (!conversationCurrent.MovePhrase(phrase.Id, conversationNew, command.Index))
            return result.Fail();

        if (!phrase.Move(conversationCurrent.Id, conversationNew.Id))
            return result.Fail();

        result += await _conversationRepository.Save(conversationCurrent, conversationNew);
        result += await _phraseRepository.Save(phrase);

        return result;
    }
}

public record MovePhraseCommand(
    string PhraseId,
    string CurrentConversationId,
    string NewConversationId,
    int Index = int.MaxValue) : ICommand<Result>;

public class MovePhraseCommandValidator : AbstractValidator<MovePhraseCommand> {}
