using Corelibs.Basic.DDD;
using Manabu.Entities.Rehearse.RehearseSettings;

namespace Manabu.Entities.Content.Users;

public class User : Entity<UserId>, IAggregateRoot<UserId>
{
    public const string DefaultCollectionName = "users";

    public RehearseSetting RehearseSetting { get; set; }

    public User(
        UserId userId) : base(userId)
    {
    }
}

public class UserId : EntityId { public UserId(string value) : base(value) {} }
