using Corelibs.Basic.DDD;

namespace Manabu.Entities.Courses;

public class Course : Entity<CourseId>, IAggregateRoot<CourseId>
{
    public string Name { get; private set; }
    public List<LessonId> Lessons { get; private set; }

    public Course(string name)
    {
        Name = name;
    }

    public Course(
        CourseId id, 
        uint version,
        string name) : base(id, version)
    {
        Name = name;
    }
}

public class CourseId : EntityId { public CourseId(string value) : base(value) { } }
