using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using com.infosupport.afstuderen.opleidingsplan.dal.mappers;
using com.infosupport.afstuderen.opleidingsplan.api.tests.helpers;
using com.infosupport.afstuderen.opleidingsplan.api.Managers;
using com.infosupport.afstuderen.opleidingsplan.models;

namespace com.infosupport.afstuderen.opleidingsplan.api.tests.managers
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
            Assert.AreEqual(150, result.OLCPrice);
            Assert.AreEqual(2, result.PeriodAfterLastCourseEmployableInDays);
            Assert.AreEqual(4, result.PeriodBeforeStartNotifiable);
            Assert.AreEqual(100, result.PeriodEducationPlanInDays);
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
