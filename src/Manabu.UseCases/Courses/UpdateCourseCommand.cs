using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Courses;
using Manabu.Entities.Lessons;
using Mediator;

namespace Manabu.UseCases.Courses;

public class UpdateCourseCommandHandler : ICommandHandler<UpdateCourseCommand, Result>
{
    private readonly IRepository<Course, CourseId> _courseRepository;
    private readonly IRepository<Lesson, LessonId> _lessonRepository;

    public UpdateCourseCommandHandler(
        IRepository<Course, CourseId> courseRepository, 
        IRepository<Lesson, LessonId> lessonRepository)
    {
        _courseRepository = courseRepository;
        _lessonRepository = lessonRepository;
    }

    public async ValueTask<Result> Handle(UpdateCourseCommand command, CancellationToken ct)
    {
        var result = Result.Success();

        var course = await _courseRepository.Get(new CourseId(command.Id), result);

        course.Name = command.Name;
        course.Description = command.Description;

        await _courseRepository.Save(course, result);

        return result;
    }
}

public record UpdateCourseCommand(
    string Id,
    string Name,
    string Description) : ICommand<Result>;

public class UpdateCourseCommandValidator : AbstractValidator<UpdateCourseCommand> {}
