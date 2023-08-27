using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using Manabu.Entities.Conversations;
using Manabu.Entities.Courses;
using Manabu.Entities.Lessons;
using Mediator;

namespace Manabu.UseCases.Lessons;

public class GetLessonQueryHandler : IQueryHandler<GetLessonQuery, Result<GetLessonQueryResponse>>
{
    private readonly IRepository<Course, CourseId> _courseRepository;
    private readonly IRepository<Lesson, LessonId> _lessonRepository;
    private readonly IRepository<Conversation, ConversationId> _conversationRepository;

    public GetLessonQueryHandler(
        IRepository<Course, CourseId> courseRepository,
        IRepository<Lesson, LessonId> lessonRepository,
        IRepository<Conversation, ConversationId> conversationRepository)
    {
        _courseRepository = courseRepository;
        _lessonRepository = lessonRepository;
        _conversationRepository = conversationRepository;
    }

    public async ValueTask<Result<GetLessonQueryResponse>> Handle(GetLessonQuery query, CancellationToken cancellationToken)
    {
        var result = Result<GetLessonQueryResponse>.Success();

        var lesson = await _lessonRepository.Get(new LessonId(query.LessonId), result);
        if (!result.ValidateSuccessAndValues())
            return result.Fail();

        var courses = await _courseRepository.Get(lesson.Courses ?? new(), result);
        var conversations = await _conversationRepository.Get(lesson.Conversations ?? new(), result);

        return result.With(
            new GetLessonQueryResponse(
                new LessonDetailsDTO(
                    lesson.Id.Value,
                    lesson.Name,
                    lesson.Description,
                    courses.Select(c => new CourseDTO(c.Id.Value, c.Name)).ToArray(),
                    conversations.Select(c => new ConversationDTO(c.Id.Value, c.Name)).ToArray())));
    }
}

public record GetLessonQuery(
    string LessonId) : IQuery<Result<GetLessonQueryResponse>>;

public record GetLessonQueryResponse(LessonDetailsDTO Course);

public record LessonDetailsDTO(
    string Id,
    string Name,
    string Description,
    CourseDTO[] Courses,
    ConversationDTO[] Conversations);

public record ConversationDTO(
    string Id,
    string Name);

public record CourseDTO(
    string Id,
    string Name);
