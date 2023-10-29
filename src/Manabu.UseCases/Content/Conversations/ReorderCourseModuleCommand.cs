using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Content.Courses;
using Mediator;

namespace Manabu.UseCases.Content.Conversations
{
    public class ReorderCourseModuleCommandHandler : ICommandHandler<ReorderCourseModuleCommand, Result>
    {
        private readonly IRepository<Course, CourseId> _courseRepository;

        public ReorderCourseModuleCommandHandler(IRepository<Course, CourseId> courseRepository)
        {
            _courseRepository = courseRepository;
        }
        public async ValueTask<Result> Handle(ReorderCourseModuleCommand command, CancellationToken cancellationToken)
        {
            var result = Result.Success();

            var course = await _courseRepository.Get(new CourseId(command.CourseId), result);
            if (!result.ValidateSuccessAndValues())
                return result.Fail();

            if (!course.ReorderModules(command.ModuleName, command.Index))
                return result.Fail();

            await _courseRepository.Save(course, result);
            return result;
        }
    }

    public record ReorderCourseModuleCommand(
    string ModuleName,
    string CourseId,
    int Index) : ICommand<Result>;

    public class ReorderCourseModuleCommandValidator : AbstractValidator<ReorderCourseModuleCommand> { }

}
