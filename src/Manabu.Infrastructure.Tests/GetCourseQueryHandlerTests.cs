using Corelibs.Basic.Repository;
using Corelibs.MongoDB;
using FluentAssertions;
using Manabu.Entities.Content.Courses;
using Manabu.Entities.Content.Lessons;
using Manabu.Infrastructure.CQRS.Content.Courses;
using MongoDB.Driver;
using Moq;
using System.Reflection;

namespace Manabu.Infrastructure.Tests
{
    public class GetCourseQueryHandlerTests
    {
        private readonly MongoConnection _mongoConnection;
        private readonly Mock<IRepository<Course,CourseId>> _courseRepositoryMock;
        private readonly Mock<IClientSessionHandle> _clientSeesionHandler;
        private readonly GetCourseQueryHandler _handler;

        public GetCourseQueryHandlerTests()
        {
            _mongoConnection = new MongoConnection("Kitsune_dev");
            var collectionMock = Mock.Of<IMongoCollection<Lesson>>();
            _clientSeesionHandler = new Mock<IClientSessionHandle>();
            var dbMock = new Mock<IMongoDatabase>();
            dbMock.Setup(_ => _.GetCollection<Lesson>(It.IsAny<string>(),null))
                .Returns(collectionMock);

            _mongoConnection.Database = dbMock.Object;
            _mongoConnection.Session = _clientSeesionHandler.Object;
            _courseRepositoryMock = new Mock<IRepository<Course, CourseId>>();
            _handler = new GetCourseQueryHandler(_mongoConnection, _courseRepositoryMock.Object);
        }

        [Test]
        public async Task Handle_CourseWithModules_ReturnArray()
        {
            _courseRepositoryMock.Setup(x => x.GetBy(It.IsAny<CourseId>())).ReturnsAsync(new Corelibs.Basic.Blocks.Result<Course>(new Course("Test Course",new Entities.Content.Users.UserId("TestUser"))
            {
                Modules = new List<Course.Module>()
            }));

            var result =  await _handler.Handle(new UseCases.Content.Courses.GetCourseQuery(It.IsAny<string>()),CancellationToken.None);
            result.Values.Count().Should().Be(2);
        }

        [Test]
        public async Task Handle_CourseWithoutModules_ReturnArray()
        {
            _courseRepositoryMock.Setup(x => x.GetBy(It.IsAny<CourseId>())).ReturnsAsync(new Corelibs.Basic.Blocks.Result<Course>(new Course("Test Course", new Entities.Content.Users.UserId("TestUser"))
            {
                Modules = new List<Course.Module>()
                {
                    new Course.Module("Module 1", new List<LessonId>()
                    {
                        
                    })
                }
            }));

            var result = await _handler.Handle(new UseCases.Content.Courses.GetCourseQuery(It.IsAny<string>()), CancellationToken.None);
            result.Values.Count().Should().Be(2);
        }
    }
}
