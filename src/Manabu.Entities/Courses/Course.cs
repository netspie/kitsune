using Corelibs.Basic.Collections;
using Corelibs.Basic.DDD;
using Manabu.Entities.Authors;
using Manabu.Entities.Lessons;
using Manabu.Entities.Users;

namespace Manabu.Entities.Courses;

public class Course : Entity<CourseId>, IAggregateRoot<CourseId>
{
    public const string DefaultCollectionName = "courses";

    public UserId Owner { get; private set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public AuthorId? Author { get; set; }
    public List<Module> Modules { get; set; }
    public List<LessonId> LessonsRemoved { get; set; }
    public List<Module> ModulesRemoved { get; set; }
    public bool IsOfficial { get; set; }

    public Course(
        string name,
        UserId owner)
    {
        Name = name;
        Owner = owner;
    }

    public bool AddLesson(LessonId lesson, int moduleIndex, int lessonIndex)
    {
        Modules ??= new();
        if (moduleIndex < 0 || moduleIndex >= Modules.Count)
            return false;
        
        var lessons = Modules[moduleIndex].LessonIds;
        lessons.InsertClamped(lesson, lessonIndex);

        return true;
    }

    public void AddModule(string name, int moduleIndex = 0)
    {
        Modules ??= new();
        Modules.InsertClamped(new Module(name, new()), moduleIndex);
    }

    public bool RemoveModule(int moduleIndex)
    {
        ModulesRemoved ??= new();
        if (Modules is null || 
            moduleIndex < 0 ||
            moduleIndex >= Modules.Count)
            return false;

        ModulesRemoved.Add(Modules[moduleIndex]);
        Modules.RemoveAt(moduleIndex);

        return true;
    }

    public bool RemoveLesson(LessonId lesson)
    {
        LessonsRemoved ??= new();
        if (Modules is null)
            return false;

        if (!Modules.Contains(m => m.LessonIds.Remove(lesson)))
            return false;

        LessonsRemoved.Add(lesson);

        return true;
    }

    public bool RestoreLesson(LessonId lesson)
    {
        if (Modules.IsNullOrEmpty())
            return false;

        LessonsRemoved ??= new();
        if (!LessonsRemoved.Remove(lesson))
            return false;

        Modules[0].LessonIds.Add(lesson);
        return true;
    }

    public record Module(string Name, List<LessonId> LessonIds);
}

public class CourseId : EntityId { public CourseId(string value) : base(value) {} }
