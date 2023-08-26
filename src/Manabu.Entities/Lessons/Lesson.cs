using Corelibs.Basic.DDD;
using Manabu.Entities.Conversations;
using Manabu.Entities.Courses;
using Manabu.Entities.Infos;
using Manabu.Entities.Phrases;

namespace Manabu.Entities.Lessons;

public class Lesson : Entity<LessonId>, IAggregateRoot<LessonId>
{
    public const string DefaultCollectionName = "lessons";

    public string Name { get; set; }
    public string Description { get; set; }
    public List<CourseId> Courses { get; private set; }
    public List<InfoId> Infos { get; private set; }
    public List<ConversationId> Conversations { get; private set; }
    public List<PhraseId> Phrases { get; private set; }

    public Lesson(
        string name,
        CourseId courseId)
    {
        Name = name;
        Courses = new() { courseId };
    }

}

public class LessonId : EntityId { public LessonId(string value) : base(value) { } }
