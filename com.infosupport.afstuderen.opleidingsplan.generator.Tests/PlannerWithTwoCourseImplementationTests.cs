﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using com.infosupport.afstuderen.opleidingsplan.generator.tests.helpers;

namespace com.infosupport.afstuderen.opleidingsplan.generator.tests
{
    [TestClass]
    public class PlannerWithTwoCourseImplementationTests : CourseTestHelper
    {
        [TestMethod]
        public void PlanThreeCourses_NoOverlap_ThreeCoursesConstant()
        {
            // Arrange
            Planner planner = new Planner();

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithTwoCourseImplementations("SCRUMES", 1, 
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }, 
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }),
                CreateNewModelCourseWithTwoCourseImplementations("ENEST", 1, 
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10)}, 
                new DateTime[] { new DateTime(2017, 4, 10), new DateTime(2017, 4, 11)}),
                CreateNewModelCourseWithTwoCourseImplementations("ENDEVN", 1, 
                new DateTime[] { new DateTime(2017, 1, 16), new DateTime(2017, 1, 17), new DateTime(2017, 1, 18) }, 
                new DateTime[] { new DateTime(2017, 3, 13), new DateTime(2017, 3, 14), new DateTime(2017, 3, 15) }),
            };

            // Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(3, planner.GetAllCourses().Count());

            Assert.AreEqual(Status.PLANNED, planner.GetAllCourses().ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.NOTPLANNED, planner.GetAllCourses().ElementAt(0).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("SCRUMES", planner.GetAllCourses().ElementAt(0).Code);

            Assert.AreEqual(Status.PLANNED, planner.GetAllCourses().ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.NOTPLANNED, planner.GetAllCourses().ElementAt(1).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENEST", planner.GetAllCourses().ElementAt(1).Code);

            Assert.AreEqual(Status.PLANNED, planner.GetAllCourses().ElementAt(2).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.NOTPLANNED, planner.GetAllCourses().ElementAt(2).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENDEVN", planner.GetAllCourses().ElementAt(2).Code);

            Assert.AreEqual(3, planner.GetPlannedCourses().Count());
            Assert.AreEqual(0, planner.GetNotPlannedCourses().Count());
        }

        [TestMethod]
        public void PlanThreeCourses_OverlapOneImplementation_ThreeCoursesConstant()
        {
            // Arrange
            Planner planner = new Planner();

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithTwoCourseImplementations("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }),
                CreateNewModelCourseWithTwoCourseImplementations("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10)},
                new DateTime[] { new DateTime(2017, 4, 10), new DateTime(2017, 4, 11)}),
                CreateNewModelCourseWithTwoCourseImplementations("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                new DateTime[] { new DateTime(2017, 3, 13), new DateTime(2017, 3, 14), new DateTime(2017, 3, 15) }),
            };

            // Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(3, planner.GetAllCourses().Count());

            Assert.AreEqual(Status.PLANNED, planner.GetAllCourses().ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.NOTPLANNED, planner.GetAllCourses().ElementAt(0).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("SCRUMES", planner.GetAllCourses().ElementAt(0).Code);

            Assert.AreEqual(Status.PLANNED, planner.GetAllCourses().ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.NOTPLANNED, planner.GetAllCourses().ElementAt(1).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENEST", planner.GetAllCourses().ElementAt(1).Code);

            Assert.AreEqual(Status.NOTPLANNED, planner.GetAllCourses().ElementAt(2).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.GetAllCourses().ElementAt(2).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENDEVN", planner.GetAllCourses().ElementAt(2).Code);

            Assert.AreEqual(3, planner.GetPlannedCourses().Count());
            Assert.AreEqual(0, planner.GetNotPlannedCourses().Count());
        }

        [TestMethod]
        public void PlanThreeCourses_OverlapTwoImplementation()
        {
            // Arrange
            Planner planner = new Planner();

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithTwoCourseImplementations("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }),
                CreateNewModelCourseWithTwoCourseImplementations("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5)},
                new DateTime[] { new DateTime(2017, 4, 10), new DateTime(2017, 4, 11)}),
                CreateNewModelCourseWithTwoCourseImplementations("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                new DateTime[] { new DateTime(2017, 3, 13), new DateTime(2017, 3, 14), new DateTime(2017, 3, 15) }),
            };

            // Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(3, planner.GetPlannedCourses().Count());
            Assert.AreEqual(Status.PLANNED, planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.NOTPLANNED, planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("SCRUMES", planner.GetPlannedCourses().ElementAt(0).Code);
            Assert.AreEqual(Status.NOTPLANNED, planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENEST", planner.GetPlannedCourses().ElementAt(1).Code);
            Assert.AreEqual(Status.NOTPLANNED, planner.GetPlannedCourses().ElementAt(2).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.GetPlannedCourses().ElementAt(2).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENDEVN", planner.GetPlannedCourses().ElementAt(2).Code);

            Assert.AreEqual(0, planner.GetNotPlannedCourses().Count());
        }

        [TestMethod]
        public void PlanThreeCourses_OverlapTwoImplementation_TwoCoursesPlanned()
        {
            // Arrange
            Planner planner = new Planner();

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithTwoCourseImplementations("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }),
                CreateNewModelCourseWithTwoCourseImplementations("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5)},
                new DateTime[] { new DateTime(2017, 3, 7), new DateTime(2017, 3, 8)}),
                CreateNewModelCourseWithTwoCourseImplementations("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                new DateTime[] { new DateTime(2017, 3, 8), new DateTime(2017, 3, 9), new DateTime(2017, 3, 10) }),
            };

            // Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(2, planner.GetPlannedCourses().Count());
            Assert.AreEqual(Status.PLANNED, planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.NOTPLANNED, planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("SCRUMES", planner.GetPlannedCourses().ElementAt(0).Code);
            Assert.AreEqual(Status.NOTPLANNED, planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENEST", planner.GetPlannedCourses().ElementAt(1).Code);

            Assert.AreEqual(1, planner.GetNotPlannedCourses().Count());
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetNotPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetNotPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENDEVN", planner.GetNotPlannedCourses().ElementAt(0).Code);
        }

        [TestMethod]
        public void PlanThreeCourses_OverlapTwoImplementation_MoveSecondCourseImplementation_ThreeCoursesPlanned()
        {
            // Arrange
            Planner planner = new Planner();

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithTwoCourseImplementations("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }),
                CreateNewModelCourseWithTwoCourseImplementations("ENEST", 1,
                new DateTime[] { new DateTime(2017, 2, 13), new DateTime(2017, 2, 14)},
                new DateTime[] { new DateTime(2017, 4, 10), new DateTime(2017, 4, 11)}),
                CreateNewModelCourseWithTwoCourseImplementations("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) },
                new DateTime[] { new DateTime(2017, 2, 14), new DateTime(2017, 2, 15) }),
            };

            // Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(3, planner.GetPlannedCourses().Count());
            Assert.AreEqual(Status.PLANNED, planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.NOTPLANNED, planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("SCRUMES", planner.GetPlannedCourses().ElementAt(0).Code);
            Assert.AreEqual(Status.NOTPLANNED, planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENEST", planner.GetPlannedCourses().ElementAt(1).Code);
            Assert.AreEqual(Status.NOTPLANNED, planner.GetPlannedCourses().ElementAt(2).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.GetPlannedCourses().ElementAt(2).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENDEVN", planner.GetPlannedCourses().ElementAt(2).Code);

            Assert.AreEqual(0, planner.GetNotPlannedCourses().Count());
        }
    }
}
