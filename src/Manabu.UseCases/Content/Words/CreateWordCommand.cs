using Corelibs.Basic.Auth;
using Corelibs.Basic.Blocks;
using Corelibs.Basic.Identity;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Content.Users;
using Manabu.Entities.Content.Words;
using Mediator;
using System.Security.Claims;

namespace Manabu.UseCases.Content.Words;

public class CreateWordCommandHandler : ICommandHandler<CreateWordCommand, Result>
{
    private readonly IAccessorAsync<ClaimsPrincipal> _userAccessor;
    private readonly IRepository<Word, WordId> _wordRepository;

    public CreateWordCommandHandler(
        IAccessorAsync<ClaimsPrincipal> userAccessor,
        IRepository<Word, WordId> wordRepository)
    {
        _userAccessor = userAccessor;
        _wordRepository = wordRepository;
    }

    public async ValueTask<Result> Handle(CreateWordCommand command, CancellationToken ct)
    {
        var wordId = new WordId(IdCreator.CreateBase64GuidId());
        var word = new Word(wordId, command.Word);
        return await _wordRepository.Save(word);
    }
}

public record CreateWordCommand(
    string Word) : ICommand<Result>;

public class CreateWordCommandValidator : AbstractValidator<CreateWordCommand> {}
