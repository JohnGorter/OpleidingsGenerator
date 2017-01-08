//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.IO;
//using System.Linq;

//namespace com.infosupport.afstuderen.opleidingsplan.integration.tests
//{
//    [TestClass]
//    public class IntegrationCourseTest
//    {
//        [Ignore]
//        [TestMethod]
//        public void FindAllCoursesCallRealServiceTest()
//        {
//            // Arrange
//            CourseService agent = new CourseService();

//            // Act
//            var result = agent.FindAllCourses();

//            // Assert
//            Assert.AreEqual(422, result.Coursesummary.Count());
//        }

//        [Ignore]
//        [TestMethod]
//        public void FindCourseADCSBTest()
//        {
//            // Arrange
//            CourseService agent = new CourseService();

//            // Act
//            var result = agent.FindCourse("ADCSB");

//            // Assert
//            Assert.AreEqual("Advanced C#", result.Name);
//        }


//        [TestMethod]
//        [Ignore]
//        public void test()
//        {
//            // Arrange
//            CourseService agent = new CourseService();

//            // Act
//            foreach (var item in agent.FindAllCourses().Coursesummary)
//            {
//                if (item.Code != "EJB3.1")
//                {
//                    var course = agent.FindCourse(item.Code);
//                }
//            }

//            // Assert
//        }
//    }
//}
