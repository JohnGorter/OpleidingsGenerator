using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.infosupport.afstuderen.opleidingsplan.api;
using com.infosupport.afstuderen.opleidingsplan.api.Controllers;
using com.infosupport.afstuderen.opleidingsplan.api.Managers;
using Moq;
using com.infosupport.afstuderen.opleidingsplan.api.Models;

namespace com.infosupport.afstuderen.opleidingsplan.API.Tests.Controllers
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
        public void Post_ManagerCalled()
        {
            // Arrange
            RestEducationPlan restEducationPlan = GetDummyRestEducationPlan(new string[] { "2NETARCH", "ADCSB" });

            var educationPlanManagerMock = new Mock<IEducationPlanManager>(MockBehavior.Strict);
            educationPlanManagerMock.Setup(manager => manager.GenerateEducationPlan(restEducationPlan)).Returns(GetDummyEducationPlan());

            EducationPlanController controller = new EducationPlanController(educationPlanManagerMock.Object);

            // Act
            controller.Post(restEducationPlan);


            // Assert
            educationPlanManagerMock.Verify(manager => manager.GenerateEducationPlan(restEducationPlan));

        }
    }
}
