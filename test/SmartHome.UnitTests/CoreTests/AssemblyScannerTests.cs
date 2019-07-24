using System;
using System.Collections.Generic;
using System.Text;
using SmartHome.Core.Infrastructure.AssemblyScanning;
using Xunit;

namespace SmartHome.Core.Tests.CoreTests
{
    public class AssemblyScannerTests
    {
        // TODO need to mock somehow AppDomain.CurrentDomain.BaseDirectory cause tests are executed from other assembly
        [Fact]
        public void GetAssemblyModuleNameByProductInfo_ExistingProductInfoGiven_ShouldReturnProperName()
        {
            const string productInfo = "Espurna-MQTT-v1";

            string result = AssemblyScanner.GetAssemblyModuleNameByProductInfo(productInfo);

            const string expectedResult = "SmartHome.Contracts.EspurnaMqtt.dll";

            Assert.Equal(expectedResult, result);
        }
    }
}
