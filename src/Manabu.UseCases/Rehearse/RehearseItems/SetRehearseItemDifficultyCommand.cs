using Corelibs.Basic.Auth;
using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Content.Users;
using Manabu.Entities.Rehearse.RehearseItems;
using Mediator;
using System.Reflection;
using System.Security.Claims;

namespace Manabu.UseCases.Rehearse.RehearseItems;

public class SetRehearseItemDifficultyCommandHandler : ICommandHandler<SetRehearseItemDifficultyCommand, Result>
{
    private readonly IRepository<RehearseItem, RehearseItemId> _rehearseItemRepository;
    private readonly IRepository<RehearseItemAsap, RehearseItemId> _rehearseItemAsapRepository;
    private readonly IAccessorAsync<ClaimsPrincipal> _userAccessor;

    public SetRehearseItemDifficultyCommandHandler(
        IAccessorAsync<ClaimsPrincipal> userAccessor,
        IRepository<RehearseItem, RehearseItemId> rehearseItemRepository,
        IRepository<RehearseItemAsap, RehearseItemId> rehearseItemAsapRepository)
    {
        _userAccessor = userAccessor;
        _rehearseItemRepository = rehearseItemRepository;
        _rehearseItemAsapRepository = rehearseItemAsapRepository;
    }

    public async ValueTask<Result> Handle(SetRehearseItemDifficultyCommand command, CancellationToken ct)
    {
        var result = Result.Success();

        var difficulty = new Difficulty(command.Difficulty);
        var rehearseItem = await _rehearseItemRepository.Get(new RehearseItemId(command.RehearseItemId), result);
        if (rehearseItem is null || !rehearseItem.Rehearse(difficulty, out bool shouldReviewAsap))
            return Result.Failure();

        var itemAsap = await _rehearseItemAsapRepository.Get(rehearseItem.Id, result);
        if (itemAsap is null && shouldReviewAsap)
        {
            if (itemAsap is null)
                result += _rehearseItemAsapRepository.Save(RehearseItemAsap.Create(rehearseItem));
        }
        else
        if (itemAsap is not null && !shouldReviewAsap)
        {
            result += await _rehearseItemAsapRepository.Delete(rehearseItem.Id);
        }

        return result;
    }
}

public record SetRehearseItemDifficultyCommand(
    string RehearseItemId,
    int Difficulty) : ICommand<Result>;

public class SetRehearseItemDifficultyCommandValidator : AbstractValidator<SetRehearseItemDifficultyCommand> {}
