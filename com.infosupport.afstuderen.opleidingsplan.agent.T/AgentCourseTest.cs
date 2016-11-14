using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace com.infosupport.afstuderen.opleidingsplan.agent.T
{
    [TestClass]
    public class AgentCourseTest
    {
        [TestMethod]
        public void FindAllCoursesWithFileStreamTest()
        {
            // Arrange
            FileStream stream = new FileStream(@"..\..\courselist.xml", FileMode.Open);
            AgentCourse agent = new AgentCourse(stream);

            // Act
            var result = agent.FindAllCourses();

            // Assert
            Assert.AreEqual(422, result.Coursesummary.Count);
        }

        [TestMethod]
        public void FindAllCoursesCallRealServiceTest()
        {
            // Arrange
            AgentCourse agent = new AgentCourse();

            // Act
            var result = agent.FindAllCourses();

            // Assert
            Assert.AreEqual(422, result.Coursesummary.Count);
        }

        [TestMethod]
        public void FindCourseADCSBTest()
        {
            // Arrange
            AgentCourse agent = new AgentCourse();

            // Act
            var result = agent.FindCourse("ADCSB");

            // Assert
            Assert.AreEqual("Advanced C#", result.Name);
        }


        [TestMethod]
        [Ignore]
        public void test()
        {
            // Arrange
            AgentCourse agent = new AgentCourse();

            // Act
            foreach (var item in agent.FindAllCourses().Coursesummary)
            {
                if (item.Code != "EJB3.1")
                {
                    var course = agent.FindCourse(item.Code);
                }
            }

            

            // Assert
        }
    }
}
