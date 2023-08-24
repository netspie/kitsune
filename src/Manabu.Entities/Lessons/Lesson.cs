using Corelibs.Basic.DDD;
using Manabu.Entities.Conversations;
using Manabu.Entities.Infos;
using Manabu.Entities.Phrases;

namespace Manabu.Entities.Lessons;

public class Lesson : Entity<LessonId>, IAggregateRoot<LessonId>
{
    public const string DefaultCollectionName = "lessons";

    public string Name { get; private set; }
    public string Description { get; private set; }
    public List<InfoId> Infos { get; private set; }
    public List<ConversationId> Conversations { get; private set; }
    public List<PhraseId> Phrases { get; private set; }

    public Lesson(string name)
    {
        Name = name;
    }

    public Lesson(
        LessonId id,
        uint version,
        string name) : base(id, version)
    {
        Name = name;
    }
}

public class LessonId : EntityId { public LessonId(string value) : base(value) { } }
