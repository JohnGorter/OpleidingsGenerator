using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.infosupport.afstuderen.opleidingsplan.api.Controllers;
using Moq;
using com.infosupport.afstuderen.opleidingsplan.api.Managers;
using System.Linq;
using com.infosupport.afstuderen.opleidingsplan.api;
using com.infosupport.afstuderen.opleidingsplan.integration;
using com.infosupport.afstuderen.opleidingsplan.model;
using System.Collections.Generic;

namespace com.infosupport.afstuderen.opleidingsplan.API.Tests.Controllers
{
    [TestClass]
    public class CourseControllerTest
    {
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            AutoMapperConfiguration.Configure();
        }

        [TestMethod]
        public void GetCoursePOLDEVELTest()
        {
            // Arrange
            var courseManagerMock = new Mock<ICourseManager>(MockBehavior.Strict);
            courseManagerMock.Setup(manager => manager.FindCourse("POLDEVEL")).Returns(DummyData.GetIntegrationCourse());

            CourseController controller = new CourseController(courseManagerMock.Object);

            // Act
            var result = controller.Get("POLDEVEL");

            // Assert
            courseManagerMock.Verify(manager => manager.FindCourse("POLDEVEL"));
            TestCourseWithDummyData(DummyData.GetIntegrationCourse(), result);
        }


        [TestMethod]
        public void GetAllCoursesTest()
        {
            // Arrange
            var courseManagerMock = new Mock<ICourseManager>(MockBehavior.Strict);
            courseManagerMock.Setup(manager => manager.FindCourses()).Returns(DummyData.GetIntegrationCourses());

            CourseController controller = new CourseController(courseManagerMock.Object);

            // Act
            var result = controller.Get();

            // Assert
            courseManagerMock.Verify(manager => manager.FindCourses());
            TestCoursesWithDummyData(DummyData.GetIntegrationCourses(), result);
        }

        private void TestCoursesWithDummyData(Coursesummarycollection expected, IEnumerable<CourseSummary> actual)
        {
            for (int i = 0; i < expected.Coursesummary.Count; i++)
            {
                Assert.AreEqual(expected.Coursesummary[i].Code, actual.ToArray()[i].Code);
                Assert.AreEqual(expected.Coursesummary[i].Name, actual.ToArray()[i].Name);
                Assert.AreEqual(expected.Coursesummary[i].Suppliername, actual.ToArray()[i].Suppliername);
            }
        }

        private void TestCourseWithDummyData(integration.Course expected, model.Course actual)
        {
            Assert.AreEqual(expected.Code, actual.Code);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.Duration, actual.Duration);
            Assert.AreEqual(expected.Prerequisites, actual.Prerequisites);
            Assert.AreEqual(expected.ShortDescription, actual.ShortDescription);
            Assert.AreEqual(expected.SupplierName, actual.SupplierName);

            for (int i = 0; i < expected.CourseImplementations.Length; i++)
            {
                Assert.AreEqual(expected.CourseImplementations[i].Location, actual.CourseImplementations.ToArray()[i].Location);
                for (int a = 0; a < expected.CourseImplementations[i].Days.Length; a++)
                {
                    Assert.AreEqual(expected.CourseImplementations[i].Days[a], actual.CourseImplementations.ToArray()[i].Days.ToArray()[a]);
                }
            }

        }
    }
}
