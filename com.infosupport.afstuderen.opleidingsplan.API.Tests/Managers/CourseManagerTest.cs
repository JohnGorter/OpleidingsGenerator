using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.infosupport.afstuderen.opleidingsplan.integration;
using Moq;
using com.infosupport.afstuderen.opleidingsplan.api.managers;
using System.Linq;
using com.infosupport.afstuderen.opleidingsplan.api.tests.helpers;
using com.infosupport.afstuderen.opleidingsplan.dal.mappers;
using com.infosupport.afstuderen.opleidingsplan.models;

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

        [TestMethod]
        public void Insert_NewCoursePriority()
        {
            // Arrange
            var courseDataMapperMock = new Mock<ICourseDataMapper>(MockBehavior.Strict);
            courseDataMapperMock.Setup(dataMapper => dataMapper.Insert(It.IsAny<CoursePriority>()));

            CourseManager manager = new CourseManager(courseDataMapperMock.Object);

            // Act
            manager.Insert(GetDummyCourse());

            // Assert
            courseDataMapperMock.Verify(dataMapper => dataMapper.Insert(It.IsAny<CoursePriority>()));
        }

        [TestMethod]
        public void Update_CoursePriority()
        {
            // Arrange
            var courseDataMapperMock = new Mock<ICourseDataMapper>(MockBehavior.Strict);
            courseDataMapperMock.Setup(dataMapper => dataMapper.Update(It.IsAny<CoursePriority>()));

            CourseManager manager = new CourseManager(courseDataMapperMock.Object);

            // Act
            manager.Update(GetDummyCourse());

            // Assert
            courseDataMapperMock.Verify(dataMapper => dataMapper.Update(It.IsAny<CoursePriority>()));
        }

        [TestMethod]
        public void Delete_NewCoursePriority()
        {
            // Arrange
            var courseDataMapperMock = new Mock<ICourseDataMapper>(MockBehavior.Strict);
            courseDataMapperMock.Setup(dataMapper => dataMapper.Delete(It.IsAny<CoursePriority>()));

            CourseManager manager = new CourseManager(courseDataMapperMock.Object);

            // Act
            manager.Delete(GetDummyCourse());

            // Assert
            courseDataMapperMock.Verify(dataMapper => dataMapper.Delete(It.IsAny<CoursePriority>()));
        }

        private void TestCoursesWithDummyData(Coursesummarycollection expected, IEnumerable<Coursesummary> actual)
        {
            for (int i = 0; i < expected.Coursesummary.Count(); i++)
            {
                Assert.AreEqual(expected.Coursesummary.ElementAt(i).Code, actual.ToArray()[i].Code);
                Assert.AreEqual(expected.Coursesummary.ElementAt(i).Name, actual.ToArray()[i].Name);
                Assert.AreEqual(expected.Coursesummary.ElementAt(i).Suppliername, actual.ToArray()[i].Suppliername);
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

            for (int i = 0; i < expected.CourseImplementations.Count; i++)
            {
                Assert.AreEqual(expected.CourseImplementations[i].Location, actual.CourseImplementations[i].Location);
                for (int a = 0; a < expected.CourseImplementations[i].Days.Count; a++)
                {
                    Assert.AreEqual(expected.CourseImplementations[i].Days[a], actual.CourseImplementations[i].Days[a]);
                }
            }

        }
    }
}
