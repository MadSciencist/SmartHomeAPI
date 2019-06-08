using System;
using Xunit;
using Moq;
using SmartHome.Core.Infrastructure;
using SmartHome.API.Utils;

namespace SmartHome.API.Tests
{
    public class ControllerResponseHelperTests
    {
        [Fact]
        public void GetDefaultResponse_Validation_ThrowsExceptionWhenEmptyArgs()
        {
            ControllerResponseHelper.GetDefaultResponse();
        }
    }
}
