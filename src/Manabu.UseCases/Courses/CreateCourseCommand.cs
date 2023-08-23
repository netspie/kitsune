using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Courses;
using Mediator;

namespace Manabu.UseCases.Courses;

public class CreateCourseCommandHandler : ICommandHandler<CreateCourseCommand, Result>
{
    private readonly IRepository<Course, CourseId> _courseRepository;

    public CreateCourseCommandHandler(IRepository<Course, CourseId> courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async ValueTask<Result> Handle(CreateCourseCommand command, CancellationToken ct)
    {
        var result = Result.Success();

        var course = new Course(command.Name);
        await _courseRepository.Save(course, result);

        return result;
    }
}

public record CreateCourseCommand(
    string Name) : ICommand<Result>;

public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand> {}
