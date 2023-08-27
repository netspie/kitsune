using Corelibs.Basic.Auth;
using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using FluentValidation;
using Manabu.Entities.Conversations;
using Manabu.Entities.Lessons;
using Manabu.Entities.Users;
using Mediator;
using System.Security.Claims;

namespace Manabu.UseCases.Conversations;

public class CreateConversationCommandHandler : ICommandHandler<CreateConversationCommand, Result>
{
    private readonly IAccessorAsync<ClaimsPrincipal> _userAccessor;
    private readonly IRepository<Conversation, ConversationId> _conversationRepository;
    private readonly IRepository<Lesson, LessonId> _lessonRepository;

    public CreateConversationCommandHandler(
        IAccessorAsync<ClaimsPrincipal> userAccessor,
        IRepository<Conversation, ConversationId> conversationRepository,
        IRepository<Lesson, LessonId> lessonRepository)
    {
        _userAccessor = userAccessor;
        _conversationRepository = conversationRepository;
        _lessonRepository = lessonRepository;
    }

    public async ValueTask<Result> Handle(CreateConversationCommand command, CancellationToken ct)
    {
        var result = Result.Success();

        var userId = await _userAccessor.GetUserID<UserId>();

        var lesson = await _lessonRepository.Get(new LessonId(command.LessonId), result);
        var conversation = new Conversation(
            command.Name, lesson.Id, userId);

        lesson.AddConversation(conversation.Id, command.Index);

        await _conversationRepository.Save(conversation, result);
        await _lessonRepository.Save(lesson, result);

        return result;
    }
}

public record CreateConversationCommand(
    string Name,
    string LessonId,
    int Index = 0) : ICommand<Result>;

public class CreateConversationCommandValidator : AbstractValidator<CreateConversationCommand> {}
