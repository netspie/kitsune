using Corelibs.Basic.Collections;
using Corelibs.Basic.DDD;
using Manabu.Entities.Conversations;
using Manabu.Entities.Courses;
using Manabu.Entities.Infos;
using Manabu.Entities.Phrases;
using Manabu.Entities.Shared;
using Manabu.Entities.Users;

namespace Manabu.Entities.Lessons;

public class Lesson : Entity<LessonId>, IAggregateRoot<LessonId>
{
    public const string DefaultCollectionName = "lessons";

    public UserId Owner { get; private set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<CourseId> Courses { get; private set; }
    public List<InfoId> Infos { get; private set; }
    public List<ConversationId> Conversations { get; private set; }
    public List<PhraseId> Phrases { get; private set; }

    public Lesson(
        string name,
        CourseId courseId,
        UserId owner)
    {
        Name = name;
        Courses = new() { courseId };
        Owner = owner;
    }

    public void AddToCourse(CourseId course)
    {
        Courses ??= new();
        Courses.Add(course);
    }

    public void AddConversation(ConversationId conversation, int index = int.MaxValue)
    {
        Conversations ??= new();
        Conversations.InsertClamped(conversation, index);
    }

    public bool RemoveFromCourse(CourseId course) =>
        Courses is null ? false :
            Courses.Count <= 1 ? true : Courses.Remove(course);
}

public class LessonId : LearningItemId { public LessonId(string value) : base(value) { } }
