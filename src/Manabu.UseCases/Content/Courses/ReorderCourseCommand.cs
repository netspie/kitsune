using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Content.Courses;
using Mediator;

namespace Manabu.UseCases.Content.Courses;

public class ReorderCourseCommandHandler : ICommandHandler<ReorderCourseCommand, Result>
{
    private readonly IRepository<Course, CourseId> _courseRepository;

    public ReorderCourseCommandHandler(IRepository<Course, CourseId> courseRepository)
    {
        _courseRepository = courseRepository;
    }
    public async ValueTask<Result> Handle(ReorderCourseCommand command, CancellationToken cancellationToken)
    {
        var result = Result.Success();

        var course = await _courseRepository.Get(new CourseId(command.CourseId), result);
        if (!result.ValidateSuccessAndValues())
            return result.Fail();

        if (!course.ReorderCourses(command.CourseId, command.Index))
            return result.Fail();

        await _courseRepository.Save(course, result);
        return result;
    }
}

public record ReorderCourseCommand(
    string CourseId,
    int Index) : ICommand<Result>;
    
public class ReorderCourseModuleCommandValidator : AbstractValidator<ReorderCourseCommand> { }