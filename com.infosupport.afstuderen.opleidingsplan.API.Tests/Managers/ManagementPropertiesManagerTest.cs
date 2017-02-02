using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using InfoSupport.KC.OpleidingsplanGenerator.Dal.Mappers;
using InfoSupport.KC.OpleidingsplanGenerator.Api.Tests.Helpers;
using InfoSupport.KC.OpleidingsplanGenerator.Api.Managers;
using InfoSupport.KC.OpleidingsplanGenerator.Models;

namespace InfoSupport.KC.OpleidingsplanGenerator.Api.Tests.Managers
{
    [TestClass]
    public class ManagementPropertiesManagerTest : ManagementPropertiesTestHelper
    {

        [TestMethod]
        public void FindManagementProperties()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(service => service.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            ManagementPropertiesManager manager = new ManagementPropertiesManager(managementPropertiesDataMapperMock.Object);

            // Act
            var result = manager.FindManagementProperties();

            // Assert
            managementPropertiesDataMapperMock.Verify(service => service.FindManagementProperties());
            Assert.AreEqual(150, result.OlcPrice);
            Assert.AreEqual(2, result.PeriodAfterLastCourseEmployableInDays);
            Assert.AreEqual(4, result.PeriodBeforeStartNotifiable);
            Assert.AreEqual(100, result.PeriodEducationPlanInDays);
            Assert.AreEqual("new footer", result.Footer);
            Assert.AreEqual(80, result.StaffDiscount);
        }

        [TestMethod]
        public void UpdateManagementProperties()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(service => service.Update(It.IsAny<ManagementProperties>()));

            ManagementPropertiesManager manager = new ManagementPropertiesManager(managementPropertiesDataMapperMock.Object);

            // Act
            manager.Update(GetDummyDataManagementProperties());

            // Assert
            managementPropertiesDataMapperMock.Verify(service => service.Update(It.IsAny<ManagementProperties>()));
        }
    }
}
