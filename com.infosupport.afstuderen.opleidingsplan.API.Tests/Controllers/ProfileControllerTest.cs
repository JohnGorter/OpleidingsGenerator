using System;
using Moq;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.infosupport.afstuderen.opleidingsplan.api.managers;
using com.infosupport.afstuderen.opleidingsplan.api.controllers;
using com.infosupport.afstuderen.opleidingsplan.api.tests.helpers;

namespace com.infosupport.afstuderen.opleidingsplan.api.tests.controllers
{
    [TestClass]
    public class ProfileControllerTest : AdministrationTestHelper
    {

        [TestMethod]
        public void Get_FindProfiles()
        {
            // Arrange
            var administrationManagerMock = new Mock<IAdministrationManager>(MockBehavior.Strict);
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
            var administrationManagerMock = new Mock<IAdministrationManager>(MockBehavior.Strict);
            administrationManagerMock.Setup(manager => manager.FindProfileById(1)).Returns(GetDummyDataProfiles().First());

            ProfileController controller = new ProfileController(administrationManagerMock.Object);

            // Act
            var result = controller.Get(1);

            // Assert
            administrationManagerMock.Verify(manager => manager.FindProfileById(1));
            Assert.AreEqual(31, result.Courses.Count());
            Assert.AreEqual("NET_Developer", result.Name);
        }
    }
}
