using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace com.infosupport.afstuderen.opleidingsplan.generator.Tests
{
    [TestClass]
    public class PlannerWithTwoCourseImplementationTests
    {
        [TestMethod]
        public void PlanThreeCourses_NoOverlap_CreateNewCourseWithTwoCourseImplementations()
        {
            Planner planner = new Planner();

            IEnumerable<model.Course> coursesToPlan = new List<model.Course>()
            {
                CreateNewCourseWithTwoCourseImplementations("SCRUMES", 1, 
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }, 
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }),
                CreateNewCourseWithTwoCourseImplementations("ENEST", 1, 
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10)}, 
                new DateTime[] { new DateTime(2017, 4, 10), new DateTime(2017, 4, 11)}),
                CreateNewCourseWithTwoCourseImplementations("ENDEVN", 1, 
                new DateTime[] { new DateTime(2017, 1, 16), new DateTime(2017, 1, 17), new DateTime(2017, 1, 18) }, 
                new DateTime[] { new DateTime(2017, 3, 13), new DateTime(2017, 3, 14), new DateTime(2017, 3, 15) }),
            };

            planner.PlanCourses(coursesToPlan);

            Assert.AreEqual(3, planner.GetPlannedCourses().Count());
            Assert.AreEqual(new DateTime(2017, 1, 2), planner.GetPlannedCourses().ElementAt(0).PlannedCourseImplementation.StartDay);
            Assert.AreEqual(new DateTime(2017, 1, 9), planner.GetPlannedCourses().ElementAt(1).PlannedCourseImplementation.StartDay);
            Assert.AreEqual(new DateTime(2017, 1, 16), planner.GetPlannedCourses().ElementAt(2).PlannedCourseImplementation.StartDay);

            Assert.AreEqual(0, planner.GetNotPlannedCourses().Count());
        }

        //[TestMethod]
        //public void PlanThreeCourses_NoOverlap_CreateNewCourseWithTwoCourseImplementations11()
        //{
        //    Planner planner = new Planner();

        //    IEnumerable<CoursePriority> coursesToPlan = new List<CoursePriority>()
        //    {
        //        CreateNewCourseWithTwoCourseImplementations("SCRUMES", 1,
        //        new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
        //        new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }),
        //        CreateNewCourseWithTwoCourseImplementations("ENEST", 1,
        //        new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10)},
        //        new DateTime[] { new DateTime(2017, 4, 10), new DateTime(2017, 4, 11)}),
        //        CreateNewCourseWithTwoCourseImplementations("ENDEVN", 1,
        //        new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
        //        new DateTime[] { new DateTime(2017, 3, 13), new DateTime(2017, 3, 14), new DateTime(2017, 3, 15) }),
        //    };

        //    planner.PlanCourses(coursesToPlan);

        //    Assert.AreEqual(3, planner.CoursesToFollow.Count);
        //    Assert.AreEqual(new DateTime(2017, 1, 2), planner.CoursesToFollow[0].CourseImplementation.StartDay);
        //    Assert.AreEqual(new DateTime(2017, 1, 9), planner.CoursesToFollow[1].CourseImplementation.StartDay);
        //    Assert.AreEqual(new DateTime(2017, 3, 13), planner.CoursesToFollow[2].CourseImplementation.StartDay);

        //    Assert.AreEqual(0, planner.CoursesNotPlanned.Count);
        //}

        private static model.Course CreateNewCourseWithTwoCourseImplementations(string courseId, int priority, DateTime[] days1, DateTime[] days2)
        {
            return new model.Course
            {
                Code = courseId,
                Priority = priority,
                CourseImplementations = new List<model.CourseImplementation>()
                    {
                        new model.CourseImplementation
                        {
                            Days = days1.ToList(),
                            StartDay =  days1.First(),
                        },
                        new model.CourseImplementation
                        {
                            Days = days2.ToList(),
                            StartDay =  days2.First(),
                        }
                    }
            };
        }
    }
}
