using Corelibs.Basic.DDD;

namespace Manabu.Entities.Users;

public class User : Entity<UserId>, IAggregateRoot<UserId>
{
    public const string DefaultCollectionName = "users";

    public User(
        UserId userId) : base(userId)
    {
    }
}

public class UserId : EntityId { public UserId(string value) : base(value) {} }
