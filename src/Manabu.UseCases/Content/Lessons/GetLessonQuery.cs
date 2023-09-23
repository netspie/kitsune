using Corelibs.Basic.Blocks;
using Mediator;

namespace Manabu.UseCases.Content.Lessons;

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
