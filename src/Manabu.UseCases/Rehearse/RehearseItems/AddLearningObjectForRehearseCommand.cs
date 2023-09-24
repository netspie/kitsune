using Corelibs.Basic.Auth;
using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using Corelibs.Basic.UseCases;
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
    private readonly IEventStore _eventStore;

    public AddLearningItemForRehearseCommandHandler(
        IAccessorAsync<ClaimsPrincipal> userAccessor,
        IEventStore eventStore)
    {
        _userAccessor = userAccessor;
        _eventStore = eventStore;
    }

    public async ValueTask<Result> Handle(AddLearningObjectForRehearseCommand command, CancellationToken ct)
    {
        await _eventStore.Save(new LearningObjectAddedForRehearseEvent()
        { 
            Id = Guid.NewGuid().ToString(),
            Timestamp = DateTime.UtcNow.Ticks,
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
