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

        var courses = await _courseRepository.GetAllAsync();
        if (!result.ValidateSuccessAndValues())
            return result.Fail();
        var seekedCourse = courses.FirstOrDefault(x => x.Id == new CourseId(command.CourseId));

        //move logic 
        var courseList = courses.ToList();
        courseList.Remove(seekedCourse);
        courseList.Insert(command.Index,seekedCourse);
        foreach (var item in courseList)
        {
            await _courseRepository.Save(item, result);
        }
      
        return result;
    }
}

public record ReorderCourseCommand(
    string CourseId,
    int Index) : ICommand<Result>;
    
public class ReorderCourseModuleCommandValidator : AbstractValidator<ReorderCourseCommand> { }