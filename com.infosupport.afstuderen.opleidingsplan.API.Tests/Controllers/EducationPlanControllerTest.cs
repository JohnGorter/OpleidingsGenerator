﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.infosupport.afstuderen.opleidingsplan.api;
using com.infosupport.afstuderen.opleidingsplan.api.controllers;
using com.infosupport.afstuderen.opleidingsplan.api.managers;
using Moq;
using com.infosupport.afstuderen.opleidingsplan.api.models;
using com.infosupport.afstuderen.opleidingsplan.api.tests.helpers;
using System.Collections.ObjectModel;
using com.infosupport.afstuderen.opleidingsplan.models;
using System.Collections.Generic;

namespace com.infosupport.afstuderen.opleidingsplan.api.tests.controllers
{
    [TestClass]
    public class EducationPlanControllerTest : EducationPlanTestHelper
    {
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            AutoMapperConfiguration.Configure();
        }


        [TestMethod]
        public void GenerateEducationPlan_ManagerCalled()
        {
            // Arrange
            RestEducationPlan restEducationPlan = GetDummyRestEducationPlan(new Collection<string> { "2NETARCH", "ADCSB" });

            var educationPlanManagerMock = new Mock<IEducationPlanManager>(MockBehavior.Strict);
            educationPlanManagerMock.Setup(manager => manager.GenerateEducationPlan(restEducationPlan)).Returns(GetDummyEducationPlan());

            EducationPlanController controller = new EducationPlanController(educationPlanManagerMock.Object);

            // Act
            controller.GenerateEducationPlan(restEducationPlan);


            // Assert
            educationPlanManagerMock.Verify(manager => manager.GenerateEducationPlan(restEducationPlan));

        }

        [TestMethod]
        public void Post_ManagerCalled()
        {
            // Arrange
            RestEducationPlan restEducationPlan = GetDummyRestEducationPlan(new Collection<string> { "2NETARCH", "ADCSB" });

            var educationPlanManagerMock = new Mock<IEducationPlanManager>(MockBehavior.Strict);
            educationPlanManagerMock.Setup(manager => manager.UpdateEducationPlan(restEducationPlan)).Returns(1);

            EducationPlanController controller = new EducationPlanController(educationPlanManagerMock.Object);

            // Act
            var result = controller.Post(restEducationPlan);


            // Assert
            Assert.AreEqual(1, result);
            educationPlanManagerMock.Verify(manager => manager.UpdateEducationPlan(restEducationPlan));

        }

        [TestMethod]
        public void Put_ManagerCalled()
        {
            // Arrange
            RestEducationPlan restEducationPlan = GetDummyRestEducationPlan(new Collection<string> { "2NETARCH", "ADCSB" });

            var educationPlanManagerMock = new Mock<IEducationPlanManager>(MockBehavior.Strict);
            educationPlanManagerMock.Setup(manager => manager.SaveEducationPlan(restEducationPlan)).Returns(1);

            EducationPlanController controller = new EducationPlanController(educationPlanManagerMock.Object);

            // Act
            var result = controller.Put(restEducationPlan);


            // Assert
            Assert.AreEqual(1, result);
            educationPlanManagerMock.Verify(manager => manager.SaveEducationPlan(restEducationPlan));
        }

        [TestMethod]
        public void Get_ManagerCalled()
        {
            // Arrange
            RestEducationPlan restEducationPlan = GetDummyRestEducationPlan(new Collection<string> { "2NETARCH", "ADCSB" });

            var educationPlanManagerMock = new Mock<IEducationPlanManager>(MockBehavior.Strict);
            educationPlanManagerMock.Setup(manager => manager.FindEducationPlan(1)).Returns(GetDummyEducationPlan());

            EducationPlanController controller = new EducationPlanController(educationPlanManagerMock.Object);

            // Act
            controller.Get(1);

            // Assert
            educationPlanManagerMock.Verify(manager => manager.FindEducationPlan(1));
        }

        [TestMethod]
        public void Search_ManagerCalled()
        {
            // Arrange
            var educationPlanManagerMock = new Mock<IEducationPlanManager>(MockBehavior.Strict);
            educationPlanManagerMock.Setup(manager => manager.FindEducationPlans(It.IsAny<EducationPlanSearch>())).Returns(new List<EducationPlan>() { GetDummyEducationPlan() });

            EducationPlanController controller = new EducationPlanController(educationPlanManagerMock.Object);

            // Act
            controller.Get("Pim", 1);

            // Assert
            educationPlanManagerMock.Verify(manager => manager.FindEducationPlans(It.IsAny<EducationPlanSearch>()));
        }

        [TestMethod]
        public void GenerateWordFile_ManagerCalled()
        {
            // Arrange
            var educationPlanManagerMock = new Mock<IEducationPlanManager>(MockBehavior.Strict);
            educationPlanManagerMock.Setup(manager => manager.FindEducationPlan(1)).Returns(GetDummyEducationPlan());
            educationPlanManagerMock.Setup(manager => manager.GenerateWordFile(It.IsAny<EducationPlan>())).Returns("path");

            EducationPlanController controller = new EducationPlanController(educationPlanManagerMock.Object);

            // Act
            var result = controller.GenerateWordFile(1);

            // Assert
            Assert.AreEqual("path", result);
            educationPlanManagerMock.Verify(manager => manager.FindEducationPlan(1));
            educationPlanManagerMock.Verify(manager => manager.GenerateWordFile(It.IsAny<EducationPlan>()));
        }
    }
}
