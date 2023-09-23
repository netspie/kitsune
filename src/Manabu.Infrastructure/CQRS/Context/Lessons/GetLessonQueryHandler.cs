//using Corelibs.Basic.Blocks;
//using Corelibs.Basic.Repository;
//using Corelibs.MongoDB;
//using Manabu.Entities.Conversations;
//using Manabu.Entities.Courses;
//using Manabu.Entities.Lessons;
//using Manabu.UseCases.Lessons;
//using Mediator;
//using MongoDB.Bson.Serialization;
//using MongoDB.Driver;

//namespace Manabu.Infrastructure.CQRS.Rehearse.Lessons;

//public class GetLessonQueryHandler : IQueryHandler<GetLessonQuery, Result<GetLessonQueryResponse>>
//{
//    private readonly MongoConnection _mongoConnection;
//    private readonly IRepository<Lesson, LessonId> _lessonRepository;

//    public GetLessonQueryHandler(
//       MongoConnection mongoConnection, IRepository<Lesson, LessonId> lessonRepository)
//    {
//        _mongoConnection = mongoConnection;
//        _lessonRepository = lessonRepository;
//    }

//    public async ValueTask<Result<GetLessonQueryResponse>> Handle(GetLessonQuery query, CancellationToken cancellationToken)
//    {
//        var result = Result<GetLessonQueryResponse>.Success();

//        var lesson = await _lessonRepository.Get(new LessonId(query.LessonId), result);
//        if (!result.ValidateSuccessAndValues())
//            return result.Fail();

//        var courseCollection = _mongoConnection.Database.GetCollection<Course>(Course.DefaultCollectionName);
//        var conversationCollection = _mongoConnection.Database.GetCollection<Conversation>(Conversation.DefaultCollectionName);

//        var convFilter = Builders<Conversation>.Filter.In(x => x.Id, lesson.Conversations);
//        var convProjection = Builders<Conversation>.Projection
//            .Include(x => x.Id)
//            .Include(x => x.Name);

//        var convDocs = await conversationCollection.Find(convFilter).Project(convProjection).ToListAsync();
//        var convs = convDocs
//            .Select(doc => BsonSerializer.Deserialize<ConversationProjection>(doc))
//            .OrderBy(c => lesson.Conversations.IndexOf(c.Id))
//            .ToArray();

//        //var courses = await _courseRepository.Get(lesson.Courses ?? new(), result);

//        return result.With(
//            new GetLessonQueryResponse(
//                new LessonDetailsDTO(
//                    lesson.Id.Value,
//                    lesson.Name,
//                    lesson.Description,
//                    null,//courses.OrderBy(c => lesson.Courses.IndexOf(c.Id)).Select(c => new CourseDTO(c.Id.Value, c.Name)).ToArray(),
//                    convs.Select(c => new ConversationDTO(c.Id.Value, c.Name)).ToArray())));
//    }
//}

//public record ConversationProjection(
//    ConversationId Id,
//    string Name);
