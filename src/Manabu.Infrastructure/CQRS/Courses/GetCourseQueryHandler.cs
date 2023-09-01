using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.Basic.DDD;
using Corelibs.Basic.Repository;
using Corelibs.MongoDB;
using Manabu.Entities.Courses;
using Manabu.Entities.Lessons;
using Manabu.UseCases.Courses;
using Mediator;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Manabu.Infrastructure.CQRS.Courses;

public class GetCourseQueryHandler : IQueryHandler<GetCourseQuery, Result<GetCourseQueryResponse>>
{
    private readonly MongoConnection _mongoConnection;
    private readonly IRepository<Course, CourseId> _courseRepository;

    public GetCourseQueryHandler(
       MongoConnection mongoConnection, IRepository<Course, CourseId> courseRepository)
    {
        _mongoConnection = mongoConnection;
        _courseRepository = courseRepository;
    }

    public async ValueTask<Result<GetCourseQueryResponse>> Handle(GetCourseQuery query, CancellationToken cancellationToken)
    {
        var result = Result<GetCourseQueryResponse>.Success();

        var course = await _courseRepository.Get(new CourseId(query.CourseId), result);
        if (!result.ValidateSuccessAndValues())
            return result.Fail();

        var lessonCollection = _mongoConnection.Database.GetCollection<Lesson>(Lesson.DefaultCollectionName);

        var lessonIDs = course.Modules.SelectMany(m => m.LessonIds).ToArray();
        var lessonItems = await lessonCollection.Get<Lesson, LessonId, LessonProjection>(lessonIDs, b => b.Include(p => p.Name));
        var lessonItemsDict = lessonItems.ToDictionary(l => l.Id);

        var modulesDtos = course.Modules.Select(m => new ModuleDTO(m.Name, 
            m.LessonIds.Select(lId => new LessonDTO(lId.Value, lessonItemsDict[lId].Name)).ToArray())).ToArray();

        var lessonRemovedItems = await lessonCollection.Get<Lesson, LessonId, LessonProjection>(course.LessonsRemoved, b => b.Include(p => p.Name));
        var lessonsRemovedDtos = lessonRemovedItems.Select(l => new LessonDTO(l.Id.Value, l.Name)).ToArray();

        return result.With(
           new GetCourseQueryResponse(
               new CourseDetailsDTO(
                   course.Id.Value,
                   course.Name,
                   course.Description,
                   modulesDtos,
                   LessonsRemoved: lessonsRemovedDtos)));
    }
}

public record LessonProjection(
    LessonId Id,
    string Name);

public static class MongoProjectionExtensions
{
    public static async Task<TEntityProjection[]> Get<TEntity, TEntityId, TEntityProjection>(
        this IMongoCollection<TEntity> collection,
        IEnumerable<TEntityId> ids,
        Func<ProjectionDefinition<TEntity>, ProjectionDefinition<TEntity>> projectionBuilder)
        where TEntity : IEntity<TEntityId>
        where TEntityId : EntityId
    {
        if (ids.IsNullOrEmpty())
            return Array.Empty<TEntityProjection>();

        var filter = Builders<TEntity>.Filter.In(x => x.Id, ids);
        var projection = projectionBuilder(Builders<TEntity>.Projection.Include(x => x.Id));
        var docs = await collection.Find(filter).Project(projection).ToListAsync();

        return docs
            .Select(doc => BsonSerializer.Deserialize<TEntityProjection>(doc))
            .ToArray();
    }

    public static Task<TEntityProjection[]> Get<TEntity, TEntityId, TEntityProjection>(
        this IMongoDatabase database,
        string collectionName,
        IEnumerable<TEntityId> ids,
        Func<ProjectionDefinition<TEntity>, ProjectionDefinition<TEntity>> projectionBuilder)
        where TEntity : IEntity<TEntityId>
        where TEntityId : EntityId
    {
        var collection = database.GetCollection<TEntity>(collectionName);
        return collection.Get<TEntity, TEntityId, TEntityProjection>(ids, projectionBuilder);
    }
}
