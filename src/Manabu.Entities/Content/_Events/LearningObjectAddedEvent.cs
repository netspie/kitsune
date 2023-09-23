using Manabu.Entities.Content.Users;
using Manabu.Entities.Shared;
using Mediator;

namespace Manabu.Entities.Content.Events;

public class LearningObjectAddedEvent : INotification
{
    public required UserId Owner { get; init; }
    public required LearningObjectId ObjectId { get; init; }
    public required LearningObjectType ObjectType { get; init; }
}
