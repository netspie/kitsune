using Corelibs.Basic.Auth;
using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Content.Courses;
using Manabu.Entities.Content.Users;
using Mediator;
using System.Security.Claims;

namespace Manabu.UseCases.Content.Courses;

public class CreateCourseCommandHandler : ICommandHandler<CreateCourseCommand, Result>
{
    private readonly IAccessorAsync<ClaimsPrincipal> _userAccessor;
    private readonly IRepository<Course, CourseId> _courseRepository;

    public CreateCourseCommandHandler(
        IAccessorAsync<ClaimsPrincipal> userAccessor,
        IRepository<Course, CourseId> courseRepository)
    {
        _userAccessor = userAccessor;
        _courseRepository = courseRepository;
    }

    public async ValueTask<Result> Handle(CreateCourseCommand command, CancellationToken ct)
    {
        var result = Result.Success();
        var userId = await _userAccessor.GetUserID<UserId>();
        var courses = (await _courseRepository.GetAll()).Get();
        var coursesToReindex = courses.Where(x=>x.Order>= command.Index).ToList();
        var course = new Course(command.Name, userId, command.Index);

        foreach (var item in coursesToReindex)
        {
            item.Order++;
            await _courseRepository.Save(item, result);
        }

        await _courseRepository.Save(course, result);

        return result;
    }
}

public record CreateCourseCommand(
    string Name, int Index) : ICommand<Result>;

public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand> {}
