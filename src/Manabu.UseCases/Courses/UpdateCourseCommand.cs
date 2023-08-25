using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.Basic.DDD;
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

        var newLessons = new List<Lesson>();

        course.Name = command.Name;
        course.Description = command.Description;
        course.Modules = command.Modules.SelectOrDefault(m => 
            new Course.Module(m.Name, m.Lessons.SelectOrDefault(l =>
                new Course.Lesson(
                    l.Id.IsNullOrEmpty() ? new Lesson(l.Name).AddTo(newLessons).Id : new LessonId(l.Id),
                    l.Name)).ToList()))
            .ToList();

        await _courseRepository.Save(course, result);
        result += await Task.WhenAll(newLessons.Select(_lessonRepository.Save));

        return result;
    }
}

public record UpdateCourseCommand(
    string Id,
    string Name,
    string Description,
    ModuleDTO[] Modules) : ICommand<Result>;

public class UpdateCourseCommandValidator : AbstractValidator<UpdateCourseCommand> {}
