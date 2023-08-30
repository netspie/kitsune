using Corelibs.Basic.DDD;

namespace Manabu.Entities.Audios;

public class Audio : Entity<AudioId>, IAggregateRoot<AudioId>
{
    public const string DefaultCollectionName = "audios";

    public string Href { get; private set; }

    public Audio(string href)
    {
        Href = href;
    }
}

public class AudioId : EntityId { public AudioId(string value) : base(value) {} }
