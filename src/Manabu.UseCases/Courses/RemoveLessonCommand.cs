using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Courses;
using Manabu.Entities.Lessons;
using Mediator;

namespace Manabu.UseCases.Lessons;

public class RemoveLessonCommandHandler : ICommandHandler<RemoveLessonCommand, Result>
{
    private readonly IRepository<Course, CourseId> _courseRepository;
    private readonly IRepository<Lesson, LessonId> _lessonRepository;

    public RemoveLessonCommandHandler(
        IRepository<Course, CourseId> courseRepository, 
        IRepository<Lesson, LessonId> lessonRepository)
    {
        _courseRepository = courseRepository;
        _lessonRepository = lessonRepository;
    }

    public async ValueTask<Result> Handle(RemoveLessonCommand command, CancellationToken ct)
    {
        var result = Result.Success();

        var course = await _courseRepository.Get(new CourseId(command.Id), result);

        var updatedLessons = command.Modules.SelectMany(m => m.Lessons).ToArray();
        var existingLessons = course.Modules.SelectMany(m => m.Lessons).ToArray();
        var lessonsToRemove = existingLessons.Where(l => updatedLessons.FirstOrDefault(ul => ul.Id == l.Id.Value) is null).ToList();

        course.LessonsRemoved ??= new();
        course.LessonsRemoved = course.LessonsRemoved.Concat(lessonsToRemove).ToList();

        var updatedModuleNames = command.Modules.Select(m => m.Name).ToArray();
        var modulesToRemove = course.Modules.Where(m => !updatedModuleNames.Contains(m.Name)).ToArray();

        course.ModulesRemoved ??= new();
        course.ModulesRemoved = course.ModulesRemoved.Concat(modulesToRemove).ToList();

        var newLessons = new List<Lesson>();
        
        course.Name = command.Name;
        course.Description = command.Description;
        course.Modules = command.Modules.SelectOrDefault(m => 
            new Course.Module(m.Name, m.Lessons.SelectOrDefault(l =>
                new Course.Lesson(
                    l.Id.IsNullOrEmpty() ? new Lesson(l.Name).AddTo(newLessons).Id : new LessonId(l.Id),
                    l.Name)).ToList()))
            .ToList();

        await _courseRepository.Save(course, result);
        result += await Task.WhenAll(newLessons.Select(_lessonRepository.Save));

        return result;
    }
}

public record RemoveLessonCommand(
    string LessonId,
    string CourseId) : ICommand<Result>;

public class RemoveLessonCommandValidator : AbstractValidator<RemoveLessonCommand> {}
