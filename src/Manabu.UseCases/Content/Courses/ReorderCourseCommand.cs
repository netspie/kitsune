using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
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

        if (!result.ValidateSuccessAndValues())
        {
            return result.Fail();
        }

        var courses = (await _courseRepository.GetAll()).Get();
        var seekedCourseId = new CourseId(command.CourseId);
        var seekedCourse = courses.FirstOrDefault(x => x.Id == seekedCourseId);
        var currentCourse = courses.FirstOrDefault(x => x.Order == command.Index);

        if (seekedCourse == null || currentCourse == null)
            result.Fail("Invalid course or index provided.");

        currentCourse!.ReorderCourse(seekedCourse!, command.Index);

        //save multiple we could add the method.
        await _courseRepository!.Save(seekedCourse!, result);
        await _courseRepository!.Save(currentCourse, result);

        return result;

    }
}

public record ReorderCourseCommand(
    string CourseId,
    int Index) : ICommand<Result>;

public class ReorderCourseModuleCommandValidator : AbstractValidator<ReorderCourseCommand> { }
