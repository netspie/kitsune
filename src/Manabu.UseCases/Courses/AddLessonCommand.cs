using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Courses;
using Manabu.Entities.Lessons;
using Mediator;

namespace Manabu.UseCases.Lessons;

public class AddLessonCommandHandler : ICommandHandler<AddLessonCommand, Result>
{
    private readonly IRepository<Course, CourseId> _courseRepository;
    private readonly IRepository<Lesson, LessonId> _lessonRepository;

    public AddLessonCommandHandler(
        IRepository<Course, CourseId> courseRepository, 
        IRepository<Lesson, LessonId> lessonRepository)
    {
        _courseRepository = courseRepository;
        _lessonRepository = lessonRepository;
    }

    public async ValueTask<Result> Handle(AddLessonCommand command, CancellationToken ct)
    {
        var result = Result.Success();

        var course = await _courseRepository.Get(new CourseId(command.CourseId), result);

        new Lesson(command.LessonName, command.l)
        await _courseRepository.Save(course, result);
        result += await Task.WhenAll(newLessons.Select(_lessonRepository.Save));

        return result;
    }
}

public record AddLessonCommand(
    string LessonId,
    string LessonName,
    string CourseId,
    int ModuleIndex,
    int LessonIndex = 0) : ICommand<Result>;

public class AddLessonCommandValidator : AbstractValidator<AddLessonCommand> {}
