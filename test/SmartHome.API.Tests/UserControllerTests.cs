//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using SmartHome.API.Controllers;
//using SmartHome.API.Dto;
//using SmartHome.API.Service;
//using SmartHome.Core.Infrastructure;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using Xunit;

//namespace SmartHome.API.Tests
//{
//    public class UserControllerTests
//    {
//        private readonly Mock<IUserService> _userServiceMock;

//        public UserControllerTests()
//        {
//            _userServiceMock = new Mock<IUserService>();
//        }

//        [Fact]
//        public async Task LoginEndpointTest()
//        {
//            ServiceResult<TokenDto> result = new ServiceResult<TokenDto>(Helpers.GetFakeAdminPrincipal());
//            _userServiceMock.Setup(service => service.LoginAsync(It.IsAny<LoginDto>(), It.IsAny<ClaimsPrincipal>()))
//                .Returns(Task.FromResult(result));

//            UsersController controller = new UsersController(_userServiceMock.Object)
//            {
//                ControllerContext = Helpers.GetFakeHttpContextWithFakeAdminPrincipal()
//            };

//            IActionResult loginResponse = await controller.Login(new LoginDto
//            {
//                Login = "fake",
//                Password = "fake11"
//            });

//            Assert.NotNull(loginResponse);
//        }
//    }
//}
