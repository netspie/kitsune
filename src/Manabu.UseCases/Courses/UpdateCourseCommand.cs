using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Courses;
using Manabu.Entities.Lessons;
using Mediator;

namespace Manabu.UseCases.Courses;

public class UpdateCourseCommandHandler : ICommandHandler<UpdateCourseCommand, Result>
{
    private readonly IRepository<Course, CourseId> _courseRepository;

    public UpdateCourseCommandHandler(IRepository<Course, CourseId> courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async ValueTask<Result> Handle(UpdateCourseCommand command, CancellationToken ct)
    {
        var result = Result.Success();

        var course = await _courseRepository.Get(new CourseId(command.Id), result);

        course.Name = command.Name;
        course.Description = command.Description;
        course.Modules = command.Modules.SelectOrDefault(m => 
            new Course.Module(m.Name, m.Lessons.SelectOrDefault(l =>
                new Course.Lesson(new LessonId(l.Id), l.Name)).ToList()))
            .ToList();

        await _courseRepository.Save(course, result);

        return result;
    }
}

public record UpdateCourseCommand(
    string Id,
    string Name,
    string Description,
    ModuleDTO[] Modules) : ICommand<Result>;

public class UpdateCourseCommandValidator : AbstractValidator<UpdateCourseCommand> {}
