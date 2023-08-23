using Corelibs.Basic.DDD;
using Manabu.Entities.Authors;
using Manabu.Entities.Lessons;

namespace Manabu.Entities.Courses;

public class Course : Entity<CourseId>, IAggregateRoot<CourseId>
{
    public const string DefaultCollectionName = "courses";

    public string Name { get; private set; }
    public string Description { get; private set; }
    public AuthorId? Author { get; private set; }
    public List<Module> Modules { get; private set; }

    public Course(
        string name)
    {
        Name = name;
    }

    public Course(
        CourseId id, 
        uint version,
        string name,
        string description,
        AuthorId author,
        List<Module> modules) : base(id, version)
    {
        Name = name;
        Description = description;
        Author = author;
        Modules = modules;
    }

    public record Module(string Name, List<LessonId> Lessons);
}

public class CourseId : EntityId { public CourseId(string value) : base(value) { } }
