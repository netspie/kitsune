using Corelibs.Basic.DDD;
using Manabu.Entities.Shared;

namespace Manabu.Entities.Content.Audios;

public class Audio : Entity<AudioId>, IAggregateRoot<AudioId>
{
    public const string DefaultCollectionName = "audios";

    public string Href { get; private set; }

    public Audio(string href)
    {
        Href = href;
    }
}

public class AudioId : LearningItemId { public AudioId(string value) : base(value) {} }
