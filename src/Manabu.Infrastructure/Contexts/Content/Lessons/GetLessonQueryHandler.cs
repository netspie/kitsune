using System.Security.Claims;
using Corelibs.Basic.Auth;
using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using Corelibs.MongoDB;
using Manabu.Entities.Content.Conversations;
using Manabu.Entities.Content.Courses;
using Manabu.Entities.Content.Lessons;
using Manabu.Entities.Content.Users;
using Manabu.Entities.Rehearse.RehearseEntities;
using Manabu.Entities.Rehearse.RehearseItems;
using Manabu.Entities.Shared;
using Manabu.UseCases.Content.Lessons;
using Mediator;
using MongoDB.Driver;

namespace Manabu.Infrastructure.Contexts.Content.Lessons;

public class GetLessonQueryHandler : IQueryHandler<GetLessonQuery, Result<GetLessonQueryResponse>>
{
    private readonly MongoConnection _mongoConnection;
    private readonly IAccessorAsync<ClaimsPrincipal> _userAccessor;
    private readonly IRepository<Course, CourseId> _courseRepository;
    private readonly IRepository<Lesson, LessonId> _lessonRepository;
    private readonly IRepository<Conversation, ConversationId> _conversationRepository;
    private readonly IRepository<RehearseEntity, RehearseEntityId> _rehearseEntityRepository;

    public GetLessonQueryHandler(
        MongoConnection mongoConnection,
        IRepository<Course, CourseId> courseRepository,
        IRepository<Lesson, LessonId> lessonRepository,
        IRepository<Conversation, ConversationId> conversationRepository,
        IRepository<RehearseItem, RehearseItemId> rehearseItemRepository,
        IAccessorAsync<ClaimsPrincipal> userAccessor,
        IRepository<RehearseEntity, RehearseEntityId> rehearseEntityRepository)
    {
        _mongoConnection = mongoConnection;
        _courseRepository = courseRepository;
        _lessonRepository = lessonRepository;
        _conversationRepository = conversationRepository;
        _userAccessor = userAccessor;
        _rehearseEntityRepository = rehearseEntityRepository;
    }

    public async ValueTask<Result<GetLessonQueryResponse>> Handle(GetLessonQuery query, CancellationToken cancellationToken)
    {
        var result = Result<GetLessonQueryResponse>.Success();

        var lesson = await _lessonRepository.Get(new LessonId(query.LessonId), result);
        if (!result.ValidateSuccessAndValues())
            return result.Fail();

        var learningObjectId = new LearningObjectId(query.LessonId);

        var collection = _mongoConnection.Database.GetCollection<RehearseEntity>(RehearseEntity.DefaultCollectionName);
        var rehearseEntityCount = collection.CountDocuments(Builders<RehearseEntity>.Filter.Eq("_id", learningObjectId));

        var userId = await _userAccessor.GetUserID<UserId>();

        var courses = await _courseRepository.Get(lesson.Courses ?? new(), result);
        var conversations = await _conversationRepository.Get(lesson.Conversations ?? new(), result);

        return result.With(
            new GetLessonQueryResponse(
                new LessonDetailsDTO(
                    lesson.Id.Value,
                    lesson.Name,
                    lesson.Description,
                    Learned: rehearseEntityCount > 0,
                    courses.OrderBy(c => lesson.Courses.IndexOf(c.Id)).Select(c => new CourseDTO(c.Id.Value, c.Name)).ToArray(),
                    conversations.OrderBy(c => lesson.Conversations.IndexOf(c.Id)).Select(c => new ConversationDTO(c.Id.Value, c.Name)).ToArray())));
    } 
}
