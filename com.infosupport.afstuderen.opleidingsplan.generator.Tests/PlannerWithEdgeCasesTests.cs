using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using com.infosupport.afstuderen.opleidingsplan.generator.tests.helpers;
using com.infosupport.afstuderen.opleidingsplan.models;

namespace com.infosupport.afstuderen.opleidingsplan.generator.tests
{
    [TestClass]
    public class PlannerWithEdgeCasesTests : CourseTestHelper
    {
        [TestMethod]
        public void PlanThreeCourses_TwoCoursesPlanned()
        {
            //Arrange
            Planner planner = new Planner();

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithTwoCourseImplementations("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }),
                CreateNewModelCourseWithTwoCourseImplementations("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3)},
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7)}),
                CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4)}),

            };

            //Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(2, planner.GetPlannedCourses().Count());
            Assert.AreEqual(1, planner.GetNotPlannedCourses().Count());

            Assert.AreEqual("SCRUMES", planner.GetPlannedCourses().ElementAt(0).Code);
            Assert.AreEqual(Status.PLANNED, planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENEST", planner.GetPlannedCourses().ElementAt(1).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENDEVN", planner.GetNotPlannedCourses().ElementAt(0).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetNotPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(0).Status);
        }

        [TestMethod]
        public void PlanFourCourses_FourCoursesPlanned()
        {
            //Arrange
            Planner planner = new Planner();
            planner.StartDate = new DateTime(2017, 1, 1);

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithTwoCourseImplementations("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }),
                CreateNewModelCourseWithTwoCourseImplementations("ENEST", 1,
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7)},
                new DateTime[] { new DateTime(2017, 4, 17), new DateTime(2017, 4, 18)}),
                CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4)}),
                CreateNewModelCourseWithTwoCourseImplementations("SECDEV", 1,
                new DateTime[] { new DateTime(2017, 4, 18), new DateTime(2017, 4, 19)},
                new DateTime[] { new DateTime(2017, 5, 15), new DateTime(2017, 5, 16)}),
            };

            //Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(4, planner.GetPlannedCourses().Count());
            Assert.AreEqual(0, planner.GetNotPlannedCourses().Count());

            Assert.AreEqual("SCRUMES", planner.GetPlannedCourses().ElementAt(0).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENEST", planner.GetPlannedCourses().ElementAt(1).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENDEVN", planner.GetPlannedCourses().ElementAt(2).Code);
            Assert.AreEqual(Status.PLANNED, planner.GetPlannedCourses().ElementAt(2).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("SECDEV", planner.GetPlannedCourses().ElementAt(3).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetPlannedCourses().ElementAt(3).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.GetPlannedCourses().ElementAt(3).CourseImplementations.ElementAt(1).Status);
        }

        [TestMethod]
        public void PlanFourCourses_TwoCoursesPlanned_TwoImplementationsAfterEndDate()
        {
            //Arrange
            Planner planner = new Planner();
            planner.StartDate = new DateTime(2017, 1, 1);

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithTwoCourseImplementations("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }),
                CreateNewModelCourseWithTwoCourseImplementations("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4)},
                new DateTime[] { new DateTime(2017, 4, 17), new DateTime(2017, 4, 18)}),
                CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4)}),
                CreateNewModelCourseWithOneCourseImplementation("SECDEV", 1,
                new DateTime[] { new DateTime(2017, 4, 17), new DateTime(2017, 4, 18), new DateTime(2017, 4, 19)}),
            };

            //Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(2, planner.GetPlannedCourses().Count());
            Assert.AreEqual(2, planner.GetNotPlannedCourses().Count());

            Assert.AreEqual("SCRUMES", planner.GetPlannedCourses().ElementAt(0).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENEST", planner.GetPlannedCourses().ElementAt(1).Code);
            Assert.AreEqual(Status.PLANNED, planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.NOTPLANNED, planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENDEVN", planner.GetNotPlannedCourses().ElementAt(0).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetNotPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("SECDEV", planner.GetNotPlannedCourses().ElementAt(1).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetNotPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(0).Status);
        }

        [TestMethod]
        public void PlanFourCourses_ThreeCoursesPlanned()
        {
            //Arrange
            Planner planner = new Planner();
            planner.StartDate = new DateTime(2017, 1, 1);

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithTwoCourseImplementations("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }),
                CreateNewModelCourseWithTwoCourseImplementations("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4)},
                new DateTime[] { new DateTime(2017, 3, 13), new DateTime(2017, 3, 14)}),
                CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4)}),
                CreateNewModelCourseWithOneCourseImplementation("SECDEV", 1,
                new DateTime[] { new DateTime(2017, 3, 13), new DateTime(2017, 3, 14), new DateTime(2017, 3, 15)}),
            };

            //Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(3, planner.GetPlannedCourses().Count());
            Assert.AreEqual(1, planner.GetNotPlannedCourses().Count());

            Assert.AreEqual("SCRUMES", planner.GetPlannedCourses().ElementAt(0).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENEST", planner.GetPlannedCourses().ElementAt(1).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENDEVN", planner.GetPlannedCourses().ElementAt(2).Code);
            Assert.AreEqual(Status.PLANNED, planner.GetPlannedCourses().ElementAt(2).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("SECDEV", planner.GetNotPlannedCourses().ElementAt(0).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetNotPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(0).Status);
        }

        [TestMethod]
        public void PlanFiveCourses_ThreeCoursesPlanned()
        {
            //Arrange
            Planner planner = new Planner();

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithTreeCourseImplementations("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                new DateTime[] { new DateTime(2017, 2, 6), new DateTime(2017, 2, 7), new DateTime(2017, 2, 8) },
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }),
                CreateNewModelCourseWithTwoCourseImplementations("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4)},
                new DateTime[] { new DateTime(2017, 2, 6), new DateTime(2017, 2, 7) }),
                CreateNewModelCourseWithTwoCourseImplementations("WINDEV", 1,
                new DateTime[] { new DateTime(2017, 2, 6), new DateTime(2017, 2, 7)},
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7)}),
                CreateNewModelCourseWithOneCourseImplementation("ADCSB", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4)}),
                CreateNewModelCourseWithOneCourseImplementation("SECDEV", 1,
                new DateTime[] { new DateTime(2017, 2, 6), new DateTime(2017, 2, 7), new DateTime(2017, 2, 8)}),
            };

            //Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(3, planner.GetPlannedCourses().Count());
            Assert.AreEqual(2, planner.GetNotPlannedCourses().Count());

            Assert.AreEqual("SCRUMES", planner.GetPlannedCourses().ElementAt(0).Code);
            Assert.AreEqual(Status.PLANNED, planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(2).Status);
            Assert.AreEqual("ENEST", planner.GetPlannedCourses().ElementAt(1).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("WINDEV", planner.GetPlannedCourses().ElementAt(2).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetPlannedCourses().ElementAt(2).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.GetPlannedCourses().ElementAt(2).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ADCSB", planner.GetNotPlannedCourses().ElementAt(0).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetNotPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("SECDEV", planner.GetNotPlannedCourses().ElementAt(1).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetNotPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(0).Status);
        }


        
        [TestMethod]
        public void PlanFiveCourses_ThreeCoursesPlanned_Matrix()
        {
            //Arrange
            Planner planner = new Planner();

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithTreeCourseImplementations("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                new DateTime[] { new DateTime(2017, 2, 6), new DateTime(2017, 2, 7), new DateTime(2017, 2, 8) },
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }),
                CreateNewModelCourseWithTreeCourseImplementations("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                new DateTime[] { new DateTime(2017, 2, 6), new DateTime(2017, 2, 7), new DateTime(2017, 2, 8) },
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }),
                CreateNewModelCourseWithTreeCourseImplementations("WINDEV", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                new DateTime[] { new DateTime(2017, 2, 6), new DateTime(2017, 2, 7), new DateTime(2017, 2, 8) },
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }),
                CreateNewModelCourseWithOneCourseImplementation("ADCSB", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
            };

            //Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(3, planner.GetPlannedCourses().Count());
            Assert.AreEqual(1, planner.GetNotPlannedCourses().Count());

            Assert.AreEqual("SCRUMES", planner.GetPlannedCourses().ElementAt(0).Code);
            Assert.AreEqual(Status.PLANNED, planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(2).Status);
            Assert.AreEqual("ENEST", planner.GetPlannedCourses().ElementAt(1).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(2).Status);
            Assert.AreEqual("WINDEV", planner.GetPlannedCourses().ElementAt(2).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetPlannedCourses().ElementAt(2).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetPlannedCourses().ElementAt(2).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual(Status.PLANNED, planner.GetPlannedCourses().ElementAt(2).CourseImplementations.ElementAt(2).Status);
            Assert.AreEqual("ADCSB", planner.GetNotPlannedCourses().ElementAt(0).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.GetNotPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(0).Status);

        }

    }
}
