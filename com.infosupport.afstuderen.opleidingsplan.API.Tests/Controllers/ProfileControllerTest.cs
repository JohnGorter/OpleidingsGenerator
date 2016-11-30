using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.infosupport.afstuderen.opleidingsplan.api.Managers;
using Moq;
using com.infosupport.afstuderen.opleidingsplan.api.Controllers;
using System.Linq;
using com.infosupport.afstuderen.opleidingsplan.API.tests.helpers;

namespace com.infosupport.afstuderen.opleidingsplan.API.tests.controllers
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
            administrationManagerMock.Setup(manager => manager.FindProfile("NET_Developer")).Returns(GetDummyDataProfiles().First());

            ProfileController controller = new ProfileController(administrationManagerMock.Object);

            // Act
            var result = controller.Get("NET_Developer");

            // Assert
            administrationManagerMock.Verify(manager => manager.FindProfile("NET_Developer"));
            Assert.AreEqual(31, result.Courses.Count());
        }
    }
}
