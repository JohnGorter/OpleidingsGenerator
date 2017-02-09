﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InfoSupport.KC.OpleidingsplanGenerator.Generator.Tests.Helpers;
using System.Collections.Generic;
using System.Linq;
using InfoSupport.KC.OpleidingsplanGenerator.Dal.Mappers;
using Moq;
using System.Collections.ObjectModel;

namespace InfoSupport.KC.OpleidingsplanGenerator.Generator.Tests
{
    [TestClass]
    public class PlannerWithOLCTests : CourseTestHelper
    {
        [TestMethod]
        public void PlanThreeCourses_SevenCoursesPlanned_ApplyOLCFourTimes()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 1);

            IEnumerable<Models.Course> coursesToPlan = new List<Models.Course>()
            {
                CreateNewModelCourseWithOneCourseImplementation("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewModelCourseWithOneCourseImplementation("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }),
                CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 16), new DateTime(2017, 1, 17), new DateTime(2017, 1, 18) }),
            };

            // Act
            planner.PlanCoursesWithOlc(coursesToPlan);
            
            // Assert
            Assert.AreEqual(7, planner.PlannedCourses.Count());
            Assert.AreEqual("SCRUMES", planner.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(new DateTime(2017, 1, 2), planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(3, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("ENEST", planner.PlannedCourses.ElementAt(1).Code);
            Assert.AreEqual(new DateTime(2017, 1, 9), planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(3, planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("ENDEVN", planner.PlannedCourses.ElementAt(2).Code);
            Assert.AreEqual(new DateTime(2017, 1, 16), planner.PlannedCourses.ElementAt(2).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(3, planner.PlannedCourses.ElementAt(2).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("OLC1", planner.PlannedCourses.ElementAt(3).Code);
            Assert.AreEqual(new DateTime(2017, 1, 5), planner.PlannedCourses.ElementAt(3).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(2, planner.PlannedCourses.ElementAt(3).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(2, planner.PlannedCourses.ElementAt(3).Duration);

            Assert.AreEqual("OLC2", planner.PlannedCourses.ElementAt(4).Code);
            Assert.AreEqual(new DateTime(2017, 1, 12), planner.PlannedCourses.ElementAt(4).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(2, planner.PlannedCourses.ElementAt(4).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(2, planner.PlannedCourses.ElementAt(4).Duration);

            Assert.AreEqual("OLC3", planner.PlannedCourses.ElementAt(5).Code);
            Assert.AreEqual(new DateTime(2017, 1, 19), planner.PlannedCourses.ElementAt(5).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(2, planner.PlannedCourses.ElementAt(5).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(2, planner.PlannedCourses.ElementAt(5).Duration);

            Assert.AreEqual("OLC4", planner.PlannedCourses.ElementAt(6).Code);
            Assert.AreEqual(new DateTime(2017, 1, 23), planner.PlannedCourses.ElementAt(6).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(3, planner.PlannedCourses.ElementAt(6).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(3, planner.PlannedCourses.ElementAt(6).Duration);

            Assert.AreEqual(0, planner.NotPlannedCourses.Count());
        }

        [TestMethod]
        public void PlanThreeCourses_SixCoursesPlanned_ApplyOLCThreeTimes_TwoCoursesOneWeek()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 1);

            IEnumerable<Models.Course> coursesToPlan = new List<Models.Course>()
            {
                CreateNewModelCourseWithOneCourseImplementation("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewModelCourseWithOneCourseImplementation("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 6) }),
                CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }),
            };

            // Act
            planner.PlanCoursesWithOlc(coursesToPlan);

            // Assert
            Assert.AreEqual(6, planner.PlannedCourses.Count());
            Assert.AreEqual("SCRUMES", planner.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(new DateTime(2017, 1, 2), planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(3, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("ENEST", planner.PlannedCourses.ElementAt(1).Code);
            Assert.AreEqual(new DateTime(2017, 1, 6), planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(1, planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("ENDEVN", planner.PlannedCourses.ElementAt(2).Code);
            Assert.AreEqual(new DateTime(2017, 1, 9), planner.PlannedCourses.ElementAt(2).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(3, planner.PlannedCourses.ElementAt(2).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("OLC1", planner.PlannedCourses.ElementAt(3).Code);
            Assert.AreEqual(new DateTime(2017, 1, 5), planner.PlannedCourses.ElementAt(3).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(1, planner.PlannedCourses.ElementAt(3).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(1, planner.PlannedCourses.ElementAt(3).Duration);

            Assert.AreEqual("OLC2", planner.PlannedCourses.ElementAt(4).Code);
            Assert.AreEqual(new DateTime(2017, 1, 12), planner.PlannedCourses.ElementAt(4).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(2, planner.PlannedCourses.ElementAt(4).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(2, planner.PlannedCourses.ElementAt(4).Duration);

            Assert.AreEqual("OLC3", planner.PlannedCourses.ElementAt(5).Code);
            Assert.AreEqual(new DateTime(2017, 1, 16), planner.PlannedCourses.ElementAt(5).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(3, planner.PlannedCourses.ElementAt(5).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(3, planner.PlannedCourses.ElementAt(5).Duration);

            Assert.AreEqual(0, planner.NotPlannedCourses.Count());
        }

        [TestMethod]
        public void PlanThreeCourses_SixCoursesPlanned_ApplyOLCThreeTimes_ThreeCoursesOneWeek()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 1);

            IEnumerable<Models.Course> coursesToPlan = new List<Models.Course>()
            {
                CreateNewModelCourseWithOneCourseImplementation("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2) }),
                CreateNewModelCourseWithOneCourseImplementation("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 4) }),
                CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 6) }),
            };

            // Act
            planner.PlanCoursesWithOlc(coursesToPlan);

            // Assert
            Assert.AreEqual(6, planner.PlannedCourses.Count());
            Assert.AreEqual("SCRUMES", planner.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(new DateTime(2017, 1, 2), planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(1, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("ENEST", planner.PlannedCourses.ElementAt(1).Code);
            Assert.AreEqual(new DateTime(2017, 1, 4), planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(1, planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("ENDEVN", planner.PlannedCourses.ElementAt(2).Code);
            Assert.AreEqual(new DateTime(2017, 1, 6), planner.PlannedCourses.ElementAt(2).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(1, planner.PlannedCourses.ElementAt(2).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("OLC1", planner.PlannedCourses.ElementAt(3).Code);
            Assert.AreEqual(new DateTime(2017, 1, 3), planner.PlannedCourses.ElementAt(3).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(1, planner.PlannedCourses.ElementAt(3).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(1, planner.PlannedCourses.ElementAt(3).Duration);

            Assert.AreEqual("OLC2", planner.PlannedCourses.ElementAt(4).Code);
            Assert.AreEqual(new DateTime(2017, 1, 5), planner.PlannedCourses.ElementAt(4).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(1, planner.PlannedCourses.ElementAt(4).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(1, planner.PlannedCourses.ElementAt(4).Duration);

            Assert.AreEqual("OLC3", planner.PlannedCourses.ElementAt(5).Code);
            Assert.AreEqual(new DateTime(2017, 1, 9), planner.PlannedCourses.ElementAt(5).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(5, planner.PlannedCourses.ElementAt(5).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(5, planner.PlannedCourses.ElementAt(5).Duration);

            Assert.AreEqual(0, planner.NotPlannedCourses.Count());
        }

        [TestMethod]
        public void PlanTwoCourses_SixCoursesPlanned_ApplyOLCFourTimes_OLCWithWeekend()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2016, 12, 26);

            IEnumerable<Models.Course> coursesToPlan = new List<Models.Course>()
            {
                CreateNewModelCourseWithOneCourseImplementation("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewModelCourseWithOneCourseImplementation("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 11), new DateTime(2017, 1, 12), new DateTime(2017, 1, 13) }),
            };

            // Act
            planner.PlanCoursesWithOlc(coursesToPlan);

            // Assert
            Assert.AreEqual(6, planner.PlannedCourses.Count());
            Assert.AreEqual("SCRUMES", planner.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(new DateTime(2017, 1, 2), planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(3, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("ENEST", planner.PlannedCourses.ElementAt(1).Code);
            Assert.AreEqual(new DateTime(2017, 1, 11), planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(3, planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("OLC1", planner.PlannedCourses.ElementAt(2).Code);
            Assert.AreEqual(new DateTime(2016, 12, 26), planner.PlannedCourses.ElementAt(2).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(5, planner.PlannedCourses.ElementAt(2).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(5, planner.PlannedCourses.ElementAt(2).Duration);

            Assert.AreEqual("OLC2", planner.PlannedCourses.ElementAt(3).Code);
            Assert.AreEqual(new DateTime(2017, 1, 5), planner.PlannedCourses.ElementAt(3).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(2, planner.PlannedCourses.ElementAt(3).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(2, planner.PlannedCourses.ElementAt(3).Duration);

            Assert.AreEqual("OLC3", planner.PlannedCourses.ElementAt(4).Code);
            Assert.AreEqual(new DateTime(2017, 1, 9), planner.PlannedCourses.ElementAt(4).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(2, planner.PlannedCourses.ElementAt(4).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(2, planner.PlannedCourses.ElementAt(4).Duration);

            Assert.AreEqual("OLC4", planner.PlannedCourses.ElementAt(5).Code);
            Assert.AreEqual(new DateTime(2017, 1, 16), planner.PlannedCourses.ElementAt(5).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(5, planner.PlannedCourses.ElementAt(5).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(5, planner.PlannedCourses.ElementAt(5).Duration);

            Assert.AreEqual(0, planner.NotPlannedCourses.Count());
        }

        [TestMethod]
        public void PlanTwoCourses_SixCoursesPlanned_ApplyOLCFourTimes_OLCWithBlockedDate()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2016, 12, 26);
            planner.BlockedDates = new Collection<DateTime>()
            {
                new DateTime(2017, 1, 4),
            };

            IEnumerable<Models.Course> coursesToPlan = new List<Models.Course>()
            {
                CreateNewModelCourseWithOneCourseImplementation("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2) }),
                CreateNewModelCourseWithOneCourseImplementation("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 6) }),
            };

            // Act
            planner.PlanCoursesWithOlc(coursesToPlan);

            // Assert
            Assert.AreEqual(6, planner.PlannedCourses.Count());
            Assert.AreEqual("SCRUMES", planner.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(new DateTime(2017, 1, 2), planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(1, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("ENEST", planner.PlannedCourses.ElementAt(1).Code);
            Assert.AreEqual(new DateTime(2017, 1, 6), planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(1, planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("OLC1", planner.PlannedCourses.ElementAt(2).Code);
            Assert.AreEqual(new DateTime(2016, 12, 26), planner.PlannedCourses.ElementAt(2).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(5, planner.PlannedCourses.ElementAt(2).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(5, planner.PlannedCourses.ElementAt(2).Duration);

            Assert.AreEqual("OLC2", planner.PlannedCourses.ElementAt(3).Code);
            Assert.AreEqual(new DateTime(2017, 1, 3), planner.PlannedCourses.ElementAt(3).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(1, planner.PlannedCourses.ElementAt(3).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(1, planner.PlannedCourses.ElementAt(3).Duration);

            Assert.AreEqual("OLC3", planner.PlannedCourses.ElementAt(4).Code);
            Assert.AreEqual(new DateTime(2017, 1, 5), planner.PlannedCourses.ElementAt(4).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(1, planner.PlannedCourses.ElementAt(4).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(1, planner.PlannedCourses.ElementAt(4).Duration);

            Assert.AreEqual("OLC4", planner.PlannedCourses.ElementAt(5).Code);
            Assert.AreEqual(new DateTime(2017, 1, 9), planner.PlannedCourses.ElementAt(5).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(5, planner.PlannedCourses.ElementAt(5).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(5, planner.PlannedCourses.ElementAt(5).Duration);

            Assert.AreEqual(0, planner.NotPlannedCourses.Count());
        }


        [TestMethod]
        public void PlanThreeCourses_NineCoursesPlanned_ApplyOLCSixTimes_OLCOrderCourses()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 2);


            IEnumerable<Models.Course> coursesToPlan = new List<Models.Course>()
            {
                CreateNewModelCourseWithOneCourseImplementation("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 30) }),
                CreateNewModelCourseWithOneCourseImplementation("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }),
            };

            // Act
            planner.PlanCoursesWithOlc(coursesToPlan);

            // Assert
            Assert.AreEqual(9, planner.PlannedCourses.Count());

            Assert.AreEqual("SCRUMES", planner.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(new DateTime(2017, 1, 30), planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(1, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("ENEST", planner.PlannedCourses.ElementAt(1).Code);
            Assert.AreEqual(new DateTime(2017, 1, 2), planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(3, planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("ENDEVN", planner.PlannedCourses.ElementAt(2).Code);
            Assert.AreEqual(new DateTime(2017, 1, 9), planner.PlannedCourses.ElementAt(2).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(3, planner.PlannedCourses.ElementAt(2).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("OLC1", planner.PlannedCourses.ElementAt(3).Code);
            Assert.AreEqual(new DateTime(2017, 1, 5), planner.PlannedCourses.ElementAt(3).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(2, planner.PlannedCourses.ElementAt(3).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(2, planner.PlannedCourses.ElementAt(3).Duration);

            Assert.AreEqual("OLC2", planner.PlannedCourses.ElementAt(4).Code);
            Assert.AreEqual(new DateTime(2017, 1, 12), planner.PlannedCourses.ElementAt(4).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(2, planner.PlannedCourses.ElementAt(4).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(2, planner.PlannedCourses.ElementAt(4).Duration);

            Assert.AreEqual("OLC3", planner.PlannedCourses.ElementAt(5).Code);
            Assert.AreEqual(new DateTime(2017, 1, 16), planner.PlannedCourses.ElementAt(5).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(5, planner.PlannedCourses.ElementAt(5).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(5, planner.PlannedCourses.ElementAt(5).Duration);

            Assert.AreEqual("OLC4", planner.PlannedCourses.ElementAt(6).Code);
            Assert.AreEqual(new DateTime(2017, 1, 23), planner.PlannedCourses.ElementAt(6).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(5, planner.PlannedCourses.ElementAt(6).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(5, planner.PlannedCourses.ElementAt(6).Duration);

            Assert.AreEqual("OLC5", planner.PlannedCourses.ElementAt(7).Code);
            Assert.AreEqual(new DateTime(2017, 1, 31), planner.PlannedCourses.ElementAt(7).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(4, planner.PlannedCourses.ElementAt(7).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(4, planner.PlannedCourses.ElementAt(7).Duration);

            Assert.AreEqual("OLC6", planner.PlannedCourses.ElementAt(8).Code);
            Assert.AreEqual(new DateTime(2017, 2, 6), planner.PlannedCourses.ElementAt(8).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(1, planner.PlannedCourses.ElementAt(8).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(1, planner.PlannedCourses.ElementAt(8).Duration);

            Assert.AreEqual(0, planner.NotPlannedCourses.Count());
        }
    }
}