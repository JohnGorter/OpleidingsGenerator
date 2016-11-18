using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using com.infosupport.afstuderen.opleidingsplan.model;

namespace com.infosupport.afstuderen.opleidingsplan.generator.Tests
{
    [TestClass]
    public class PlannerWithOneCourseImplementationTests
    {
        [TestMethod]
        public void PlanThreeCourses_NoOverlap_CourseWithOneCourseImplementation()
        {
            Planner planner = new Planner();

            IEnumerable<model.Course> coursesToPlan = new List<model.Course>()
            {
                CreateNewCourseWithOneCourseImplementation("SCRUMES", 1, new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewCourseWithOneCourseImplementation("ENEST", 1, new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }),
                CreateNewCourseWithOneCourseImplementation("ENDEVN", 1, new DateTime[] { new DateTime(2017, 1, 16), new DateTime(2017, 1, 17), new DateTime(2017, 1, 18) }),
            };

            planner.PlanCourses(coursesToPlan);

            Assert.AreEqual(3, planner.GetPlannedCourses().Count());
            Assert.AreEqual(new DateTime(2017, 1, 2), planner.GetPlannedCourses().ElementAt(0).PlannedCourseImplementation.StartDay);
            Assert.AreEqual(new DateTime(2017, 1, 9), planner.GetPlannedCourses().ElementAt(1).PlannedCourseImplementation.StartDay);
            Assert.AreEqual(new DateTime(2017, 1, 16), planner.GetPlannedCourses().ElementAt(2).PlannedCourseImplementation.StartDay);
            Assert.AreEqual(0, planner.GetNotPlannedCourses().Count());
        }

        [TestMethod]
        public void PlanThreeCourses_OneWithFullOverlap_TwoCoursesPlanned__CourseWithOneCourseImplementation()
        {
            Planner planner = new Planner();

            IEnumerable<model.Course> coursesToPlan = new List<model.Course>()
            {
                CreateNewCourseWithOneCourseImplementation("SCRUMES", 1, new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewCourseWithOneCourseImplementation("ENEST", 1, new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }),
                CreateNewCourseWithOneCourseImplementation("ENDEVN", 1, new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }),
            };

            planner.PlanCourses(coursesToPlan);

            Assert.AreEqual(2, planner.GetPlannedCourses().Count());
            Assert.AreEqual("SCRUMES", planner.GetPlannedCourses().First().Code);
            Assert.AreEqual("ENEST", planner.GetPlannedCourses().Last().Code);
            Assert.AreEqual(1, planner.GetNotPlannedCourses().Count());
            Assert.AreEqual("ENDEVN", planner.GetNotPlannedCourses().First().Code);
            Assert.IsNull(planner.GetNotPlannedCourses().First().PlannedCourseImplementation);
        }

        [TestMethod]
        public void PlanThreeCourses_OneWithOverlapOneDay_TwoCoursesPlanned_CourseWithOneCourseImplementation()
        {
            Planner planner = new Planner();

            IEnumerable<model.Course> coursesToPlan = new List<model.Course>()
            {
                CreateNewCourseWithOneCourseImplementation("SCRUMES", 1, new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewCourseWithOneCourseImplementation("ENEST", 1, new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5), new DateTime(2017, 1, 6) }),
                CreateNewCourseWithOneCourseImplementation("ENDEVN", 1, new DateTime[] { new DateTime(2017, 1, 16), new DateTime(2017, 1, 17), new DateTime(2017, 1, 18) }),
            };

            planner.PlanCourses(coursesToPlan);

            Assert.AreEqual(2, planner.GetPlannedCourses().Count());
            Assert.AreEqual("SCRUMES", planner.GetPlannedCourses().First().Code);
            Assert.AreEqual("ENDEVN", planner.GetPlannedCourses().Last().Code);
            Assert.AreEqual(1, planner.GetNotPlannedCourses().Count());
            Assert.AreEqual("ENEST", planner.GetNotPlannedCourses().First().Code);
        }

        [TestMethod]
        public void PlanThreeCourses_OneWithOverlapOneDayAndPriority_TwoCoursesPlanned_CourseWithOneCourseImplementation()
        {
            Planner planner = new Planner();

            IEnumerable<model.Course> coursesToPlan = new List<model.Course>()
            {
                CreateNewCourseWithOneCourseImplementation("SCRUMES", 2, new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewCourseWithOneCourseImplementation("ENEST", 1, new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5), new DateTime(2017, 1, 6) }),
                CreateNewCourseWithOneCourseImplementation("ENDEVN", 1, new DateTime[] { new DateTime(2017, 1, 16), new DateTime(2017, 1, 17), new DateTime(2017, 1, 18) }),
            };

            planner.PlanCourses(coursesToPlan);

            Assert.AreEqual(2, planner.GetPlannedCourses().Count());
            Assert.AreEqual("ENEST", planner.GetPlannedCourses().First().Code);
            Assert.AreEqual("ENDEVN", planner.GetPlannedCourses().Last().Code);

            Assert.AreEqual(1, planner.GetNotPlannedCourses().Count());
            Assert.AreEqual("SCRUMES", planner.GetNotPlannedCourses().First().Code);
        }

        [TestMethod]
        public void PlanThreeCourses_TwoWithOverlapOneDayAndTwoPriority_OneCoursePlanned_CourseWithOneCourseImplementation()
        {
            Planner planner = new Planner();

            IEnumerable<model.Course> coursesToPlan = new List<model.Course>()
            {
                CreateNewCourseWithOneCourseImplementation("SCRUMES", 2, new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewCourseWithOneCourseImplementation("ENEST", 1, new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5), new DateTime(2017, 1, 6) }),
                CreateNewCourseWithOneCourseImplementation("ENDEVN", 1, new DateTime[] { new DateTime(2017, 1, 6) }),
            };

            planner.PlanCourses(coursesToPlan);

            Assert.AreEqual(1, planner.GetPlannedCourses().Count());
            Assert.AreEqual("ENEST", planner.GetPlannedCourses().First().Code);
            Assert.AreEqual(2, planner.GetNotPlannedCourses().Count());
            Assert.AreEqual("ENDEVN", planner.GetNotPlannedCourses().First().Code);
            Assert.AreEqual("SCRUMES", planner.GetNotPlannedCourses().Last().Code);
        }

        [TestMethod]
        public void PlanThreeCourses_TwoWithOverlapOneDayAndThreePriorities_TwoCoursesPlanned_CourseWithOneCourseImplementation()
        {
            Planner planner = new Planner();

            IEnumerable<model.Course> coursesToPlan = new List<model.Course>()
            {
                CreateNewCourseWithOneCourseImplementation("SCRUMES", 3, new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewCourseWithOneCourseImplementation("ENEST", 2, new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5), new DateTime(2017, 1, 6) }),
                CreateNewCourseWithOneCourseImplementation("ENDEVN", 1, new DateTime[] { new DateTime(2017, 1, 6) }),
            };

            planner.PlanCourses(coursesToPlan);

            Assert.AreEqual(2, planner.GetPlannedCourses().Count());
            Assert.AreEqual("ENDEVN", planner.GetPlannedCourses().First().Code);
            Assert.AreEqual("SCRUMES", planner.GetPlannedCourses().Last().Code);
            Assert.AreEqual(1, planner.GetNotPlannedCourses().Count());
            Assert.AreEqual("ENEST", planner.GetNotPlannedCourses().First().Code);
        }


        [TestMethod]
        public void PlanTwoCourses_WithOverlapOneDay_OneCoursesPlanned_CourseWithOneCourseImplementation_TestOverlapCourses()
        {
            Planner planner = new Planner();

            IEnumerable<model.Course> coursesToPlan = new List<model.Course>()
            {
                CreateNewCourseWithOneCourseImplementation("SCRUMES", 1, new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewCourseWithOneCourseImplementation("ENEST", 1, new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5), new DateTime(2017, 1, 6) }),
            };

            planner.PlanCourses(coursesToPlan);

            Assert.AreEqual(1, planner.GetPlannedCourses().Count());
            Assert.AreEqual("SCRUMES", planner.GetPlannedCourses().First().Code);
            Assert.AreEqual(1, planner.GetNotPlannedCourses().Count());
            Assert.AreEqual("ENEST", planner.GetNotPlannedCourses().First().Code);

            Assert.AreEqual(1, planner.GetNotPlannedCourses().First().IntersectedCourseIds.Count());
            Assert.AreEqual("SCRUMES", planner.GetNotPlannedCourses().First().IntersectedCourseIds.First());
        }

        [TestMethod]
        public void PlanSixCourses_FourWithOverlap_FourCoursesPlanned_CourseWithOneCourseImplementation_TestOverlapCourses()
        {
            Planner planner = new Planner();

            IEnumerable<model.Course> coursesToPlan = new List<model.Course>()
            {
                CreateNewCourseWithOneCourseImplementation("SCRUMES", 1, new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewCourseWithOneCourseImplementation("ENEST", 1, new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5), new DateTime(2017, 1, 6) }),
                CreateNewCourseWithOneCourseImplementation("ENDEVN", 1, new DateTime[] { new DateTime(2017, 1, 6) }),
                CreateNewCourseWithOneCourseImplementation("SECDEV", 1, new DateTime[] { new DateTime(2017, 1, 16), new DateTime(2017, 1, 17) }),
                CreateNewCourseWithOneCourseImplementation("XSD", 1, new DateTime[] { new DateTime(2017, 1, 18) }),
                CreateNewCourseWithOneCourseImplementation("MVC", 1, new DateTime[] { new DateTime(2017, 1, 17), new DateTime(2017, 1, 18) }),
            };

            planner.PlanCourses(coursesToPlan);

            Assert.AreEqual(4, planner.GetPlannedCourses().Count());
            Assert.AreEqual("SCRUMES", planner.GetPlannedCourses().First().Code);
            Assert.AreEqual("ENDEVN", planner.GetPlannedCourses().ElementAt(1).Code);
            Assert.AreEqual("SECDEV", planner.GetPlannedCourses().ElementAt(2).Code);
            Assert.AreEqual("XSD", planner.GetPlannedCourses().ElementAt(3).Code);
            Assert.AreEqual(2, planner.GetNotPlannedCourses().Count());
            Assert.AreEqual("ENEST", planner.GetNotPlannedCourses().First().Code);
            Assert.AreEqual("MVC", planner.GetNotPlannedCourses().Last().Code);

            Assert.AreEqual(2, planner.GetNotPlannedCourses().First().IntersectedCourseIds.Count());
            Assert.AreEqual("SCRUMES", planner.GetNotPlannedCourses().First().IntersectedCourseIds.First());
            Assert.AreEqual("ENDEVN", planner.GetNotPlannedCourses().First().IntersectedCourseIds.Last());
            Assert.AreEqual(2, planner.GetNotPlannedCourses().Last().IntersectedCourseIds.Count());
            Assert.AreEqual("SECDEV", planner.GetNotPlannedCourses().Last().IntersectedCourseIds.First());
            Assert.AreEqual("XSD", planner.GetNotPlannedCourses().Last().IntersectedCourseIds.Last());
        }



        private static model.Course CreateNewCourseWithOneCourseImplementation(string Code, int priority, DateTime[] days)
        {
            return new model.Course
            {
                Code = Code,
                Priority = priority,
                CourseImplementations = new List<model.CourseImplementation>()
                    {
                        new model.CourseImplementation
                        {
                            Days = days.ToList(),
                            StartDay =  days.First(),
                        }
                    }
            };
        }






    }
}
