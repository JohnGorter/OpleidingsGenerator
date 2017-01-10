using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.infosupport.afstuderen.opleidingsplan.generator.tests.helpers;
using System.Collections.Generic;
using System.Linq;
using com.infosupport.afstuderen.opleidingsplan.dal.mappers;
using Moq;
using System.Collections.ObjectModel;

namespace com.infosupport.afstuderen.opleidingsplan.generator.tests
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
            planner.PlanCoursesWithOLC(coursesToPlan);
            
            // Assert
            Assert.AreEqual(7, planner.GetPlannedCourses().Count());
            Assert.AreEqual("SCRUMES", planner.GetPlannedCourses().ElementAt(0).Code);
            Assert.AreEqual(new DateTime(2017, 1, 2), planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(3, planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("ENEST", planner.GetPlannedCourses().ElementAt(1).Code);
            Assert.AreEqual(new DateTime(2017, 1, 9), planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(3, planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("ENDEVN", planner.GetPlannedCourses().ElementAt(2).Code);
            Assert.AreEqual(new DateTime(2017, 1, 16), planner.GetPlannedCourses().ElementAt(2).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(3, planner.GetPlannedCourses().ElementAt(2).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("OLC", planner.GetPlannedCourses().ElementAt(3).Code);
            Assert.AreEqual(new DateTime(2017, 1, 5), planner.GetPlannedCourses().ElementAt(3).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(2, planner.GetPlannedCourses().ElementAt(3).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(2, planner.GetPlannedCourses().ElementAt(3).Duration);

            Assert.AreEqual("OLC", planner.GetPlannedCourses().ElementAt(4).Code);
            Assert.AreEqual(new DateTime(2017, 1, 12), planner.GetPlannedCourses().ElementAt(4).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(2, planner.GetPlannedCourses().ElementAt(4).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(2, planner.GetPlannedCourses().ElementAt(4).Duration);

            Assert.AreEqual("OLC", planner.GetPlannedCourses().ElementAt(5).Code);
            Assert.AreEqual(new DateTime(2017, 1, 19), planner.GetPlannedCourses().ElementAt(5).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(2, planner.GetPlannedCourses().ElementAt(5).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(2, planner.GetPlannedCourses().ElementAt(5).Duration);

            Assert.AreEqual("OLC", planner.GetPlannedCourses().ElementAt(6).Code);
            Assert.AreEqual(new DateTime(2017, 1, 23), planner.GetPlannedCourses().ElementAt(6).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(3, planner.GetPlannedCourses().ElementAt(6).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(3, planner.GetPlannedCourses().ElementAt(6).Duration);

            Assert.AreEqual(0, planner.GetNotPlannedCourses().Count());
        }

        [TestMethod]
        public void PlanThreeCourses_SixCoursesPlanned_ApplyOLCThreeTimes_TwoCoursesOneWeek()
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
                new DateTime[] { new DateTime(2017, 1, 6) }),
                CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }),
            };

            // Act
            planner.PlanCoursesWithOLC(coursesToPlan);

            // Assert
            Assert.AreEqual(6, planner.GetPlannedCourses().Count());
            Assert.AreEqual("SCRUMES", planner.GetPlannedCourses().ElementAt(0).Code);
            Assert.AreEqual(new DateTime(2017, 1, 2), planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(3, planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("ENEST", planner.GetPlannedCourses().ElementAt(1).Code);
            Assert.AreEqual(new DateTime(2017, 1, 6), planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(1, planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("ENDEVN", planner.GetPlannedCourses().ElementAt(2).Code);
            Assert.AreEqual(new DateTime(2017, 1, 9), planner.GetPlannedCourses().ElementAt(2).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(3, planner.GetPlannedCourses().ElementAt(2).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("OLC", planner.GetPlannedCourses().ElementAt(3).Code);
            Assert.AreEqual(new DateTime(2017, 1, 5), planner.GetPlannedCourses().ElementAt(3).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(1, planner.GetPlannedCourses().ElementAt(3).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(1, planner.GetPlannedCourses().ElementAt(3).Duration);

            Assert.AreEqual("OLC", planner.GetPlannedCourses().ElementAt(4).Code);
            Assert.AreEqual(new DateTime(2017, 1, 12), planner.GetPlannedCourses().ElementAt(4).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(2, planner.GetPlannedCourses().ElementAt(4).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(2, planner.GetPlannedCourses().ElementAt(4).Duration);

            Assert.AreEqual("OLC", planner.GetPlannedCourses().ElementAt(5).Code);
            Assert.AreEqual(new DateTime(2017, 1, 16), planner.GetPlannedCourses().ElementAt(5).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(3, planner.GetPlannedCourses().ElementAt(5).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(3, planner.GetPlannedCourses().ElementAt(5).Duration);

            Assert.AreEqual(0, planner.GetNotPlannedCourses().Count());
        }

        [TestMethod]
        public void PlanThreeCourses_SixCoursesPlanned_ApplyOLCThreeTimes_ThreeCoursesOneWeek()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 1);

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithOneCourseImplementation("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2) }),
                CreateNewModelCourseWithOneCourseImplementation("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 4) }),
                CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 6) }),
            };

            // Act
            planner.PlanCoursesWithOLC(coursesToPlan);

            // Assert
            Assert.AreEqual(6, planner.GetPlannedCourses().Count());
            Assert.AreEqual("SCRUMES", planner.GetPlannedCourses().ElementAt(0).Code);
            Assert.AreEqual(new DateTime(2017, 1, 2), planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(1, planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("ENEST", planner.GetPlannedCourses().ElementAt(1).Code);
            Assert.AreEqual(new DateTime(2017, 1, 4), planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(1, planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("ENDEVN", planner.GetPlannedCourses().ElementAt(2).Code);
            Assert.AreEqual(new DateTime(2017, 1, 6), planner.GetPlannedCourses().ElementAt(2).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(1, planner.GetPlannedCourses().ElementAt(2).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("OLC", planner.GetPlannedCourses().ElementAt(3).Code);
            Assert.AreEqual(new DateTime(2017, 1, 3), planner.GetPlannedCourses().ElementAt(3).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(1, planner.GetPlannedCourses().ElementAt(3).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(1, planner.GetPlannedCourses().ElementAt(3).Duration);

            Assert.AreEqual("OLC", planner.GetPlannedCourses().ElementAt(4).Code);
            Assert.AreEqual(new DateTime(2017, 1, 5), planner.GetPlannedCourses().ElementAt(4).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(1, planner.GetPlannedCourses().ElementAt(4).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(1, planner.GetPlannedCourses().ElementAt(4).Duration);

            Assert.AreEqual("OLC", planner.GetPlannedCourses().ElementAt(5).Code);
            Assert.AreEqual(new DateTime(2017, 1, 9), planner.GetPlannedCourses().ElementAt(5).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(5, planner.GetPlannedCourses().ElementAt(5).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(5, planner.GetPlannedCourses().ElementAt(5).Duration);

            Assert.AreEqual(0, planner.GetNotPlannedCourses().Count());
        }

        [TestMethod]
        public void PlanTwoCourses_SixCoursesPlanned_ApplyOLCFourTimes_OLCWithWeekend()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2016, 12, 26);

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithOneCourseImplementation("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewModelCourseWithOneCourseImplementation("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 11), new DateTime(2017, 1, 12), new DateTime(2017, 1, 13) }),
            };

            // Act
            planner.PlanCoursesWithOLC(coursesToPlan);

            // Assert
            Assert.AreEqual(6, planner.GetPlannedCourses().Count());
            Assert.AreEqual("SCRUMES", planner.GetPlannedCourses().ElementAt(0).Code);
            Assert.AreEqual(new DateTime(2017, 1, 2), planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(3, planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("ENEST", planner.GetPlannedCourses().ElementAt(1).Code);
            Assert.AreEqual(new DateTime(2017, 1, 11), planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(3, planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("OLC", planner.GetPlannedCourses().ElementAt(2).Code);
            Assert.AreEqual(new DateTime(2016, 12, 26), planner.GetPlannedCourses().ElementAt(2).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(5, planner.GetPlannedCourses().ElementAt(2).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(5, planner.GetPlannedCourses().ElementAt(2).Duration);

            Assert.AreEqual("OLC", planner.GetPlannedCourses().ElementAt(3).Code);
            Assert.AreEqual(new DateTime(2017, 1, 5), planner.GetPlannedCourses().ElementAt(3).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(2, planner.GetPlannedCourses().ElementAt(3).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(2, planner.GetPlannedCourses().ElementAt(3).Duration);

            Assert.AreEqual("OLC", planner.GetPlannedCourses().ElementAt(4).Code);
            Assert.AreEqual(new DateTime(2017, 1, 9), planner.GetPlannedCourses().ElementAt(4).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(2, planner.GetPlannedCourses().ElementAt(4).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(2, planner.GetPlannedCourses().ElementAt(4).Duration);

            Assert.AreEqual("OLC", planner.GetPlannedCourses().ElementAt(5).Code);
            Assert.AreEqual(new DateTime(2017, 1, 16), planner.GetPlannedCourses().ElementAt(5).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(5, planner.GetPlannedCourses().ElementAt(5).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(5, planner.GetPlannedCourses().ElementAt(5).Duration);

            Assert.AreEqual(0, planner.GetNotPlannedCourses().Count());
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

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithOneCourseImplementation("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2) }),
                CreateNewModelCourseWithOneCourseImplementation("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 6) }),
            };

            // Act
            planner.PlanCoursesWithOLC(coursesToPlan);

            // Assert
            Assert.AreEqual(6, planner.GetPlannedCourses().Count());
            Assert.AreEqual("SCRUMES", planner.GetPlannedCourses().ElementAt(0).Code);
            Assert.AreEqual(new DateTime(2017, 1, 2), planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(1, planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("ENEST", planner.GetPlannedCourses().ElementAt(1).Code);
            Assert.AreEqual(new DateTime(2017, 1, 6), planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(1, planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("OLC", planner.GetPlannedCourses().ElementAt(2).Code);
            Assert.AreEqual(new DateTime(2016, 12, 26), planner.GetPlannedCourses().ElementAt(2).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(5, planner.GetPlannedCourses().ElementAt(2).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(5, planner.GetPlannedCourses().ElementAt(2).Duration);

            Assert.AreEqual("OLC", planner.GetPlannedCourses().ElementAt(3).Code);
            Assert.AreEqual(new DateTime(2017, 1, 3), planner.GetPlannedCourses().ElementAt(3).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(1, planner.GetPlannedCourses().ElementAt(3).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(1, planner.GetPlannedCourses().ElementAt(3).Duration);

            Assert.AreEqual("OLC", planner.GetPlannedCourses().ElementAt(4).Code);
            Assert.AreEqual(new DateTime(2017, 1, 5), planner.GetPlannedCourses().ElementAt(4).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(1, planner.GetPlannedCourses().ElementAt(4).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(1, planner.GetPlannedCourses().ElementAt(4).Duration);

            Assert.AreEqual("OLC", planner.GetPlannedCourses().ElementAt(5).Code);
            Assert.AreEqual(new DateTime(2017, 1, 9), planner.GetPlannedCourses().ElementAt(5).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(5, planner.GetPlannedCourses().ElementAt(5).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(5, planner.GetPlannedCourses().ElementAt(5).Duration);

            Assert.AreEqual(0, planner.GetNotPlannedCourses().Count());
        }


        [TestMethod]
        public void PlanThreeCourses_NineCoursesPlanned_ApplyOLCSixTimes_OLCOrderCourses()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 2);


            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithOneCourseImplementation("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 30) }),
                CreateNewModelCourseWithOneCourseImplementation("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }),
            };

            // Act
            planner.PlanCoursesWithOLC(coursesToPlan);

            // Assert
            Assert.AreEqual(9, planner.GetPlannedCourses().Count());

            Assert.AreEqual("SCRUMES", planner.GetPlannedCourses().ElementAt(0).Code);
            Assert.AreEqual(new DateTime(2017, 1, 30), planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(1, planner.GetPlannedCourses().ElementAt(0).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("ENEST", planner.GetPlannedCourses().ElementAt(1).Code);
            Assert.AreEqual(new DateTime(2017, 1, 2), planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(3, planner.GetPlannedCourses().ElementAt(1).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("ENDEVN", planner.GetPlannedCourses().ElementAt(2).Code);
            Assert.AreEqual(new DateTime(2017, 1, 9), planner.GetPlannedCourses().ElementAt(2).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(3, planner.GetPlannedCourses().ElementAt(2).CourseImplementations.ElementAt(0).Days.Count());

            Assert.AreEqual("OLC", planner.GetPlannedCourses().ElementAt(3).Code);
            Assert.AreEqual(new DateTime(2017, 1, 5), planner.GetPlannedCourses().ElementAt(3).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(2, planner.GetPlannedCourses().ElementAt(3).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(2, planner.GetPlannedCourses().ElementAt(3).Duration);

            Assert.AreEqual("OLC", planner.GetPlannedCourses().ElementAt(4).Code);
            Assert.AreEqual(new DateTime(2017, 1, 12), planner.GetPlannedCourses().ElementAt(4).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(2, planner.GetPlannedCourses().ElementAt(4).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(2, planner.GetPlannedCourses().ElementAt(4).Duration);

            Assert.AreEqual("OLC", planner.GetPlannedCourses().ElementAt(5).Code);
            Assert.AreEqual(new DateTime(2017, 1, 16), planner.GetPlannedCourses().ElementAt(5).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(5, planner.GetPlannedCourses().ElementAt(5).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(5, planner.GetPlannedCourses().ElementAt(5).Duration);

            Assert.AreEqual("OLC", planner.GetPlannedCourses().ElementAt(6).Code);
            Assert.AreEqual(new DateTime(2017, 1, 23), planner.GetPlannedCourses().ElementAt(6).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(5, planner.GetPlannedCourses().ElementAt(6).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(5, planner.GetPlannedCourses().ElementAt(6).Duration);

            Assert.AreEqual("OLC", planner.GetPlannedCourses().ElementAt(7).Code);
            Assert.AreEqual(new DateTime(2017, 1, 31), planner.GetPlannedCourses().ElementAt(7).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(4, planner.GetPlannedCourses().ElementAt(7).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(4, planner.GetPlannedCourses().ElementAt(7).Duration);

            Assert.AreEqual("OLC", planner.GetPlannedCourses().ElementAt(8).Code);
            Assert.AreEqual(new DateTime(2017, 2, 6), planner.GetPlannedCourses().ElementAt(8).CourseImplementations.ElementAt(0).StartDay);
            Assert.AreEqual(1, planner.GetPlannedCourses().ElementAt(8).CourseImplementations.ElementAt(0).Days.Count());
            Assert.AreEqual(1, planner.GetPlannedCourses().ElementAt(8).Duration);

            Assert.AreEqual(0, planner.GetNotPlannedCourses().Count());
        }
    }
}
