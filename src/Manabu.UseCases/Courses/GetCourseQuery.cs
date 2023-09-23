using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.Basic.Repository;
using Mediator;

namespace Manabu.UseCases.Courses;

//public class GetCourseQueryHandler : IQueryHandler<GetCourseQuery, Result<GetCourseQueryResponse>>
//{
//    private readonly IRepository<Course, CourseId> _courseRepository;
//    private readonly IRepository<Lesson, LessonId> _lessonRepository;

//    public GetCourseQueryHandler(
//        IRepository<Course, CourseId> courseRepository,
//        IRepository<Lesson, LessonId> lessonRepository)
//    {
//        _courseRepository = courseRepository;
//        _lessonRepository = lessonRepository;
//    }

//    public async ValueTask<Result<GetCourseQueryResponse>> Handle(GetCourseQuery query, CancellationToken cancellationToken)
//    {
//        var result = Result<GetCourseQueryResponse>.Success();

//        var course = await _courseRepository.Get(new CourseId(query.CourseId), result);
//        if (!result.ValidateSuccessAndValues())
//            return result.Fail();

//        var modulesTasks = course.Modules?.SelectOrDefault(m => m.LessonIds?.SelectOrDefault(lessonId =>
//            _lessonRepository.Get(lessonId, result)));

//        var lessons = await Task.WhenAll(modulesTasks.SelectOrDefault(m => Task.WhenAll(m)));
//        if (!result.ValidateSuccessAndValues())
//            return result.Fail();

//        var modulesDtos = course.Modules?
//            .SelectOrDefault((m, i) => 
//                new ModuleDTO(
//                    m.Name,
//                    lessons[i].SelectOrDefault(l => new LessonDTO(l.Id.Value, l.Name)).ToArray()))
//            .ToArray();

//        var lessonsRemoved = await _lessonRepository.Get(course.LessonsRemoved ?? new(), result);
//        var lessonsRemovedDtos = lessonsRemoved.Select(l => new LessonDTO(l.Id.Value, l.Name)).ToArray();

//        return result.With(
//            new GetCourseQueryResponse(
//                new CourseDetailsDTO(
//                    course.Id.Value, 
//                    course.Name, 
//                    course.Description, 
//                    modulesDtos,
//                    LessonsRemoved: lessonsRemovedDtos)));
//    }
//}

public record GetCourseQuery(
    string CourseId) : IQuery<Result<GetCourseQueryResponse>>;

public record GetCourseQueryResponse(CourseDetailsDTO Course);

public record CourseDetailsDTO(
    string Id,
    string Name,
    string Description,
    ModuleDTO[] Modules,
    ModuleDTO[]? ModulesRemoved = null,
    LessonDTO[]? LessonsRemoved = null);

public record ModuleDTO(
    string Name,
    LessonDTO[]? Lessons = null);

public record LessonDTO(
    string Id,
    string Name);
