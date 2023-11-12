using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Content.Conversations;
using Manabu.Entities.Content.Courses;
using Manabu.Entities.Content.Lessons;
using Manabu.UseCases.Content.Lessons;
using Mediator;

namespace Manabu.UseCases.Content.Conversations;

public class RemoveConversationCommandHandler : ICommandHandler<RemoveConversationCommand, Result>
{
    private readonly IRepository<Lesson, LessonId> _lessonRepository;
    private readonly IRepository<Conversation, ConversationId> _conversationRepository;

    public RemoveConversationCommandHandler(
        IRepository<Lesson, LessonId> lessonRepository, 
        IRepository<Conversation, ConversationId> conversationRepository)
    {
        _lessonRepository = lessonRepository;
        _conversationRepository = conversationRepository;
    }

    public async ValueTask<Result> Handle(RemoveConversationCommand command, CancellationToken cancellationToken)
    {
        var result = Result.Success();
        
        var lesson = await _lessonRepository.Get(new LessonId(command.LessonId), result);
        var conversation = await _conversationRepository.Get(new ConversationId(command.ConversationId), result);
        
        
        if (!result.ValidateSuccessAndValues())
            return result.Fail();

        if (!lesson.RemoveConversation(conversation.Id))
            return result.Fail();

        if (!conversation.RemoveFromLesson(lesson.Id))
            return result.Fail();

        await _lessonRepository.Save(lesson, result);
        await _conversationRepository.Save(conversation, result);

        return result;
    }
}

public record RemoveConversationCommand(
    string LessonId,
    string ConversationId) : ICommand<Result>;

public class RemoveConversationCommandValidator : AbstractValidator<RemoveConversationCommand> {}