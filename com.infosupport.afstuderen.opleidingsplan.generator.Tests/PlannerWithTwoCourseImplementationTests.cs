using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using com.infosupport.afstuderen.opleidingsplan.generator.tests.helpers;
using com.infosupport.afstuderen.opleidingsplan.dal.mappers;
using Moq;

namespace com.infosupport.afstuderen.opleidingsplan.generator.tests
{
    [TestClass]
    public class PlannerWithTwoCourseImplementationTests : CourseTestHelper
    {
        [TestMethod]
        public void PlanThreeCourses_NoOverlap_ThreeCoursesPlanned()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 1);

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
            Assert.AreEqual(3, planner.PlannedCourses.Count());
            Assert.AreEqual(Status.PLANNED, planner.AllCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.NOTPLANNED, planner.AllCourses.ElementAt(0).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("SCRUMES", planner.AllCourses.ElementAt(0).Code);
            Assert.AreEqual(Status.PLANNED, planner.AllCourses.ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.UNPLANNABLE, planner.AllCourses.ElementAt(1).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENEST", planner.AllCourses.ElementAt(1).Code);
            Assert.AreEqual(Status.PLANNED, planner.AllCourses.ElementAt(2).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.NOTPLANNED, planner.AllCourses.ElementAt(2).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENDEVN", planner.AllCourses.ElementAt(2).Code);

            Assert.AreEqual(0, planner.NotPlannedCourses.Count());
        }

        [TestMethod]
        public void PlanThreeCourses_OverlapOneImplementation_ThreeCoursesPlanned()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 1);

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
            Assert.AreEqual(3, planner.PlannedCourses.Count());
            Assert.AreEqual(Status.PLANNED, planner.AllCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.NOTPLANNED, planner.AllCourses.ElementAt(0).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("SCRUMES", planner.AllCourses.ElementAt(0).Code);
            Assert.AreEqual(Status.PLANNED, planner.AllCourses.ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.UNPLANNABLE, planner.AllCourses.ElementAt(1).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENEST", planner.AllCourses.ElementAt(1).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.AllCourses.ElementAt(2).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.AllCourses.ElementAt(2).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENDEVN", planner.AllCourses.ElementAt(2).Code);

            Assert.AreEqual(0, planner.NotPlannedCourses.Count());
        }

        [TestMethod]
        public void PlanThreeCourses_OverlapTwoImplementation()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 1);

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
            Assert.AreEqual(3, planner.PlannedCourses.Count());
            Assert.AreEqual(Status.UNPLANNABLE, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("SCRUMES", planner.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.UNPLANNABLE, planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENEST", planner.PlannedCourses.ElementAt(1).Code);
            Assert.AreEqual(Status.NOTPLANNED, planner.PlannedCourses.ElementAt(2).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(2).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENDEVN", planner.PlannedCourses.ElementAt(2).Code);

            Assert.AreEqual(0, planner.NotPlannedCourses.Count());
        }

        [TestMethod]
        public void PlanThreeCourses_OverlapTwoImplementation_TwoCoursesPlanned()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 1);

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
            Assert.AreEqual(2, planner.PlannedCourses.Count());
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.UNPLANNABLE, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("SCRUMES", planner.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENEST", planner.PlannedCourses.ElementAt(1).Code);

            Assert.AreEqual(1, planner.NotPlannedCourses.Count());
            Assert.AreEqual(Status.UNPLANNABLE, planner.NotPlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.UNPLANNABLE, planner.NotPlannedCourses.ElementAt(0).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENDEVN", planner.NotPlannedCourses.ElementAt(0).Code);
        }

        [TestMethod]
        public void PlanThreeCourses_OverlapTwoImplementation_MoveSecondCourseImplementation_ThreeCoursesPlanned()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 1);

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
            Assert.AreEqual(3, planner.PlannedCourses.Count());
            Assert.AreEqual(Status.UNPLANNABLE, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("SCRUMES", planner.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.UNPLANNABLE, planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENEST", planner.PlannedCourses.ElementAt(1).Code);
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(2).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.NOTPLANNED, planner.PlannedCourses.ElementAt(2).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENDEVN", planner.PlannedCourses.ElementAt(2).Code);

            Assert.AreEqual(0, planner.NotPlannedCourses.Count());
        }
    }
}
