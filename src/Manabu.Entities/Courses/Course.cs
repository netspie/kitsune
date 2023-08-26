using Corelibs.Basic.DDD;
using Manabu.Entities.Authors;
using Manabu.Entities.Lessons;
using Newtonsoft.Json;

namespace Manabu.Entities.Courses;

public class Course : Entity<CourseId>, IAggregateRoot<CourseId>
{
    public const string DefaultCollectionName = "courses";

    public string Name { get; set; }
    public string Description { get; set; }
    public AuthorId? Author { get; set; }
    public List<Module> Modules { get; set; }
    public List<Lesson> LessonsRemoved { get; set; }
    public List<Module> ModulesRemoved { get; set; }
    public bool IsOfficial { get; set; }

    public Course(
        string name)
    {
        Name = name;
    }

    [JsonConstructor]
    public Course(
        CourseId id, 
        uint version,
        string name,
        string description,
        AuthorId author,
        List<Module> modules,
        bool isOfficial = false,
        List<Lesson> lessonsRemoved = null,
        List<Module> modulesRemoved = null) : base(id, version)
    {
        Name = name;
        Description = description;
        Author = author;
        Modules = modules;
        IsOfficial = isOfficial;
        LessonsRemoved = lessonsRemoved;
        ModulesRemoved = modulesRemoved;
    }

    public record Module(string Name, List<Lesson> Lessons);
    public record Lesson(LessonId Id, string Name);
}

public class CourseId : EntityId { public CourseId(string value) : base(value) {} }
