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
            FileStream stream = new FileStream(@"..\..\traininglist.xml", FileMode.Open);
            AgentCourse agent = new AgentCourse(stream);

            // Act
            var result = agent.FindAllCourses();

            // Assert
            Assert.AreEqual(421, result.Coursesummary.Count);
        }

        [TestMethod]
        public void FindAllCoursesCallRealServiceTest()
        {
            // Arrange
            AgentCourse agent = new AgentCourse();

            // Act
            var result = agent.FindAllCourses();

            // Assert
            Assert.AreEqual(421, result.Coursesummary.Count);
        }
    }
}
