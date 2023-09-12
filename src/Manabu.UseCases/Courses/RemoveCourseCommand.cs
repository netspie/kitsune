using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Courses;
using Mediator;

namespace Manabu.UseCases.Courses;

public class RemoveCourseCommandHandler : ICommandHandler<RemoveCourseCommand, Result>
{
    private readonly IRepository<Course, CourseId> _courseRepository;

    public RemoveCourseCommandHandler(
        IRepository<Course, CourseId> courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async ValueTask<Result> Handle(RemoveCourseCommand command, CancellationToken ct)
    {
        var result = Result.Success();

        var course = await _courseRepository.Get(new CourseId(command.CourseId), result);
        if (!result.ValidateSuccessAndValues())
            return result.Fail();

        if (course.HasContent())
        {
            course.IsArchived = true;
            await _courseRepository.Save(course, result);
            return result;
        }

        result += await _courseRepository.Delete(course.Id);

        return result;
    }
}

public record RemoveCourseCommand(
    string CourseId) : ICommand<Result>;

public class RemoveCourseCommandValidator : AbstractValidator<RemoveCourseCommand> {}
