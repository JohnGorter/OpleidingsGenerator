﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InfoSupport.KC.OpleidingsplanGenerator.Api.Controllers;
using Moq;
using InfoSupport.KC.OpleidingsplanGenerator.Api.Managers;
using System.Linq;
using InfoSupport.KC.OpleidingsplanGenerator.Api;
using InfoSupport.KC.OpleidingsplanGenerator.Integration;
using InfoSupport.KC.OpleidingsplanGenerator.Models;
using System.Collections.Generic;
using InfoSupport.KC.OpleidingsplanGenerator.Api.Tests.Helpers;

namespace InfoSupport.KC.OpleidingsplanGenerator.Api.Tests.Controllers
{
    [TestClass]
    public class CourseControllerTest : CourseTestHelper
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
            courseManagerMock.Setup(manager => manager.FindCourse("POLDEVEL")).Returns(GetDummyDataIntegrationCourse());

            CourseController controller = new CourseController(courseManagerMock.Object);

            // Act
            var result = controller.Get("POLDEVEL");

            // Assert
            courseManagerMock.Verify(manager => manager.FindCourse("POLDEVEL"));
            TestCourseWithDummyData(GetDummyDataIntegrationCourse(), result);
        }


        [TestMethod]
        public void GetAllCoursesTest()
        {
            // Arrange
            var courseManagerMock = new Mock<ICourseManager>(MockBehavior.Strict);
            courseManagerMock.Setup(manager => manager.FindCourses()).Returns(GetDummyDataIntegrationCourses());

            CourseController controller = new CourseController(courseManagerMock.Object);

            // Act
            var result = controller.Get();

            // Assert
            courseManagerMock.Verify(manager => manager.FindCourses());
            TestCoursesWithDummyData(GetDummyDataIntegrationCourses(), result);
        }

       
        [TestMethod]
        public void Post_CoursePriority()
        {
            // Arrange
            var courseManagerMock = new Mock<ICourseManager>(MockBehavior.Strict);
            courseManagerMock.Setup(manager => manager.Update(It.IsAny<CoursePriority>()));

            CourseController controller = new CourseController(courseManagerMock.Object);

            // Act
            controller.Post(GetDummyCourse());

            // Assert
            courseManagerMock.Verify(manager => manager.Update(It.IsAny<CoursePriority>()));
        }

        [TestMethod]
        public void Put_CoursePriority()
        {
            // Arrange
            var courseManagerMock = new Mock<ICourseManager>(MockBehavior.Strict);
            courseManagerMock.Setup(manager => manager.Insert(It.IsAny<CoursePriority>()));

            CourseController controller = new CourseController(courseManagerMock.Object);

            // Act
            controller.Put(GetDummyCourse());

            // Assert
            courseManagerMock.Verify(manager => manager.Insert(It.IsAny<CoursePriority>()));
        }

        [TestMethod]
        public void Delete_CoursePriority()
        {
            // Arrange
            var courseManagerMock = new Mock<ICourseManager>(MockBehavior.Strict);
            courseManagerMock.Setup(manager => manager.Delete(It.IsAny<CoursePriority>()));

            CourseController controller = new CourseController(courseManagerMock.Object);

            // Act
            controller.Delete(GetDummyCourse());

            // Assert
            courseManagerMock.Verify(manager => manager.Delete(It.IsAny<CoursePriority>()));
        }

        private void TestCoursesWithDummyData(Coursesummarycollection expected, IEnumerable<CourseSummary> actual)
        {
            for (int i = 0; i < expected.Coursesummary.Count(); i++)
            {
                Assert.AreEqual(expected.Coursesummary.ElementAt(i).Code, actual.ToArray()[i].Code);
                Assert.AreEqual(expected.Coursesummary.ElementAt(i).Name, actual.ToArray()[i].Name);
                Assert.AreEqual(expected.Coursesummary.ElementAt(i).Suppliername, actual.ToArray()[i].Suppliername);
            }
        }

        private void TestCourseWithDummyData(Integration.Course expected, OpleidingsplanGenerator.Models.Course actual)
        {
            Assert.AreEqual(expected.Code, actual.Code);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.Duration, actual.Duration);
            Assert.AreEqual(expected.ShortDescription, actual.ShortDescription);
            Assert.AreEqual(expected.SupplierName, actual.SupplierName);

            for (int i = 0; i < expected.CourseImplementations.Count; i++)
            {
                Assert.AreEqual(expected.CourseImplementations[i].Location, actual.CourseImplementations.ToArray()[i].Location);
                for (int a = 0; a < expected.CourseImplementations[i].Days.Count; a++)
                {
                    Assert.AreEqual(expected.CourseImplementations[i].Days[a], actual.CourseImplementations.ToArray()[i].Days.ToArray()[a]);
                }
            }

        }

    }
}
