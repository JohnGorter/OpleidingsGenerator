using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.infosupport.afstuderen.opleidingsplan.integration;
using Moq;
using com.infosupport.afstuderen.opleidingsplan.api.managers;
using System.Linq;
using com.infosupport.afstuderen.opleidingsplan.api.tests.helpers;

namespace com.infosupport.afstuderen.opleidingsplan.api.tests.managers
{
    /// <summary>
    /// Summary description for CourseManagerTest
    /// </summary>
    [TestClass]
    public class CourseManagerTest : CourseTestHelper
    {


        [TestMethod]
        public void FindCourseTest()
        {
            // Arrange
            var courseServiceMock = new Mock<ICourseService>(MockBehavior.Strict);
            courseServiceMock.Setup(service => service.FindCourse("POLDEVEL")).Returns(GetDummyDataIntegrationCourse());

            CourseManager manager = new CourseManager(courseServiceMock.Object);

            // Act
            var result = manager.FindCourse("POLDEVEL");

            // Assert
            courseServiceMock.Verify(service => service.FindCourse("POLDEVEL"));
            TestCourseWithDummyData(GetDummyDataIntegrationCourse(), result);
        }

        [TestMethod]
        public void FindAllCoursesTest()
        {
            // Arrange
            var courseServiceMock = new Mock<ICourseService>(MockBehavior.Strict);
            courseServiceMock.Setup(service => service.FindAllCourses()).Returns(GetDummyDataIntegrationCourses());

            CourseManager manager = new CourseManager(courseServiceMock.Object);

            // Act
            var result = manager.FindCourses();

            // Assert
            courseServiceMock.Verify(service => service.FindAllCourses());
            TestCoursesWithDummyData(GetDummyDataIntegrationCourses(), result.Coursesummary);
        }

        private void TestCoursesWithDummyData(Coursesummarycollection expected, IEnumerable<Coursesummary> actual)
        {
            for (int i = 0; i < expected.Coursesummary.Count; i++)
            {
                Assert.AreEqual(expected.Coursesummary[i].Code, actual.ToArray()[i].Code);
                Assert.AreEqual(expected.Coursesummary[i].Name, actual.ToArray()[i].Name);
                Assert.AreEqual(expected.Coursesummary[i].Suppliername, actual.ToArray()[i].Suppliername);
            }
        }

        private void TestCourseWithDummyData(integration.Course expected, integration.Course actual)
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
                Assert.AreEqual(expected.CourseImplementations[i].Location, actual.CourseImplementations[i].Location);
                for (int a = 0; a < expected.CourseImplementations[i].Days.Length; a++)
                {
                    Assert.AreEqual(expected.CourseImplementations[i].Days[a], actual.CourseImplementations[i].Days[a]);
                }
            }

        }
    }
}
