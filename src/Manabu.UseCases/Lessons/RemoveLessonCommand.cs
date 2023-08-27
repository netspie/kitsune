using Corelibs.Basic.Blocks;
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

        var course = await _courseRepository.Get(new CourseId(command.CourseId), result);
        var lesson = await _lessonRepository.Get(new LessonId(command.CourseId), result);
        
        if (!lesson.RemoveFromCourse(course.Id))
            return result.Fail();

        if (!course.RemoveLesson(lesson.Id))
            return result.Fail();

        await _lessonRepository.Save(lesson, result);
        await _courseRepository.Save(course, result);

        return result;
    }
}

public record RemoveLessonCommand(
    string LessonId,
    string CourseId) : ICommand<Result>;

public class RemoveLessonCommandValidator : AbstractValidator<RemoveLessonCommand> {}
