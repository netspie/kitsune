using Corelibs.Basic.Blocks;
using Corelibs.Basic.Collections;
using Corelibs.Basic.Repository;
using Manabu.Entities.Courses;
using Mediator;

namespace Manabu.UseCases.Courses;

public class GetCourseQueryHandler : IQueryHandler<GetCourseQuery, Result<GetCourseQueryResponse>>
{
    private readonly IRepository<Course, CourseId> _courseRepository;

    public GetCourseQueryHandler(IRepository<Course, CourseId> courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async ValueTask<Result<GetCourseQueryResponse>> Handle(GetCourseQuery query, CancellationToken cancellationToken)
    {
        var result = Result<GetCourseQueryResponse>.Success();

        var course = await _courseRepository.Get(new CourseId(query.CourseId), result);
        if (!result.ValidateSuccessAndValues())
            return result.Fail();

        var modules = course.Modules?
            .SelectOrDefault(m => 
                new ModuleDTO(
                    m.Name, 
                    m.Lessons?.SelectOrDefault(l => 
                        new LessonDTO(l.Id.Value, l.Name)).ToArray()))
            .ToArray();

        return result.With(
            new GetCourseQueryResponse(
                new CourseDetailsDTO(
                    course.Id.Value, 
                    course.Name, 
                    course.Description, 
                    modules)));
    }
}

public record GetCourseQuery(
    string CourseId) : IQuery<Result<GetCourseQueryResponse>>;

public record GetCourseQueryResponse(CourseDetailsDTO Course);

public record CourseDetailsDTO(
    string Id,
    string Name,
    string Description,
    ModuleDTO[] Modules);

public record ModuleDTO(
    string Name,
    LessonDTO[]? Lessons = null);

public record LessonDTO(
    string Id,
    string Name);
