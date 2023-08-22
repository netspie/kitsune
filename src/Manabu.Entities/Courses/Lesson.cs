using Corelibs.Basic.DDD;

namespace Manabu.Entities.Courses;

public class Lesson : Entity<LessonId>, IAggregateRoot<LessonId>
{
    public string Name { get; private set; }

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


