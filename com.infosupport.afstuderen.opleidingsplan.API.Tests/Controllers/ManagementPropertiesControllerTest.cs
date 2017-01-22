using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.infosupport.afstuderen.opleidingsplan.api.managers;
using Moq;
using com.infosupport.afstuderen.opleidingsplan.models;
using com.infosupport.afstuderen.opleidingsplan.api.controllers;
using com.infosupport.afstuderen.opleidingsplan.api.tests.helpers;

namespace com.infosupport.afstuderen.opleidingsplan.api.tests.controllers
{
    [TestClass]
    public class ManagementPropertiesControllerTest : ManagementPropertiesTestHelper
    {
        [TestMethod]
        public void Post_ManagementPropertiesy()
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
        public void Get_ManagementPropertiesy()
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
        }
    }
}
