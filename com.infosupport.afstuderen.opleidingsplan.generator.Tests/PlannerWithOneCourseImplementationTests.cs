using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using com.infosupport.afstuderen.opleidingsplan.models;
using com.infosupport.afstuderen.opleidingsplan.generator.tests.helpers;
using com.infosupport.afstuderen.opleidingsplan.dal.mappers;
using Moq;
using System.Collections.ObjectModel;

namespace com.infosupport.afstuderen.opleidingsplan.generator.tests
{
    [TestClass]
    public class PlannerWithOneCourseImplementationTests : CourseTestHelper
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
            Assert.AreEqual(3, planner.AllCourses.Count());
            Assert.AreEqual(Status.PLANNED, planner.AllCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("SCRUMES", planner.AllCourses.ElementAt(0).Code);
            Assert.AreEqual(Status.PLANNED, planner.AllCourses.ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENEST", planner.AllCourses.ElementAt(1).Code);
            Assert.AreEqual(Status.PLANNED, planner.AllCourses.ElementAt(2).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENDEVN", planner.AllCourses.ElementAt(2).Code);

            Assert.AreEqual(3, planner.PlannedCourses.Count());
            Assert.AreEqual(0, planner.NotPlannedCourses.Count());
        }

        [TestMethod]
        public void PlanOneCourse_NoImplementations_NoPlannedCourses()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 1);

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
            Assert.AreEqual(0, planner.PlannedCourses.Count());
            Assert.AreEqual(1, planner.NotPlannedCourses.Count());
        }

        [TestMethod]
        public void PlanThreeCourses_OneWithFullOverlap_TwoCoursesPlanned()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 1);

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
            Assert.AreEqual(3, planner.AllCourses.Count());
            Assert.AreEqual(Status.PLANNED, planner.AllCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("SCRUMES", planner.AllCourses.ElementAt(0).Code);
            Assert.AreEqual(Status.PLANNED, planner.AllCourses.ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENEST", planner.AllCourses.ElementAt(1).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.AllCourses.ElementAt(2).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENDEVN", planner.AllCourses.ElementAt(2).Code);

            Assert.AreEqual(2, planner.PlannedCourses.Count());
            Assert.AreEqual(1, planner.NotPlannedCourses.Count());
        }

        [TestMethod]
        public void PlanThreeCourses_OneWithOverlapOneDay_TwoCoursesPlanned()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 1);

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithOneCourseImplementation("SCRUMES", 1, new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewModelCourseWithOneCourseImplementation("ENEST", 1, new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5), new DateTime(2017, 1, 6) }),
                CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1, new DateTime[] { new DateTime(2017, 1, 16), new DateTime(2017, 1, 17), new DateTime(2017, 1, 18) }),
            };

            // Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(3, planner.AllCourses.Count());
            Assert.AreEqual(Status.PLANNED, planner.AllCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("SCRUMES", planner.AllCourses.ElementAt(0).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.AllCourses.ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENEST", planner.AllCourses.ElementAt(1).Code);
            Assert.AreEqual(Status.PLANNED, planner.AllCourses.ElementAt(2).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENDEVN", planner.AllCourses.ElementAt(2).Code);

            Assert.AreEqual(2, planner.PlannedCourses.Count());
            Assert.AreEqual(1, planner.NotPlannedCourses.Count());
        }

        [TestMethod]
        public void PlanThreeCourses_OneWithOverlapOneDayAndPriority_TwoCoursesPlanned()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 1);

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithOneCourseImplementation("SCRUMES", 2, new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewModelCourseWithOneCourseImplementation("ENEST", 1, new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5), new DateTime(2017, 1, 6) }),
                CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1, new DateTime[] { new DateTime(2017, 1, 16), new DateTime(2017, 1, 17), new DateTime(2017, 1, 18) }),
            };

            // Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(3, planner.AllCourses.Count());
            Assert.AreEqual(Status.PLANNED, planner.AllCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENEST", planner.AllCourses.ElementAt(0).Code);
            Assert.AreEqual(Status.PLANNED, planner.AllCourses.ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENDEVN", planner.AllCourses.ElementAt(1).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.AllCourses.ElementAt(2).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("SCRUMES", planner.AllCourses.ElementAt(2).Code);

            Assert.AreEqual(2, planner.PlannedCourses.Count());
            Assert.AreEqual(1, planner.NotPlannedCourses.Count());
        }

        [TestMethod]
        public void PlanThreeCourses_TwoWithOverlapOneDayAndTwoPriority_OneCoursePlanned()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 1);

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithOneCourseImplementation("SCRUMES", 2, new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewModelCourseWithOneCourseImplementation("ENEST", 1, new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5), new DateTime(2017, 1, 6) }),
                CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1, new DateTime[] { new DateTime(2017, 1, 6) }),
            };

            // Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(3, planner.AllCourses.Count());
            Assert.AreEqual(Status.PLANNED, planner.AllCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENEST", planner.AllCourses.ElementAt(0).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.AllCourses.ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENDEVN", planner.AllCourses.ElementAt(1).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.AllCourses.ElementAt(2).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("SCRUMES", planner.AllCourses.ElementAt(2).Code);

            Assert.AreEqual(1, planner.PlannedCourses.Count());
            Assert.AreEqual(2, planner.NotPlannedCourses.Count());
        }

        [TestMethod]
        public void PlanThreeCourses_TwoWithOverlapOneDayAndThreePriorities_TwoCoursesPlanned()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 1);

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithOneCourseImplementation("SCRUMES", 3, new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewModelCourseWithOneCourseImplementation("ENEST", 2, new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5), new DateTime(2017, 1, 6) }),
                CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1, new DateTime[] { new DateTime(2017, 1, 6) }),
            };

            // Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(3, planner.AllCourses.Count());
            Assert.AreEqual(Status.PLANNED, planner.AllCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENDEVN", planner.AllCourses.ElementAt(0).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.AllCourses.ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENEST", planner.AllCourses.ElementAt(1).Code);
            Assert.AreEqual(Status.PLANNED, planner.AllCourses.ElementAt(2).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("SCRUMES", planner.AllCourses.ElementAt(2).Code);

            Assert.AreEqual(2, planner.PlannedCourses.Count());
            Assert.AreEqual(1, planner.NotPlannedCourses.Count());
        }

        [TestMethod]
        public void PlanTwoCourses_WithOverlapOneDay_OneCoursePlanned_TestOverlapCourse()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 1);

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
            Assert.AreEqual(1, planner.PlannedCourses.Count());
            Assert.AreEqual(1, planner.NotPlannedCourses.Count());
            Assert.AreEqual("SCRUMES", planner.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual("ENEST", planner.NotPlannedCourses.ElementAt(0).Code);

            Assert.AreEqual(1, planner.NotPlannedCourses.ElementAt(0).IntersectedCourseIds.Count());
            Assert.AreEqual("SCRUMES", planner.NotPlannedCourses.ElementAt(0).IntersectedCourseIds.ElementAt(0));
        }

        [TestMethod]
        public void PlanSixCourses_FourWithOverlap_FourCoursesPlanned_TestOverlapCourses()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 1);

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
            Assert.AreEqual(4, planner.PlannedCourses.Count());
            Assert.AreEqual("SCRUMES", planner.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual("ENDEVN", planner.PlannedCourses.ElementAt(1).Code);
            Assert.AreEqual("SECDEV", planner.PlannedCourses.ElementAt(2).Code);
            Assert.AreEqual("XSD", planner.PlannedCourses.ElementAt(3).Code);
            Assert.AreEqual(2, planner.NotPlannedCourses.Count());
            Assert.AreEqual("ENEST", planner.NotPlannedCourses.ElementAt(0).Code);
            Assert.AreEqual("MVC", planner.NotPlannedCourses.ElementAt(1).Code);

            Assert.AreEqual(2, planner.NotPlannedCourses.ElementAt(0).IntersectedCourseIds.Count());
            Assert.AreEqual("SCRUMES", planner.NotPlannedCourses.ElementAt(0).IntersectedCourseIds.ElementAt(0));
            Assert.AreEqual("ENDEVN", planner.NotPlannedCourses.ElementAt(0).IntersectedCourseIds.ElementAt(1));
            Assert.AreEqual(2, planner.NotPlannedCourses.ElementAt(1).IntersectedCourseIds.Count());
            Assert.AreEqual("SECDEV", planner.NotPlannedCourses.ElementAt(1).IntersectedCourseIds.ElementAt(0));
            Assert.AreEqual("XSD", planner.NotPlannedCourses.ElementAt(1).IntersectedCourseIds.ElementAt(1));
        }

        [TestMethod]
        public void PlanThreeCourses_StartDayAfterFirstDate_TwoCoursesPlanned()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 3);

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
            Assert.AreEqual(3, planner.AllCourses.Count());

            Assert.AreEqual(2, planner.PlannedCourses.Count());
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENEST", planner.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENDEVN", planner.PlannedCourses.ElementAt(1).Code);

            Assert.AreEqual(1, planner.NotPlannedCourses.Count());
            Assert.AreEqual(Status.UNPLANNABLE, planner.NotPlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("SCRUMES", planner.NotPlannedCourses.ElementAt(0).Code);
        }

        [TestMethod]
        public void PlanThreeCourses_StartDayAfterPeriod_TwoCoursesPlanned()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 1);

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithOneCourseImplementation("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewModelCourseWithOneCourseImplementation("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }),
                CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 4, 16), new DateTime(2017, 1, 17), new DateTime(2017, 1, 18) }),
            };

            // Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(3, planner.AllCourses.Count());

            Assert.AreEqual(2, planner.PlannedCourses.Count());
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("SCRUMES", planner.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENEST", planner.PlannedCourses.ElementAt(1).Code);

            Assert.AreEqual(1, planner.NotPlannedCourses.Count());
            Assert.AreEqual(Status.UNPLANNABLE, planner.NotPlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENDEVN", planner.NotPlannedCourses.ElementAt(0).Code);
        }

        [TestMethod]
        public void PlanThreeCourses_BlockDates_OneCoursePlanned()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 1);
            planner.BlockedDates = new Collection<DateTime>()
            {
                new DateTime(2017, 1, 11),
                new DateTime(2017, 1, 12),
                new DateTime(2017, 1, 13),
                new DateTime(2017, 1, 14),
                new DateTime(2017, 1, 15),
                new DateTime(2017, 1, 16),
            };

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
            Assert.AreEqual(3, planner.AllCourses.Count());

            Assert.AreEqual(1, planner.PlannedCourses.Count());
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("SCRUMES", planner.PlannedCourses.ElementAt(0).Code);

            Assert.AreEqual(2, planner.NotPlannedCourses.Count());
            Assert.AreEqual(Status.UNPLANNABLE, planner.NotPlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENEST", planner.NotPlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.NotPlannedCourses.ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENDEVN", planner.NotPlannedCourses.ElementAt(1).Code);
        }

        [TestMethod]
        public void PlanTwoCourses_StartDayOfFirstImplementationAfterPeriod_TwoCoursesPlanned()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 5);

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithTwoCourseImplementations("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                new DateTime[] { new DateTime(2017, 1, 16), new DateTime(2017, 1, 17), new DateTime(2017, 1, 17) }),
                CreateNewModelCourseWithOneCourseImplementation("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }),
            };

            // Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(2, planner.AllCourses.Count());

            Assert.AreEqual(2, planner.PlannedCourses.Count());
            Assert.AreEqual(Status.UNPLANNABLE, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("SCRUMES", planner.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("ENEST", planner.PlannedCourses.ElementAt(1).Code);

            Assert.AreEqual(0, planner.NotPlannedCourses.Count());
        }
    }
}
