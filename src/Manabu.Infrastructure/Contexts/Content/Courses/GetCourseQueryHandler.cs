using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.Basic.Repository;
using Corelibs.MongoDB;
using Manabu.Entities.Content.Courses;
using Manabu.Entities.Content.Lessons;
using Manabu.UseCases.Content.Courses;
using Mediator;
using MongoDB.Driver;

namespace Manabu.Infrastructure.CQRS.Content.Courses;

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
        if (course.Modules.Any())
        {
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
        else
        {
            return result.With(
                new GetCourseQueryResponse(
              new CourseDetailsDTO(
                  course.Id.Value,
                  course.Name,
                  course.Description,
                  new ModuleDTO[] { },
                  LessonsRemoved: new LessonDTO[] { })));
        }
    }
}

public record LessonProjection(
    LessonId Id,
    string Name);