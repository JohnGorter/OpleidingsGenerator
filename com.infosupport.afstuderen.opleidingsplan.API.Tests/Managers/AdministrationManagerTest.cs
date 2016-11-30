using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using com.infosupport.afstuderen.opleidingsplan.DAL.mapper;
using com.infosupport.afstuderen.opleidingsplan.model;
using com.infosupport.afstuderen.opleidingsplan.API.tests.helpers;
using com.infosupport.afstuderen.opleidingsplan.api.Managers;
using System.Linq;

namespace com.infosupport.afstuderen.opleidingsplan.API.tests.managers
{
    [TestClass]
    public class AdministrationManagerTest : AdministrationTestHelper
    {
        [TestMethod]
        public void FindProfilesTest()
        {
            // Arrange
            var profileDataMapperMock = new Mock<IDataMapper<Profile>>(MockBehavior.Strict);
            profileDataMapperMock.Setup(service => service.FindAll()).Returns(GetDummyDataProfiles());

            AdministrationManager manager = new AdministrationManager(profileDataMapperMock.Object);

            // Act
            var result = manager.FindProfiles();

            // Assert
            profileDataMapperMock.Verify(service => service.FindAll());
            Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        public void FindProfile_NET_Developer_31_CoursesTest()
        {
            // Arrange
            var profileDataMapperMock = new Mock<IDataMapper<Profile>>(MockBehavior.Strict);
            profileDataMapperMock.Setup(service => service.Find(It.IsAny<Func<Profile, bool>>())).Returns(GetDummyDataProfiles());

            AdministrationManager manager = new AdministrationManager(profileDataMapperMock.Object);

            // Act
            var result = manager.FindProfile("NET_Developer");

            // Assert
            profileDataMapperMock.Verify(service => service.Find(It.IsAny<Func<Profile, bool>>()));
            Assert.AreEqual(31, result.Courses.Count());
        }
    }
}
