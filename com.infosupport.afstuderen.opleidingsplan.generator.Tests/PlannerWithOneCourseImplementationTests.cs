using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using com.infosupport.afstuderen.opleidingsplan.models;
using com.infosupport.afstuderen.opleidingsplan.generator.tests.helpers;

namespace com.infosupport.afstuderen.opleidingsplan.generator.tests
{
    [TestClass]
    public class PlannerWithOneCourseImplementationTests : CourseTestHelper
    {
        [TestMethod]
        public void PlanThreeCourses_NoOverlap_ThreeCoursesPlanned()
        {
            // Arrange
            Planner planner = new Planner();

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithOneCourseImplementation("SCRUMES", 1, 
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewModelCourseWithOneCourseImplementation("ENEST", 1, 
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }),
                CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1, 
                new DateTime[] { new DateTime(2017, 1, 16), new DateTime(2017, 1, 17), new DateTime(2017, 1, 18) }),
            };

            // Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(3, planner.GetAllCourses().Count());
            Assert.AreEqual(Status.PLANNED, planner.GetAllCourses().ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("SCRUMES", planner.GetAllCourses().ElementAt(0).Code);
            Assert.AreEqual(Status.PLANNED, planner.GetAllCourses().ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENEST", planner.GetAllCourses().ElementAt(1).Code);
            Assert.AreEqual(Status.PLANNED, planner.GetAllCourses().ElementAt(2).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENDEVN", planner.GetAllCourses().ElementAt(2).Code);

            Assert.AreEqual(3, planner.GetPlannedCourses().Count());
            Assert.AreEqual(0, planner.GetNotPlannedCourses().Count());
        }

        [TestMethod]
        public void PlanOneCourse_NoImplementations_NoPlannedCourses()
        {
            // Arrange
            Planner planner = new Planner();

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                new models.Course
                {
                    Code = "SCRUMES"
                }
            };

            // Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(0, planner.GetPlannedCourses().Count());
            Assert.AreEqual(1, planner.GetNotPlannedCourses().Count());
        }

        [TestMethod]
        public void PlanThreeCourses_OneWithFullOverlap_TwoCoursesPlanned()
        {
            // Arrange
            Planner planner = new Planner();

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithOneCourseImplementation("SCRUMES", 1, 
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewModelCourseWithOneCourseImplementation("ENEST", 1, 
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }),
                CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1, 
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }),
            };

            // Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(3, planner.GetAllCourses().Count());
            Assert.AreEqual(Status.PLANNED, planner.GetAllCourses().ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("SCRUMES", planner.GetAllCourses().ElementAt(0).Code);
            Assert.AreEqual(Status.PLANNED, planner.GetAllCourses().ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENEST", planner.GetAllCourses().ElementAt(1).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetAllCourses().ElementAt(2).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENDEVN", planner.GetAllCourses().ElementAt(2).Code);

            Assert.AreEqual(2, planner.GetPlannedCourses().Count());
            Assert.AreEqual(1, planner.GetNotPlannedCourses().Count());
        }

        [TestMethod]
        public void PlanThreeCourses_OneWithOverlapOneDay_TwoCoursesPlanned()
        {
            // Arrange
            Planner planner = new Planner();

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithOneCourseImplementation("SCRUMES", 1, new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewModelCourseWithOneCourseImplementation("ENEST", 1, new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5), new DateTime(2017, 1, 6) }),
                CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1, new DateTime[] { new DateTime(2017, 1, 16), new DateTime(2017, 1, 17), new DateTime(2017, 1, 18) }),
            };

            // Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(3, planner.GetAllCourses().Count());
            Assert.AreEqual(Status.PLANNED, planner.GetAllCourses().ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("SCRUMES", planner.GetAllCourses().ElementAt(0).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetAllCourses().ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENEST", planner.GetAllCourses().ElementAt(1).Code);
            Assert.AreEqual(Status.PLANNED, planner.GetAllCourses().ElementAt(2).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENDEVN", planner.GetAllCourses().ElementAt(2).Code);

            Assert.AreEqual(2, planner.GetPlannedCourses().Count());
            Assert.AreEqual(1, planner.GetNotPlannedCourses().Count());
        }

        [TestMethod]
        public void PlanThreeCourses_OneWithOverlapOneDayAndPriority_TwoCoursesPlanned()
        {
            // Arrange
            Planner planner = new Planner();

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithOneCourseImplementation("SCRUMES", 2, new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewModelCourseWithOneCourseImplementation("ENEST", 1, new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5), new DateTime(2017, 1, 6) }),
                CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1, new DateTime[] { new DateTime(2017, 1, 16), new DateTime(2017, 1, 17), new DateTime(2017, 1, 18) }),
            };

            // Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(3, planner.GetAllCourses().Count());
            Assert.AreEqual(Status.PLANNED, planner.GetAllCourses().ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENEST", planner.GetAllCourses().ElementAt(0).Code);
            Assert.AreEqual(Status.PLANNED, planner.GetAllCourses().ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENDEVN", planner.GetAllCourses().ElementAt(1).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetAllCourses().ElementAt(2).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("SCRUMES", planner.GetAllCourses().ElementAt(2).Code);

            Assert.AreEqual(2, planner.GetPlannedCourses().Count());
            Assert.AreEqual(1, planner.GetNotPlannedCourses().Count());
        }

        [TestMethod]
        public void PlanThreeCourses_TwoWithOverlapOneDayAndTwoPriority_OneCoursePlanned()
        {
            // Arrange
            Planner planner = new Planner();

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithOneCourseImplementation("SCRUMES", 2, new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewModelCourseWithOneCourseImplementation("ENEST", 1, new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5), new DateTime(2017, 1, 6) }),
                CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1, new DateTime[] { new DateTime(2017, 1, 6) }),
            };

            // Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(3, planner.GetAllCourses().Count());
            Assert.AreEqual(Status.PLANNED, planner.GetAllCourses().ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENEST", planner.GetAllCourses().ElementAt(0).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetAllCourses().ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENDEVN", planner.GetAllCourses().ElementAt(1).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetAllCourses().ElementAt(2).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("SCRUMES", planner.GetAllCourses().ElementAt(2).Code);

            Assert.AreEqual(1, planner.GetPlannedCourses().Count());
            Assert.AreEqual(2, planner.GetNotPlannedCourses().Count());
        }

        [TestMethod]
        public void PlanThreeCourses_TwoWithOverlapOneDayAndThreePriorities_TwoCoursesPlanned()
        {
            // Arrange
            Planner planner = new Planner();

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithOneCourseImplementation("SCRUMES", 3, new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewModelCourseWithOneCourseImplementation("ENEST", 2, new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5), new DateTime(2017, 1, 6) }),
                CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1, new DateTime[] { new DateTime(2017, 1, 6) }),
            };

            // Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(3, planner.GetAllCourses().Count());
            Assert.AreEqual(Status.PLANNED, planner.GetAllCourses().ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENDEVN", planner.GetAllCourses().ElementAt(0).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetAllCourses().ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENEST", planner.GetAllCourses().ElementAt(1).Code);
            Assert.AreEqual(Status.PLANNED, planner.GetAllCourses().ElementAt(2).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("SCRUMES", planner.GetAllCourses().ElementAt(2).Code);

            Assert.AreEqual(2, planner.GetPlannedCourses().Count());
            Assert.AreEqual(1, planner.GetNotPlannedCourses().Count());
        }

        [TestMethod]
        public void PlanTwoCourses_WithOverlapOneDay_OneCoursePlanned_TestOverlapCourse()
        {
            // Arrange
            Planner planner = new Planner();

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithOneCourseImplementation("SCRUMES", 1, 
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewModelCourseWithOneCourseImplementation("ENEST", 1, 
                new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5), new DateTime(2017, 1, 6) }),
            };

            // Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(1, planner.GetPlannedCourses().Count());
            Assert.AreEqual(1, planner.GetNotPlannedCourses().Count());
            Assert.AreEqual("SCRUMES", planner.GetPlannedCourses().ElementAt(0).Code);
            Assert.AreEqual("ENEST", planner.GetNotPlannedCourses().ElementAt(0).Code);

            Assert.AreEqual(1, planner.GetNotPlannedCourses().ElementAt(0).IntersectedCourseIds.Count());
            Assert.AreEqual("SCRUMES", planner.GetNotPlannedCourses().ElementAt(0).IntersectedCourseIds.ElementAt(0));
        }

        [TestMethod]
        public void PlanSixCourses_FourWithOverlap_FourCoursesPlanned_TestOverlapCourses()
        {
            // Arrange
            Planner planner = new Planner();

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithOneCourseImplementation("SCRUMES", 1, new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewModelCourseWithOneCourseImplementation("ENEST", 1, new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5), new DateTime(2017, 1, 6) }),
                CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1, new DateTime[] { new DateTime(2017, 1, 6) }),
                CreateNewModelCourseWithOneCourseImplementation("SECDEV", 1, new DateTime[] { new DateTime(2017, 1, 16), new DateTime(2017, 1, 17) }),
                CreateNewModelCourseWithOneCourseImplementation("XSD", 1, new DateTime[] { new DateTime(2017, 1, 18) }),
                CreateNewModelCourseWithOneCourseImplementation("MVC", 1, new DateTime[] { new DateTime(2017, 1, 17), new DateTime(2017, 1, 18) }),
            };

            // Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(4, planner.GetPlannedCourses().Count());
            Assert.AreEqual("SCRUMES", planner.GetPlannedCourses().ElementAt(0).Code);
            Assert.AreEqual("ENDEVN", planner.GetPlannedCourses().ElementAt(1).Code);
            Assert.AreEqual("SECDEV", planner.GetPlannedCourses().ElementAt(2).Code);
            Assert.AreEqual("XSD", planner.GetPlannedCourses().ElementAt(3).Code);
            Assert.AreEqual(2, planner.GetNotPlannedCourses().Count());
            Assert.AreEqual("ENEST", planner.GetNotPlannedCourses().ElementAt(0).Code);
            Assert.AreEqual("MVC", planner.GetNotPlannedCourses().ElementAt(1).Code);

            Assert.AreEqual(2, planner.GetNotPlannedCourses().ElementAt(0).IntersectedCourseIds.Count());
            Assert.AreEqual("SCRUMES", planner.GetNotPlannedCourses().ElementAt(0).IntersectedCourseIds.ElementAt(0));
            Assert.AreEqual("ENDEVN", planner.GetNotPlannedCourses().ElementAt(0).IntersectedCourseIds.ElementAt(1));
            Assert.AreEqual(2, planner.GetNotPlannedCourses().ElementAt(1).IntersectedCourseIds.Count());
            Assert.AreEqual("SECDEV", planner.GetNotPlannedCourses().ElementAt(1).IntersectedCourseIds.ElementAt(0));
            Assert.AreEqual("XSD", planner.GetNotPlannedCourses().ElementAt(1).IntersectedCourseIds.ElementAt(1));
        }
    }
}
