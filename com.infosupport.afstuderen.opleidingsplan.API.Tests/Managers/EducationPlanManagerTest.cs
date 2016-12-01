using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.infosupport.afstuderen.opleidingsplan.api;
using com.infosupport.afstuderen.opleidingsplan.api.Managers;
using com.infosupport.afstuderen.opleidingsplan.integration;
using Moq;
using com.infosupport.afstuderen.opleidingsplan.api.Models;
using System.Collections.Generic;
using com.infosupport.afstuderen.opleidingsplan.generator;
using com.infosupport.afstuderen.opleidingsplan.API.tests.helpers;
using com.infosupport.afstuderen.opleidingsplan.DAL.mapper;
using com.infosupport.afstuderen.opleidingsplan.model;
using System.Linq;

namespace com.infosupport.afstuderen.opleidingsplan.API.tests.managers
{
    [TestClass]
    public class EducationPlanManagerTest : EducationPlanTestHelper
    {
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            AutoMapperConfiguration.Configure();
        }


        [TestMethod]
        public void GenerateEducationPlan_Planner_Outputter_Service_DAL_Called()
        {
            // Arrange
            string[] courses = new string[] { "2NETARCH", "ADCSB" };

            var educationPlanOutputterMock = new Mock<IEducationPlanOutputter>(MockBehavior.Strict);
            educationPlanOutputterMock.Setup(planner => planner.GenerateEducationPlan(It.IsAny<EducationPlanData>())).Returns(GetDummyEducationPlan());

            var plannerMock = new Mock<IPlanner>(MockBehavior.Strict);
            plannerMock.Setup(planner => planner.PlanCourses(It.IsAny<IEnumerable<model.Course>>()));

            var courseServiceMock = new Mock<ICourseService>(MockBehavior.Strict);
            courseServiceMock.Setup(service => service.FindCourses(courses)).Returns(
                new List<integration.Course>() {
                    CreateNewIntegrationCourseWithTwoCourseImplementations("2NETARCH", 1,
                    new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                    new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) })
            });

            var profileDataMapperMock = new Mock<IDataMapper<Profile>>(MockBehavior.Strict);
            profileDataMapperMock.Setup(dataMapper => dataMapper.FindById(1)).Returns(GetDummyDataProfiles().First());

            EducationPlanManager manager = new EducationPlanManager(courseServiceMock.Object, plannerMock.Object, educationPlanOutputterMock.Object, profileDataMapperMock.Object);
            RestEducationPlan educationPlan = GetDummyRestEducationPlan(courses);


            // Act
            var result = manager.GenerateEducationPlan(educationPlan);

            // Assert
            educationPlanOutputterMock.Verify(outputter => outputter.GenerateEducationPlan(It.IsAny<EducationPlanData>()));
            plannerMock.Verify(outputter => outputter.PlanCourses(It.IsAny<IEnumerable<model.Course>>()));
            courseServiceMock.Verify(outputter => outputter.FindCourses(courses));
            profileDataMapperMock.Verify(dataMapper => dataMapper.FindById(1));
        }

    }
}
