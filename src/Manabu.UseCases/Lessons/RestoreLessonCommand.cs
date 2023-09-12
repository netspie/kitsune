using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Courses;
using Manabu.Entities.Lessons;
using Mediator;

namespace Manabu.UseCases.Lessons;

public class RestoreLessonCommandHandler : ICommandHandler<RestoreLessonCommand, Result>
{
    private readonly IRepository<Course, CourseId> _courseRepository;
    private readonly IRepository<Lesson, LessonId> _lessonRepository;

    public RestoreLessonCommandHandler(
        IRepository<Course, CourseId> courseRepository, 
        IRepository<Lesson, LessonId> lessonRepository)
    {
        _courseRepository = courseRepository;
        _lessonRepository = lessonRepository;
    }

    public async ValueTask<Result> Handle(RestoreLessonCommand command, CancellationToken ct)
    {
        var result = Result.Success();

        var course = await _courseRepository.Get(new CourseId(command.CourseId), result);
        if (!result.ValidateSuccessAndValues())
            return result.Fail();

        var lesson = await _lessonRepository.Get(new LessonId(command.LessonId), result);
        if (!result.ValidateSuccessAndValues())
            return result.Fail();

        if (!course.RestoreLesson(lesson.Id))
            return result.Fail();

        lesson.AddToCourse(course.Id);
        
        await _courseRepository.Save(course, result);
        await _lessonRepository.Save(lesson, result);

        return result;
    }
}

public record RestoreLessonCommand(
    string LessonId,
    string CourseId) : ICommand<Result>;

public class RestoreLessonCommandValidator : AbstractValidator<RestoreLessonCommand> {}
