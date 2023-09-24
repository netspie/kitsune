using Corelibs.Basic.DDD;
using Manabu.Entities.Content.Users;
using Manabu.Entities.Shared;

namespace Manabu.Entities.Content.Events;

public class LearningObjectAddedForRehearseEvent : BaseDomainEvent
{
    public required UserId Owner { get; init; }
    public required LearningObjectId ObjectId { get; init; }
    public required LearningObjectType ObjectType { get; init; }
}
