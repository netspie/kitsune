using Corelibs.Basic.DDD;

namespace Manabu.Entities.Courses;

public class Audio : Entity<AudioId>, IAggregateRoot<AudioId>
{
    public string Name { get; private set; }

    public Audio(string name)
    {
        Name = name;
    }

    public Audio(
        AudioId id,
        uint version,
        string name) : base(id, version)
    {
        Name = name;
    }
}

public class AudioId : EntityId { public AudioId(string value) : base(value) { } }
