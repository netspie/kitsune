using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using Manabu.Entities.Content.Courses;
using Mediator;

namespace Manabu.UseCases.Content.Courses;

public class GetCoursesQueryHandler : IQueryHandler<GetCoursesQuery, Result<GetCoursesQueryResponse>>
{
    private readonly IRepository<Course, CourseId> _courseRepository;

    public GetCoursesQueryHandler(IRepository<Course, CourseId> courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async ValueTask<Result<GetCoursesQueryResponse>> Handle(GetCoursesQuery query, CancellationToken cancellationToken)
    {
        var result = Result<GetCoursesQueryResponse>.Success();

        var courses = await _courseRepository.GetAll(result);
        if (!result.ValidateSuccessAndValues())
            return result.Fail();

        var courseDtos = courses.Where(c => !c.IsArchived).OrderBy(x=>x.Order).Select(c => new CourseDTO(c.Id.Value, c.Name)).ToArray();
        var courseArchivedDtos = courses.Where(c => c.IsArchived).OrderBy(x => x.Order).Select(c => new CourseDTO(c.Id.Value, c.Name)).ToArray();

        return result.With(new GetCoursesQueryResponse(
                new CoursesDTO(
                    courseDtos, 
                    courseArchivedDtos)));
    }
}

public record GetCoursesQuery() : IQuery<Result<GetCoursesQueryResponse>>;

public record GetCoursesQueryResponse(CoursesDTO Content);

public record CoursesDTO(
    CourseDTO[] Courses,
    CourseDTO[] CoursesArchived);

public record CourseDTO(
    string Id,
    string Name);
