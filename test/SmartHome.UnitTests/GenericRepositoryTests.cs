//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using Moq;
//using SmartHome.Core.DataAccess;
//using SmartHome.Core.DataAccess.Repository;
//using SmartHome.Core.Domain.Entity;
//using System.Threading.Tasks;
//using Xunit;

//namespace SmartHome.DataAccess.Tests
//{
//    public class GenericRepositoryTests
//    {
//        private readonly Mock<ILoggerFactory> _mockLoggerFactory;

//        public GenericRepositoryTests()
//        {
//            _mockLoggerFactory = new Mock<ILoggerFactory>();
//        }

//        [Fact]
//        public async Task CanAddNewNode()
//        {
//            var options = new DbContextOptionsBuilder<AppDbContext>()
//                .UseInMemoryDatabase("IN_MEMORY_DATABASE")
//                .Options;

//            using (var context = new AppDbContext(options))
//            {
//                var repo = new GenericRepository<RegisteredSensors>(context, _mockLoggerFactory.Object);

//                var registeredSensor = new RegisteredSensors
//                {
//                    Id = 111,
//                    Name = "test",
//                    Description = "desc"
//                };

//                await repo.CreateAsync(registeredSensor);

//                var getByIdResult = await repo.GetByIdAsync(registeredSensor.Id);

//                Assert.Single(repo.GetAll());
//                Assert.Equal(registeredSensor, getByIdResult);
//            }
//        }
//    }
//}
