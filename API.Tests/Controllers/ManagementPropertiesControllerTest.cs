using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InfoSupport.KC.OpleidingsplanGenerator.Api.Managers;
using Moq;
using InfoSupport.KC.OpleidingsplanGenerator.Models;
using InfoSupport.KC.OpleidingsplanGenerator.Api.Controllers;
using InfoSupport.KC.OpleidingsplanGenerator.Api.Tests.Helpers;

namespace InfoSupport.KC.OpleidingsplanGenerator.Api.Tests.Controllers
{
    [TestClass]
    public class ManagementPropertiesControllerTest : ManagementPropertiesTestHelper
    {
        [TestMethod]
        public void Post_ManagementProperties()
        {
            // Arrange
            var managementPropertiesManagerMock = new Mock<IManagementPropertiesManager>(MockBehavior.Strict);
            managementPropertiesManagerMock.Setup(manager => manager.Update(It.IsAny<ManagementProperties>()));

            ManagementPropertiesController controller = new ManagementPropertiesController(managementPropertiesManagerMock.Object);

            // Act
            controller.Post(GetDummyDataManagementProperties());

            // Assert
            managementPropertiesManagerMock.Verify(manager => manager.Update(It.IsAny<ManagementProperties>()));
        }

        [TestMethod]
        public void Get_ManagementProperties()
        {
            // Arrange
            var managementPropertiesManagerMock = new Mock<IManagementPropertiesManager>(MockBehavior.Strict);
            managementPropertiesManagerMock.Setup(manager => manager.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            ManagementPropertiesController controller = new ManagementPropertiesController(managementPropertiesManagerMock.Object);

            // Act
            var result = controller.Get();

            // Assert
            managementPropertiesManagerMock.Verify(manager => manager.FindManagementProperties());
            Assert.AreEqual(150, result.OlcPrice);
            Assert.AreEqual(2, result.PeriodAfterLastCourseEmployableInDays);
            Assert.AreEqual(4, result.PeriodBeforeStartNotifiable);
            Assert.AreEqual(100, result.PeriodEducationPlanInDays);
            Assert.AreEqual("new footer", result.Footer);
            Assert.AreEqual(80, result.StaffDiscount);
        }
    }
}
