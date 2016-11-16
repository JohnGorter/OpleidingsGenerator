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

            IEnumerable<CoursePriority> coursesToPlan = new List<CoursePriority>()
            {
                CreateNewCourseWithOneCourseImplementation("SCRUMES", 1, new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewCourseWithOneCourseImplementation("ENEST", 1, new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }),
                CreateNewCourseWithOneCourseImplementation("ENDEVN", 1, new DateTime[] { new DateTime(2017, 1, 16), new DateTime(2017, 1, 17), new DateTime(2017, 1, 18) }),
            };

            planner.PlanCourses(coursesToPlan);

            Assert.AreEqual(3, planner.CoursesToFollow.Count);
            Assert.AreEqual(0, planner.CoursesNotPlanned.Count);
        }

        [TestMethod]
        public void PlanThreeCourses_OneWithFullOverlap_TwoCoursesPlanned__CourseWithOneCourseImplementation()
        {
            Planner planner = new Planner();

            IEnumerable<CoursePriority> coursesToPlan = new List<CoursePriority>()
            {
                CreateNewCourseWithOneCourseImplementation("SCRUMES", 1, new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewCourseWithOneCourseImplementation("ENEST", 1, new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }),
                CreateNewCourseWithOneCourseImplementation("ENDEVN", 1, new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }),
            };

            planner.PlanCourses(coursesToPlan);

            Assert.AreEqual(2, planner.CoursesToFollow.Count);
            Assert.AreEqual("SCRUMES", planner.CoursesToFollow[0].CourseId);
            Assert.AreEqual("ENEST", planner.CoursesToFollow[1].CourseId);
            Assert.AreEqual(1, planner.CoursesNotPlanned.Count);
            Assert.AreEqual("ENDEVN", planner.CoursesNotPlanned[0].CourseId);
        }

        [TestMethod]
        public void PlanThreeCourses_OneWithOverlapOneDay_TwoCoursesPlanned_CourseWithOneCourseImplementation()
        {
            Planner planner = new Planner();

            IEnumerable<CoursePriority> coursesToPlan = new List<CoursePriority>()
            {
                CreateNewCourseWithOneCourseImplementation("SCRUMES", 1, new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewCourseWithOneCourseImplementation("ENEST", 1, new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5), new DateTime(2017, 1, 6) }),
                CreateNewCourseWithOneCourseImplementation("ENDEVN", 1, new DateTime[] { new DateTime(2017, 1, 16), new DateTime(2017, 1, 17), new DateTime(2017, 1, 18) }),
            };

            planner.PlanCourses(coursesToPlan);

            Assert.AreEqual(2, planner.CoursesToFollow.Count);
            Assert.AreEqual("SCRUMES", planner.CoursesToFollow[0].CourseId);
            Assert.AreEqual("ENDEVN", planner.CoursesToFollow[1].CourseId);
            Assert.AreEqual(1, planner.CoursesNotPlanned.Count);
            Assert.AreEqual("ENEST", planner.CoursesNotPlanned[0].CourseId);
        }

        [TestMethod]
        public void PlanThreeCourses_OneWithOverlapOneDayAndPriority_TwoCoursesPlanned_CourseWithOneCourseImplementation()
        {
            Planner planner = new Planner();

            IEnumerable<CoursePriority> coursesToPlan = new List<CoursePriority>()
            {
                CreateNewCourseWithOneCourseImplementation("SCRUMES", 2, new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewCourseWithOneCourseImplementation("ENEST", 1, new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5), new DateTime(2017, 1, 6) }),
                CreateNewCourseWithOneCourseImplementation("ENDEVN", 1, new DateTime[] { new DateTime(2017, 1, 16), new DateTime(2017, 1, 17), new DateTime(2017, 1, 18) }),
            };

            planner.PlanCourses(coursesToPlan);

            Assert.AreEqual(2, planner.CoursesToFollow.Count);
            Assert.AreEqual("ENEST", planner.CoursesToFollow[0].CourseId);
            Assert.AreEqual("ENDEVN", planner.CoursesToFollow[1].CourseId);
            Assert.AreEqual(1, planner.CoursesNotPlanned.Count);
            Assert.AreEqual("SCRUMES", planner.CoursesNotPlanned[0].CourseId);
        }

        [TestMethod]
        public void PlanThreeCourses_TwoWithOverlapOneDayAndTwoPriority_OneCoursePlanned_CourseWithOneCourseImplementation()
        {
            Planner planner = new Planner();

            IEnumerable<CoursePriority> coursesToPlan = new List<CoursePriority>()
            {
                CreateNewCourseWithOneCourseImplementation("SCRUMES", 2, new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewCourseWithOneCourseImplementation("ENEST", 1, new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5), new DateTime(2017, 1, 6) }),
                CreateNewCourseWithOneCourseImplementation("ENDEVN", 1, new DateTime[] { new DateTime(2017, 1, 6) }),
            };

            planner.PlanCourses(coursesToPlan);

            Assert.AreEqual(1, planner.CoursesToFollow.Count);
            Assert.AreEqual("ENEST", planner.CoursesToFollow[0].CourseId);
            Assert.AreEqual(2, planner.CoursesNotPlanned.Count);
            Assert.AreEqual("ENDEVN", planner.CoursesNotPlanned[0].CourseId);
            Assert.AreEqual("SCRUMES", planner.CoursesNotPlanned[1].CourseId);
        }

        [TestMethod]
        public void PlanThreeCourses_TwoWithOverlapOneDayAndThreePriorities_TwoCoursesPlanned_CourseWithOneCourseImplementation()
        {
            Planner planner = new Planner();

            IEnumerable<CoursePriority> coursesToPlan = new List<CoursePriority>()
            {
                CreateNewCourseWithOneCourseImplementation("SCRUMES", 3, new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewCourseWithOneCourseImplementation("ENEST", 2, new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5), new DateTime(2017, 1, 6) }),
                CreateNewCourseWithOneCourseImplementation("ENDEVN", 1, new DateTime[] { new DateTime(2017, 1, 6) }),
            };

            planner.PlanCourses(coursesToPlan);

            Assert.AreEqual(2, planner.CoursesToFollow.Count);
            Assert.AreEqual("ENDEVN", planner.CoursesToFollow[0].CourseId);
            Assert.AreEqual("SCRUMES", planner.CoursesToFollow[1].CourseId);
            Assert.AreEqual(1, planner.CoursesNotPlanned.Count);
            Assert.AreEqual("ENEST", planner.CoursesNotPlanned[0].CourseId);
        }


        [TestMethod]
        public void PlanTwoCourses_WithOverlapOneDay_OneCoursesPlanned_CourseWithOneCourseImplementation_TestOverlapCourses()
        {
            Planner planner = new Planner();

            IEnumerable<CoursePriority> coursesToPlan = new List<CoursePriority>()
            {
                CreateNewCourseWithOneCourseImplementation("SCRUMES", 1, new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewCourseWithOneCourseImplementation("ENEST", 1, new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5), new DateTime(2017, 1, 6) }),
            };

            planner.PlanCourses(coursesToPlan);

            Assert.AreEqual(1, planner.CoursesToFollow.Count);
            Assert.AreEqual("SCRUMES", planner.CoursesToFollow[0].CourseId);
            Assert.AreEqual(1, planner.CoursesNotPlanned.Count);
            Assert.AreEqual("ENEST", planner.CoursesNotPlanned[0].CourseId);

            Assert.AreEqual(1, planner.CoursesNotPlanned[0].CourseIdsOverlap.Count());
            Assert.AreEqual("SCRUMES", planner.CoursesNotPlanned[0].CourseIdsOverlap.First());
        }

        [TestMethod]
        public void PlanSixCourses_FourWithOverlap_FourCoursesPlanned_CourseWithOneCourseImplementation_TestOverlapCourses()
        {
            Planner planner = new Planner();

            IEnumerable<CoursePriority> coursesToPlan = new List<CoursePriority>()
            {
                CreateNewCourseWithOneCourseImplementation("SCRUMES", 1, new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewCourseWithOneCourseImplementation("ENEST", 1, new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5), new DateTime(2017, 1, 6) }),
                CreateNewCourseWithOneCourseImplementation("ENDEVN", 1, new DateTime[] { new DateTime(2017, 1, 6) }),
                CreateNewCourseWithOneCourseImplementation("SECDEV", 1, new DateTime[] { new DateTime(2017, 1, 16), new DateTime(2017, 1, 17) }),
                CreateNewCourseWithOneCourseImplementation("XSD", 1, new DateTime[] { new DateTime(2017, 1, 18) }),
                CreateNewCourseWithOneCourseImplementation("MVC", 1, new DateTime[] { new DateTime(2017, 1, 17), new DateTime(2017, 1, 18) }),
            };

            planner.PlanCourses(coursesToPlan);

            Assert.AreEqual(4, planner.CoursesToFollow.Count);
            Assert.AreEqual("SCRUMES", planner.CoursesToFollow[0].CourseId);
            Assert.AreEqual("ENDEVN", planner.CoursesToFollow[1].CourseId);
            Assert.AreEqual("SECDEV", planner.CoursesToFollow[2].CourseId);
            Assert.AreEqual("XSD", planner.CoursesToFollow[3].CourseId);
            Assert.AreEqual(2, planner.CoursesNotPlanned.Count);
            Assert.AreEqual("ENEST", planner.CoursesNotPlanned[0].CourseId);
            Assert.AreEqual("MVC", planner.CoursesNotPlanned[1].CourseId);

            Assert.AreEqual(2, planner.CoursesNotPlanned[0].CourseIdsOverlap.Count());
            Assert.AreEqual("SCRUMES", planner.CoursesNotPlanned[0].CourseIdsOverlap[0]);
            Assert.AreEqual("ENDEVN", planner.CoursesNotPlanned[0].CourseIdsOverlap[1]);
            Assert.AreEqual(2, planner.CoursesNotPlanned[1].CourseIdsOverlap.Count());
            Assert.AreEqual("SECDEV", planner.CoursesNotPlanned[1].CourseIdsOverlap[0]);
            Assert.AreEqual("XSD", planner.CoursesNotPlanned[1].CourseIdsOverlap[1]);
        }



        private static CoursePriority CreateNewCourseWithOneCourseImplementation(string courseId, int priority, DateTime[] days)
        {
            return new CoursePriority
            {
                CourseId = courseId,
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
