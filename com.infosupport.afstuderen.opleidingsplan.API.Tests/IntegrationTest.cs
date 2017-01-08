using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.infosupport.afstuderen.opleidingsplan.api.controllers;
using com.infosupport.afstuderen.opleidingsplan.api.models;
using com.infosupport.afstuderen.opleidingsplan.api.tests.helpers;
using com.infosupport.afstuderen.opleidingsplan.api.managers;
using com.infosupport.afstuderen.opleidingsplan.integration;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace com.infosupport.afstuderen.opleidingsplan.api.tests
{
    [TestClass]
    public class IntegrationTest : EducationPlanTestHelper
    {
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            AutoMapperConfiguration.Configure();
        }

        [TestMethod]
        public void GenerateEducationPlan_IntegrationTest()
        {
            // Arrange
            var courses = new string[] { "2NETARCH" };

            RestEducationPlan restEducationPlan = GetDummyRestEducationPlan(courses);
            var courseServiceMock = new Mock<ICourseService>(MockBehavior.Strict);
            courseServiceMock.Setup(service => service.FindCourses(courses)).Returns(
                new List<integration.Course>() {
                    CreateNewIntegrationCourseWithTwoCourseImplementations("2NETARCH", 1,
                    new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                    new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) })
            });

            IEducationPlanManager manager = new EducationPlanManager("../../Data/Profiles.json", courseServiceMock.Object, "../../Data/ManagementProperties.json");

            // Act
            var result = manager.GenerateEducationPlan(restEducationPlan);


            // Assert
            courseServiceMock.Verify(service => service.FindCourses(courses));

            Assert.AreEqual(1, result.PlannedCourses.Count());
            Assert.AreEqual(0, result.NotPlannedCourses.Count());
            Assert.AreEqual("2NETARCH", result.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(new DateTime(2017, 1, 2), result.PlannedCourses.ElementAt(0).Date);
        }
    }
}
