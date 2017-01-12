using System;
using Moq;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.infosupport.afstuderen.opleidingsplan.api.managers;
using com.infosupport.afstuderen.opleidingsplan.api.controllers;
using com.infosupport.afstuderen.opleidingsplan.api.tests.helpers;
using com.infosupport.afstuderen.opleidingsplan.models;

namespace com.infosupport.afstuderen.opleidingsplan.api.tests.controllers
{
    [TestClass]
    public class ProfileControllerTest : ProfileTestHelper
    {

        [TestMethod]
        public void Get_FindProfiles()
        {
            // Arrange
            var administrationManagerMock = new Mock<IProfileManager>(MockBehavior.Strict);
            administrationManagerMock.Setup(manager => manager.FindProfiles()).Returns(GetDummyDataProfiles());

            ProfileController controller = new ProfileController(administrationManagerMock.Object);

            // Act
            var result = controller.Get();

            // Assert
            administrationManagerMock.Verify(manager => manager.FindProfiles());
            Assert.AreEqual(3, result.Count());
            Assert.AreEqual("NET_Developer", result.ElementAt(0).Name);
            Assert.AreEqual("Developer_Mobile", result.ElementAt(1).Name);
            Assert.AreEqual("JAVA_Developer", result.ElementAt(2).Name);
        }

        [TestMethod]
        public void Get_NET_Developer_FindProfile_31_Courses()
        {
            // Arrange
            var administrationManagerMock = new Mock<IProfileManager>(MockBehavior.Strict);
            administrationManagerMock.Setup(manager => manager.FindProfileById(1)).Returns(GetDummyDataProfiles().First());

            ProfileController controller = new ProfileController(administrationManagerMock.Object);

            // Act
            var result = controller.Get(1);

            // Assert
            administrationManagerMock.Verify(manager => manager.FindProfileById(1));
            Assert.AreEqual(31, result.Courses.Count());
            Assert.AreEqual("NET_Developer", result.Name);
        }

        [TestMethod]
        public void Get_null_ReturnsEmptyCourseProfile()
        {
            // Arrange
            var administrationManagerMock = new Mock<IProfileManager>(MockBehavior.Strict);
            ProfileController controller = new ProfileController(administrationManagerMock.Object);

            // Act
            var result = controller.Get(null);

            // Assert
            Assert.AreEqual(0, result.Courses.Count());
            Assert.IsNull(result.Name);
        }

        [TestMethod]
        public void Post_CourseProfile()
        {
            // Arrange
            var administrationManagerMock = new Mock<IProfileManager>(MockBehavior.Strict);
            administrationManagerMock.Setup(manager => manager.Update(It.IsAny<CourseProfile>()));

            ProfileController controller = new ProfileController(administrationManagerMock.Object);

            // Act
            controller.Post(GetDummyDataProfiles().First());

            // Assert
            administrationManagerMock.Verify(manager => manager.Update(It.IsAny<CourseProfile>()));
        }

        [TestMethod]
        public void Put_CourseProfile()
        {
            // Arrange
            var administrationManagerMock = new Mock<IProfileManager>(MockBehavior.Strict);
            administrationManagerMock.Setup(manager => manager.Insert(It.IsAny<CourseProfile>()));

            ProfileController controller = new ProfileController(administrationManagerMock.Object);

            // Act
            controller.Put(GetDummyDataProfiles().First());

            // Assert
            administrationManagerMock.Verify(manager => manager.Insert(It.IsAny<CourseProfile>()));
        }

        [TestMethod]
        public void Delete_CourseProfile()
        {
            // Arrange
            var administrationManagerMock = new Mock<IProfileManager>(MockBehavior.Strict);
            administrationManagerMock.Setup(manager => manager.Delete(It.IsAny<CourseProfile>()));

            ProfileController controller = new ProfileController(administrationManagerMock.Object);

            // Act
            controller.Delete(GetDummyDataProfiles().First());

            // Assert
            administrationManagerMock.Verify(manager => manager.Delete(It.IsAny<CourseProfile>()));
        }
    }
}
