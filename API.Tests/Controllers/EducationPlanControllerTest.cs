using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InfoSupport.KC.OpleidingsplanGenerator.Api;
using InfoSupport.KC.OpleidingsplanGenerator.Api.Controllers;
using InfoSupport.KC.OpleidingsplanGenerator.Api.Managers;
using Moq;
using InfoSupport.KC.OpleidingsplanGenerator.Api.Models;
using InfoSupport.KC.OpleidingsplanGenerator.Api.Tests.Helpers;
using System.Collections.ObjectModel;
using InfoSupport.KC.OpleidingsplanGenerator.Models;
using System.Collections.Generic;

namespace InfoSupport.KC.OpleidingsplanGenerator.Api.Tests.Controllers
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
            var courses = new Collection<RestEducationPlanCourse> {
                new RestEducationPlanCourse
                {
                    Code = "2NETARCH"
                },
                new RestEducationPlanCourse
                {
                    Code = "ADCSB"
                },
            };

            RestEducationPlan restEducationPlan = GetDummyRestEducationPlan(courses);

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
            var courses = new Collection<RestEducationPlanCourse> {
                new RestEducationPlanCourse
                {
                    Code = "2NETARCH"
                },
                new RestEducationPlanCourse
                {
                    Code = "ADCSB"
                },
            };
            RestEducationPlan restEducationPlan = GetDummyRestEducationPlan(courses);

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
            var courses = new Collection<RestEducationPlanCourse> {
                new RestEducationPlanCourse
                {
                    Code = "2NETARCH"
                },
                new RestEducationPlanCourse
                {
                    Code = "ADCSB"
                },
            };
            RestEducationPlan restEducationPlan = GetDummyRestEducationPlan(courses);

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
            var courses = new Collection<RestEducationPlanCourse> {
                new RestEducationPlanCourse
                {
                    Code = "2NETARCH"
                },
                new RestEducationPlanCourse
                {
                    Code = "ADCSB"
                },
            };
            RestEducationPlan restEducationPlan = GetDummyRestEducationPlan(courses);

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

        [TestMethod]
        public void Delete_ManagerCalled()
        {
            // Arrange
            var courses = new Collection<RestEducationPlanCourse> {
                new RestEducationPlanCourse
                {
                    Code = "2NETARCH"
                },
                new RestEducationPlanCourse
                {
                    Code = "ADCSB"
                },
            };
            RestEducationPlan restEducationPlan = GetDummyRestEducationPlan(courses);

            var educationPlanManagerMock = new Mock<IEducationPlanManager>(MockBehavior.Strict);
            educationPlanManagerMock.Setup(manager => manager.DeleteEducationPlan(1));

            EducationPlanController controller = new EducationPlanController(educationPlanManagerMock.Object);

            // Act
            controller.Delete(1);

            // Assert
            educationPlanManagerMock.Verify(manager => manager.DeleteEducationPlan(1));
        }
    }
}
