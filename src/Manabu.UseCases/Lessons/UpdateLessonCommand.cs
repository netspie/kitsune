using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Lessons;
using Mediator;

namespace Manabu.UseCases.Lessons;

public class UpdateLessonCommandHandler : ICommandHandler<UpdateLessonCommand, Result>
{
    private readonly IRepository<Lesson, LessonId> _lessonRepository;

    public UpdateLessonCommandHandler(
        IRepository<Lesson, LessonId> lessonRepository)
    {
        _lessonRepository = lessonRepository;
    }

    public async ValueTask<Result> Handle(UpdateLessonCommand command, CancellationToken ct)
    {
        var result = Result.Success();

        var lesson = await _lessonRepository.Get(new LessonId(command.LessonId), result);

        lesson.Name = command.Name;
        lesson.Description = command.Description;

        await _lessonRepository.Save(lesson, result);

        return result;
    }
}

public record UpdateLessonCommand(
    string LessonId,
    string Name,
    string Description) : ICommand<Result>;

public class UpdateLessonCommandValidator : AbstractValidator<UpdateLessonCommand> {}
