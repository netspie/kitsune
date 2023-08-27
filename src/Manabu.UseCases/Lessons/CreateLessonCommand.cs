using Corelibs.Basic.Auth;
using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Courses;
using Manabu.Entities.Lessons;
using Manabu.Entities.Users;
using Mediator;
using System.Security.Claims;

namespace Manabu.UseCases.Lessons;

public class CreateLessonCommandHandler : ICommandHandler<CreateLessonCommand, Result>
{
    private readonly IAccessorAsync<ClaimsPrincipal> _userAccessor;
    private readonly IRepository<Course, CourseId> _courseRepository;
    private readonly IRepository<Lesson, LessonId> _lessonRepository;

    public CreateLessonCommandHandler(
        IAccessorAsync<ClaimsPrincipal> userAccessor,
        IRepository<Course, CourseId> courseRepository,
        IRepository<Lesson, LessonId> lessonRepository)
    {
        _userAccessor = userAccessor;
        _courseRepository = courseRepository;
        _lessonRepository = lessonRepository;
    }

    public async ValueTask<Result> Handle(CreateLessonCommand command, CancellationToken ct)
    {
        var result = Result.Success();

        var userId = await _userAccessor.GetUserID<UserId>();

        var course = await _courseRepository.Get(new CourseId(command.CourseId), result);
        if (!result.ValidateSuccessAndValues())
            return result.Fail();

        var lesson = new Lesson(command.LessonName, course.Id, userId);
        if (!course.AddLesson(lesson.Id, command.ModuleIndex, command.LessonIndex))
            return result.Fail();

        await _courseRepository.Save(course, result);
        await _lessonRepository.Save(lesson, result);

        return result;
    }
}

public record CreateLessonCommand(
    string LessonName,
    string CourseId,
    int ModuleIndex,
    int LessonIndex = 0) : ICommand<Result>;

public class CreateLessonCommandValidator : AbstractValidator<CreateLessonCommand> {}
