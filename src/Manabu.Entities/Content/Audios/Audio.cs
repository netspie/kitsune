using Corelibs.Basic.DDD;

namespace Manabu.Entities.Content.Audios;

public class Audio : Entity<AudioId>, IAggregateRoot<AudioId>
{
    public static string DefaultCollectionName { get; } = "audios";

    public string Href { get; private set; }

    public Audio(string href)
    {
        Href = href;
    }
}

public class AudioId : EntityId { public AudioId(string value) : base(value) {} }
