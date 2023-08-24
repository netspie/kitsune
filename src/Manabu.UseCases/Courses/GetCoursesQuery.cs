using Corelibs.Basic.Blocks;
using Corelibs.Basic.Repository;
using Manabu.Entities.Courses;
using Mediator;

namespace Manabu.UseCases.Courses;

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

        var dtos = courses.Select(c => new CourseDTO(c.Id.Value, c.Name)).ToArray();
        return result.With(new GetCoursesQueryResponse(dtos));
    }
}

public record GetCoursesQuery() : IQuery<Result<GetCoursesQueryResponse>>;

public record GetCoursesQueryResponse(CourseDTO[] Courses);

public record CourseDTO(
    string Id,
    string Name);
