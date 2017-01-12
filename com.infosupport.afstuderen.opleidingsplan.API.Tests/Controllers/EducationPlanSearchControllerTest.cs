using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using com.infosupport.afstuderen.opleidingsplan.api.managers;
using com.infosupport.afstuderen.opleidingsplan.api.controllers;
using com.infosupport.afstuderen.opleidingsplan.api.models;
using com.infosupport.afstuderen.opleidingsplan.models;
using System.Collections.Generic;
using com.infosupport.afstuderen.opleidingsplan.api.tests.helpers;

namespace com.infosupport.afstuderen.opleidingsplan.api.tests.controllers
{
    [TestClass]
    public class EducationPlanSearchControllerTest : EducationPlanTestHelper
    {
        [TestMethod]
        public void Search_ManagerCalled()
        {
            // Arrange
            var educationPlanManagerMock = new Mock<IEducationPlanManager>(MockBehavior.Strict);
            educationPlanManagerMock.Setup(manager => manager.FindEducationPlans(It.IsAny<EducationPlanSearch>())).Returns(new List<EducationPlan>() { GetDummyEducationPlan() });

            EducationPlanSearchController controller = new EducationPlanSearchController(educationPlanManagerMock.Object);

            // Act
            controller.Get("Pim", null);

            // Assert
            educationPlanManagerMock.Verify(manager => manager.FindEducationPlans(It.IsAny<EducationPlanSearch>()));
        }
    }
}
