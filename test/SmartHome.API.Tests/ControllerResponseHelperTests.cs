using Matty.Framework;
using Matty.Framework.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.API.Utils;
using System;
using System.Net;
using Xunit;

namespace SmartHome.API.Tests
{
    public class ControllerResponseHelperTests
    {
        [Fact]
        public void GetDefaultResponse_Validation_ThrowsExceptionWhenEmptyArg()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                ControllerResponseHelper.GetDefaultResponse(null as ServiceResult<object>);
            });
        }

        [Fact]
        public void GetDefaultResponse_Validation_ThrowsExceptionWhenDataIsNull()
        {
            var result = new Mock<ServiceResult<object>>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                ControllerResponseHelper.GetDefaultResponse(result.Object);
            });
        }

        [Fact]
        public void GetDefaultResponse_Validation_ThrowsExceptionWhenPrincipalIsNull()
        {
            var result = new Mock<ServiceResult<object>>();
            result.Object.Data = new object();

            Assert.Throws<ArgumentNullException>(() =>
            {
                ControllerResponseHelper.GetDefaultResponse(result.Object);
            });
        }


        [Fact]
        public void GetDefaultResponse_ProperlyInferInternalserverErrorActionResult()
        {
            // Arrange
            var result = new ServiceResult<object>
            {
                Data = new object(),
                Principal = new System.Security.Claims.ClaimsPrincipal()
            };
            result.Alerts.Add(new Alert { Message = "", MessageType = MessageType.Exception });

            // Act
            IActionResult response = ControllerResponseHelper.GetDefaultResponse(result);
            ObjectResult contentResponse = response as ObjectResult;

            // Assert
            Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, contentResponse.StatusCode);
            Assert.NotNull(response);
        }

        [Fact]
        public void GetDefaultResponse_ProperlyInfersBadRequestActionResult()
        {
            // Arrange
            var result = new ServiceResult<object>
            {
                Data = new object(),
                Principal = new System.Security.Claims.ClaimsPrincipal()
            };
            result.Alerts.Add(new Alert { Message = "", MessageType = MessageType.Error });

            // Act
            IActionResult response = ControllerResponseHelper.GetDefaultResponse(result);
            BadRequestObjectResult contentResponse = response as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, contentResponse.StatusCode);
            Assert.NotNull(response);
        }

        //[Fact]
        //public void GetDefaultResponse_ProperlyInfersOkActionResult()
        //{
        //    // Arrange
        //    var result = new ServiceResult<object>();
        //    result.Data = new object();
        //    result.Principal = new System.Security.Claims.ClaimsPrincipal();
        //    result.Alerts.Add(new Alert { Message = "", MessageType = MessageType.Success });

        //    // Act
        //    IActionResult response = ControllerResponseHelper.GetDefaultResponse(result);
        //    OkObjectResult contentResponse = response as OkObjectResult;

        //    // Assert
        //    Assert.IsType<ObjectResult>(response);
        //    Assert.Equal((int)HttpStatusCode.OK, contentResponse.StatusCode);
        //    Assert.NotNull(response);
        //}
    }
}
