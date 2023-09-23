using Corelibs.Basic.Auth;
using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using Manabu.Entities.Rehearse.RehearseContainers;
using Mediator;
using System.Security.Claims;
using Manabu.Entities.Content.Conversations;
using Manabu.Entities.Content.Courses;
using Manabu.Entities.Content.Lessons;
using Manabu.Entities.Content.Users;

namespace Manabu.UseCases.Lessons;

public class GetLessonQueryHandler : IQueryHandler<GetLessonQuery, Result<GetLessonQueryResponse>>
{
    private readonly IAccessorAsync<ClaimsPrincipal> _userAccessor;
    private readonly IRepository<Course, CourseId> _courseRepository;
    private readonly IRepository<Lesson, LessonId> _lessonRepository;
    private readonly IRepository<Conversation, ConversationId> _conversationRepository;
    private readonly IRepository<RehearseContainer, RehearseContainerId> _rehearseContainerRepository;

    public GetLessonQueryHandler(
        IRepository<Course, CourseId> courseRepository,
        IRepository<Lesson, LessonId> lessonRepository,
        IRepository<Conversation, ConversationId> conversationRepository,
        IRepository<RehearseContainer, RehearseContainerId> rehearseContainerRepository,
        IAccessorAsync<ClaimsPrincipal> userAccessor)
    {
        _courseRepository = courseRepository;
        _lessonRepository = lessonRepository;
        _conversationRepository = conversationRepository;
        _rehearseContainerRepository = rehearseContainerRepository;
        _userAccessor = userAccessor;
    }

    public async ValueTask<Result<GetLessonQueryResponse>> Handle(GetLessonQuery query, CancellationToken cancellationToken)
    {
        var result = Result<GetLessonQueryResponse>.Success();

        var lesson = await _lessonRepository.Get(new LessonId(query.LessonId), result);
        if (!result.ValidateSuccessAndValues())
            return result.Fail();

        var userId = await _userAccessor.GetUserID<UserId>();

        var rehearseContainerId = new RehearseContainerId(userId.Value, query.LessonId);
        var rehearseContainer = await _rehearseContainerRepository.Get(rehearseContainerId, result);

        var courses = await _courseRepository.Get(lesson.Courses ?? new(), result);
        var conversations = await _conversationRepository.Get(lesson.Conversations ?? new(), result);

        return result.With(
            new GetLessonQueryResponse(
                new LessonDetailsDTO(
                    lesson.Id.Value,
                    lesson.Name,
                    lesson.Description,
                    Learned: rehearseContainer is not null,
                    courses.OrderBy(c => lesson.Courses.IndexOf(c.Id)).Select(c => new CourseDTO(c.Id.Value, c.Name)).ToArray(),
                    conversations.OrderBy(c => lesson.Conversations.IndexOf(c.Id)).Select(c => new ConversationDTO(c.Id.Value, c.Name)).ToArray())));
    }
}

public record GetLessonQuery(
    string LessonId) : IQuery<Result<GetLessonQueryResponse>>;

public record GetLessonQueryResponse(LessonDetailsDTO Content);

public record LessonDetailsDTO(
    string Id,
    string Name,
    string Description,
    bool Learned,
    CourseDTO[] Courses,
    ConversationDTO[] Conversations);

public record ConversationDTO(
    string Id,
    string Name);

public record CourseDTO(
    string Id,
    string Name);
