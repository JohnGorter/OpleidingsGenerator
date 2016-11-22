using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace com.infosupport.afstuderen.opleidingsplan.generator.Tests
{
    [TestClass]
    public class PlannerWithMixCourseImplementationTests : CourseTestHelper
    {
        [Ignore]
        [TestMethod]
        public void PlanThreeCourses_OverlapTwoImplementation_MoveSecondCourseImplementation_ThreeCoursesPlanned1()
        {
            //Arrange
            Planner planner = new Planner();

            IEnumerable<model.Course> coursesToPlan = new List<model.Course>()
            {
                CreateNewModelCourseWithTwoCourseImplementations("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }),
                CreateNewModelCourseWithTwoCourseImplementations("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3)},
                new DateTime[] { new DateTime(2017, 2, 13), new DateTime(2017, 2, 14)}),
                CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 2, 14), new DateTime(2017, 2, 15), new DateTime(2017, 2, 16)}),
               
            };

            //Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(3, planner.GetPlannedCourses().Count());
            Assert.AreEqual(new DateTime(2017, 3, 6), planner.GetPlannedCourses().ElementAt(0).PlannedCourseImplementation.StartDay);
            Assert.AreEqual("SCRUMES", planner.GetPlannedCourses().ElementAt(0).Code);
            Assert.AreEqual(new DateTime(2017, 1, 2), planner.GetPlannedCourses().ElementAt(1).PlannedCourseImplementation.StartDay);
            Assert.AreEqual("ENEST", planner.GetPlannedCourses().ElementAt(1).Code);
            Assert.AreEqual(new DateTime(2017, 2, 14), planner.GetPlannedCourses().ElementAt(2).PlannedCourseImplementation.StartDay);
            Assert.AreEqual("ENDEVN", planner.GetPlannedCourses().ElementAt(2).Code);

            Assert.AreEqual(0, planner.GetNotPlannedCourses().Count());
        }
    }
}
