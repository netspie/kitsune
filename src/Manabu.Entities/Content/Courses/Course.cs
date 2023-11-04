using Corelibs.Basic.Collections;
using Corelibs.Basic.DDD;
using Manabu.Entities.Content.Authors;
using Manabu.Entities.Content.Lessons;
using Manabu.Entities.Content.Users;

namespace Manabu.Entities.Content.Courses;

public class Course : Entity<CourseId>, IAggregateRoot<CourseId>
{
    public static string DefaultCollectionName { get; } = "courses";

    public UserId Owner { get; private set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public AuthorId? Author { get; set; }
    public List<Module> Modules { get; set; }
    public List<LessonId> LessonsRemoved { get; set; }
    public List<Module> ModulesRemoved { get; set; }
    public bool IsOfficial { get; set; }
    public bool IsArchived { get; set; }
    public int Order { get; set; }

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

    public bool ReorderModules(string name, int index)
    {
        var moduleData = Modules.FirstOrDefault(p => p.Name == name);
        if (moduleData is null)
            return false;

        if (!Modules.Remove(moduleData))
            return false;

        Modules.InsertClamped(moduleData, index);

        return true;
    }
    
    public bool HasContent() =>
        !Modules.IsNullOrEmpty() ||
        !LessonsRemoved.IsNullOrEmpty() ||
        !ModulesRemoved.IsNullOrEmpty();

    public record Module(string Name, List<LessonId> LessonIds);
}

public class CourseId : EntityId { public CourseId(string value) : base(value) {} }
