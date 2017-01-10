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
using System.Collections.ObjectModel;

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
                    new Collection<DateTime> { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                    new Collection<DateTime> { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) })
            });

            IEducationPlanManager manager = new EducationPlanManager("../../Data/Profiles.json", courseServiceMock.Object, "../../Data/ManagementProperties.json");

            // Act
            var result = manager.GenerateEducationPlan(restEducationPlan);


            // Assert
            courseServiceMock.Verify(service => service.FindCourses(courses));

            Assert.AreEqual(7, result.PlannedCourses.Count());
            Assert.AreEqual(0, result.NotPlannedCourses.Count());

            Assert.AreEqual("OLC", result.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(new DateTime(2016, 12, 5), result.PlannedCourses.ElementAt(0).Date);
            Assert.AreEqual(5, result.PlannedCourses.ElementAt(0).Days);

            Assert.AreEqual("OLC", result.PlannedCourses.ElementAt(1).Code);
            Assert.AreEqual(new DateTime(2016, 12, 12), result.PlannedCourses.ElementAt(1).Date);
            Assert.AreEqual(5, result.PlannedCourses.ElementAt(1).Days);

            Assert.AreEqual("OLC", result.PlannedCourses.ElementAt(2).Code);
            Assert.AreEqual(new DateTime(2016, 12, 19), result.PlannedCourses.ElementAt(2).Date);
            Assert.AreEqual(5, result.PlannedCourses.ElementAt(2).Days);

            Assert.AreEqual("OLC", result.PlannedCourses.ElementAt(3).Code);
            Assert.AreEqual(new DateTime(2016, 12, 26), result.PlannedCourses.ElementAt(3).Date);
            Assert.AreEqual(5, result.PlannedCourses.ElementAt(3).Days);

            Assert.AreEqual("2NETARCH", result.PlannedCourses.ElementAt(4).Code);
            Assert.AreEqual(new DateTime(2017, 1, 2), result.PlannedCourses.ElementAt(4).Date);
            Assert.AreEqual(3, result.PlannedCourses.ElementAt(4).Days);

            Assert.AreEqual("OLC", result.PlannedCourses.ElementAt(5).Code);
            Assert.AreEqual(new DateTime(2017, 1, 5), result.PlannedCourses.ElementAt(5).Date);
            Assert.AreEqual(2, result.PlannedCourses.ElementAt(5).Days);

            Assert.AreEqual("OLC", result.PlannedCourses.ElementAt(6).Code);
            Assert.AreEqual(new DateTime(2017, 1, 9), result.PlannedCourses.ElementAt(6).Date);
            Assert.AreEqual(3, result.PlannedCourses.ElementAt(6).Days);
        }
    }
}
