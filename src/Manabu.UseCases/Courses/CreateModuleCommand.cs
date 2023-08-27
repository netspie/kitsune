using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Courses;
using Mediator;

namespace Manabu.UseCases.Courses;

public class CreateModuleCommandHandler : ICommandHandler<CreateModuleCommand, Result>
{
    private readonly IRepository<Course, CourseId> _courseRepository;

    public CreateModuleCommandHandler(
        IRepository<Course, CourseId> courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async ValueTask<Result> Handle(CreateModuleCommand command, CancellationToken ct)
    {
        var result = Result.Success();

        var course = await _courseRepository.Get(new CourseId(command.CourseId), result);
        if (!result.ValidateSuccessAndValues())
            return result.Fail();

        course.AddModule(command.ModuleName, command.ModuleIndex);

        await _courseRepository.Save(course, result);

        return result;
    }
}

public record CreateModuleCommand(
    string CourseId,
    string ModuleName,
    int ModuleIndex) : ICommand<Result>;

public class CreateModuleCommandValidator : AbstractValidator<CreateModuleCommand> {}
