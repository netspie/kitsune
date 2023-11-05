using Corelibs.Basic.Blocks;
using Corelibs.Basic.Identity;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Content.WordMeanings;
using Manabu.Entities.Content.Words;
using Mediator;
using System.Security.Claims;

namespace Manabu.UseCases.Content.Words;

public class CreateWordMeaningCommandHandler : ICommandHandler<CreateWordMeaningCommand, Result>
{
    private readonly IAccessorAsync<ClaimsPrincipal> _userAccessor;
    private readonly IRepository<Word, WordId> _wordRepository;
    private readonly IRepository<WordMeaning, WordMeaningId> _wordMeaningRepository;

    public CreateWordMeaningCommandHandler(
        IAccessorAsync<ClaimsPrincipal> userAccessor,
        IRepository<Word, WordId> wordRepository,
        IRepository<WordMeaning, WordMeaningId> wordMeaningRepository)
    {
        _userAccessor = userAccessor;
        _wordRepository = wordRepository;
        _wordMeaningRepository = wordMeaningRepository;
    }

    public async ValueTask<Result> Handle(CreateWordMeaningCommand command, CancellationToken ct)
    {
        var result = Result.Success();
        
        var word = await _wordRepository.Get(new WordId(command.WordId), result);
        if (!result.ValidateSuccessAndValues())
            return result.Fail();

        var wordMeaningId = new WordMeaningId(IdCreator.CreateBase64GuidId());
        var wordMeaning = new WordMeaning(wordMeaningId, word.Id, word.Value, new() { command.WordMeaning });
        
        word.AddMeaning(wordMeaningId, command.Index);

        result += await _wordRepository.Save(word);
        result += await _wordMeaningRepository.Save(wordMeaning);

        return result;
    }
}

public record CreateWordMeaningCommand(
    string WordMeaning,
    string WordId,
    int Index = int.MaxValue) : ICommand<Result>;

public class CreateWordMeaningCommandValidator : AbstractValidator<CreateWordMeaningCommand> {}
