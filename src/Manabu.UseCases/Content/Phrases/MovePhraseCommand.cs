using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Content.Conversations;
using Manabu.Entities.Content.Phrases;
using Mediator;

namespace Manabu.UseCases.Content.Phrases;

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

        var phrasesIds = command.PhraseIds.Select(id => new PhraseId(id)).ToList();
        var phrases = (await _phraseRepository.Get(phrasesIds, result)).OrderBy(p => phrasesIds.IndexOf(p.Id)).ToArray();
        var conversationCurrent = await _conversationRepository.Get(new ConversationId(command.CurrentConversationId), result);
        var conversationNew = await _conversationRepository.Get(new ConversationId(command.NewConversationId), result);
        if (!result.ValidateSuccessAndValues())
            return result.Fail();

        if (!conversationCurrent.MovePhrases(phrasesIds, conversationNew, command.Index))
            return result.Fail();

        foreach (var phrase in phrases)
        {
            if (!phrase.Move(conversationCurrent.Id, conversationNew.Id))
                return result.Fail();
        }

        result += await _conversationRepository.Save(conversationCurrent, conversationNew);
        result += await _phraseRepository.Save(phrases);

        return result;
    }
}

public record MovePhraseCommand(
    string[] PhraseIds,
    string CurrentConversationId,
    string NewConversationId,
    int Index = int.MaxValue) : ICommand<Result>;

public class MovePhraseCommandValidator : AbstractValidator<MovePhraseCommand> {}
