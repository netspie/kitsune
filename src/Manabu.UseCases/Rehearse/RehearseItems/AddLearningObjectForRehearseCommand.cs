using Corelibs.Basic.Auth;
using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Content.Events;
using Manabu.Entities.Content.Users;
using Manabu.Entities.Shared;
using Mediator;
using System.Security.Claims;

namespace Manabu.UseCases.Rehearse.RehearseItems;

public class AddLearningItemForRehearseCommandHandler : ICommandHandler<AddLearningObjectForRehearseCommand, Result>
{
    private readonly IAccessorAsync<ClaimsPrincipal> _userAccessor;
    private readonly IPublisher _publisher;

    public AddLearningItemForRehearseCommandHandler(
        IAccessorAsync<ClaimsPrincipal> userAccessor,
        IPublisher publisher)
    {
        _userAccessor = userAccessor;
        _publisher = publisher;
    }

    public async ValueTask<Result> Handle(AddLearningObjectForRehearseCommand command, CancellationToken ct)
    {
        await _publisher.Publish(new LearningObjectAddedForRehearseEvent()
        { 
            ObjectId = new LearningObjectId(command.LearningObjectId),
            ObjectType = new LearningObjectType(command.LearningObjectType),
            Owner = await _userAccessor.GetUserID<UserId>()
        });

        return Result.Success();
    }
}

public record AddLearningObjectForRehearseCommand(
    string LearningObjectId,
    string LearningObjectType) : ICommand<Result>;

public class AddLearningItemForRehearseCommandValidator : AbstractValidator<AddLearningObjectForRehearseCommand> {}
