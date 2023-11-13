using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Content.Courses;
using Mediator;

namespace Manabu.UseCases.Content.Courses;

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
        var courses = (await _courseRepository.GetAll()).Get();
        var course = courses.First(x => x.Id == new CourseId(command.CourseId));
        var coursesToReorder = courses.Where(x => x.Order > course.Order);

        if (!result.ValidateSuccessAndValues())
            return result.Fail();

        if (course.HasContent())
        {
            course.IsArchived = true;
            await _courseRepository.Save(course, result);
            return result;
        }
        
        foreach (var item in coursesToReorder)
        {
            item.Order--;
            await _courseRepository.Save(item, result);
        }

        result += await _courseRepository.Delete(course.Id);
        return result;
    }
}

public record RemoveCourseCommand(
    string CourseId) : ICommand<Result>;

public class RemoveCourseCommandValidator : AbstractValidator<RemoveCourseCommand> {}
