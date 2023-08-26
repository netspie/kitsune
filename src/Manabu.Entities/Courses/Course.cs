using Corelibs.Basic.Collections;
using Corelibs.Basic.DDD;
using Corelibs.Basic.Functional;
using Corelibs.Basic.Maths;
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

    public bool AddLesson(LessonId lesson, int moduleIndex, int lessonIndex)
    {
        Modules ??= new();
        if (moduleIndex < 0 || moduleIndex >= Modules.Count)
            return false;
        
        moduleIndex = moduleIndex.Clamp(Modules.Count);

        var lessons = Modules[moduleIndex];
        lessonIndex = lessonIndex.Clamp(lessons.IsNullOrEmpty() ? 0 : lessons.Count);

        return true;
    }

    public void AddModule(string name, int moduleIndex = 0)
    {
        Modules ??= new();
        Modules.InsertClamped(new Module(name, new List<string>()), moduleIndex);
    }

    public bool RemoveModule(int moduleIndex)
    {
        Modules ??= new();
        Modules.InsertClamped(new Module(name, new List<string>()), moduleIndex);
    }

    public record Module(string Name, List<string> LessonIds);
}

public class CourseId : EntityId { public CourseId(string value) : base(value) {} }
