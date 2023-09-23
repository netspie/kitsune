using Corelibs.Basic.DDD;
using Corelibs.Basic.Encryption;
using Manabu.Entities.Content.Users;
using Manabu.Entities.Shared;

namespace Manabu.Entities.Rehearse.RehearseContainers;

public class RehearseContainer : Entity<RehearseContainerId>, IAggregateRoot<RehearseContainerId>
{
    public const string DefaultCollectionName = "rehearseContainers";

    public UserId Owner { get; private set; }
    public LearningObjectId ReferenceId { get; private set; }
    public LearningObjectType ReferenceType { get; private set; }
    public DateTime CreatedUtcTime { get; init; }

    public RehearseContainer(
        RehearseContainerId id,
        UserId owner,
        LearningObjectId referenceId,
        LearningObjectType referenceType) : base(id)
    {
        Owner = owner;
        ReferenceId = referenceId;
        ReferenceType = referenceType;
        CreatedUtcTime = DateTime.UtcNow;
    }
}

public class RehearseContainerId : EntityId
{
    public RehearseContainerId(string userId, string itemId) : base(EncryptionFunctions.GenerateGuidHash(userId, itemId))
    {
    }
}
