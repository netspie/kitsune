using Corelibs.Basic.Auth;
using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Flashcards;
using Manabu.Entities.RehearseContainers;
using Manabu.Entities.RehearseItems;
using Manabu.Entities.Users;
using Manabu.UseCases.FlashcardLists;
using Mediator;
using System.Security.Claims;

namespace Manabu.UseCases.RehearseItems;

public class SetRehearseRehearseItemAnswerCommandHandler : ICommandHandler<SetRehearseRehearseItemAnswerCommand, Result>
{
    private readonly IRepository<RehearseItem, RehearseItemId> _rehearseItemRepository;
    private readonly IAccessorAsync<ClaimsPrincipal> _userAccessor;

    public SetRehearseRehearseItemAnswerCommandHandler(
        IAccessorAsync<ClaimsPrincipal> userAccessor,
        IRepository<RehearseItem, RehearseItemId> rehearseItemRepository)
    {
        _userAccessor = userAccessor;
        _rehearseItemRepository = rehearseItemRepository;
    }

    public async ValueTask<Result> Handle(SetRehearseRehearseItemAnswerCommand command, CancellationToken ct)
    {
        var result = Result.Success();

        var userId = await _userAccessor.GetUserID<UserId>();

        var rehearseItem = await _rehearseItemRepository.Get(new RehearseItemId(command.RehearseItemId), result);

        return result;
    }
}

public record SetRehearseRehearseItemAnswerCommand(
    string RehearseItemId) : ICommand<Result>;

public class SetRehearseRehearseItemAnswerCommandValidator : AbstractValidator<SetRehearseRehearseItemAnswerCommand> {}
