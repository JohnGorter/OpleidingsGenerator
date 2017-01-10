using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using com.infosupport.afstuderen.opleidingsplan.generator.tests.helpers;
using com.infosupport.afstuderen.opleidingsplan.models;
using com.infosupport.afstuderen.opleidingsplan.dal.mappers;
using Moq;

namespace com.infosupport.afstuderen.opleidingsplan.generator.tests
{
    [TestClass]
    public class PlannerWithEdgeCasesTests : CourseTestHelper
    {
        [TestMethod]
        public void PlanThreeCourses_TwoCoursesPlanned()
        {
            //Arrange
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
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3)},
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7)}),
                CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4)}),

            };

            //Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(2, planner.PlannedCourses.Count());
            Assert.AreEqual(1, planner.NotPlannedCourses.Count());

            Assert.AreEqual("SCRUMES", planner.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.UNPLANNABLE, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENEST", planner.PlannedCourses.ElementAt(1).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENDEVN", planner.NotPlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.NotPlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
        }

        [TestMethod]
        public void PlanFourCourses_FourCoursesPlanned()
        {
            //Arrange
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
            Assert.AreEqual(2, planner.PlannedCourses.Count());

            Assert.AreEqual("SCRUMES", planner.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.UNPLANNABLE, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENEST", planner.PlannedCourses.ElementAt(1).Code);
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.UNPLANNABLE, planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(1).Status);

            Assert.AreEqual(2, planner.NotPlannedCourses.Count());
            Assert.AreEqual("ENDEVN", planner.NotPlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.NotPlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("SECDEV", planner.NotPlannedCourses.ElementAt(1).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.NotPlannedCourses.ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.UNPLANNABLE, planner.NotPlannedCourses.ElementAt(1).CourseImplementations.ElementAt(1).Status);
        }

       
        [TestMethod]
        public void PlanFourCourses_ThreeCoursesPlanned()
        {
            //Arrange
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
            Assert.AreEqual(3, planner.PlannedCourses.Count());
            Assert.AreEqual(1, planner.NotPlannedCourses.Count());

            Assert.AreEqual("SCRUMES", planner.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENEST", planner.PlannedCourses.ElementAt(1).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENDEVN", planner.PlannedCourses.ElementAt(2).Code);
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(2).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("SECDEV", planner.NotPlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.NotPlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
        }

        [TestMethod]
        public void PlanFiveCourses_ThreeCoursesPlanned()
        {
            //Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 1);

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
            Assert.AreEqual(3, planner.PlannedCourses.Count());
            Assert.AreEqual(2, planner.NotPlannedCourses.Count());

            Assert.AreEqual("SCRUMES", planner.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.UNPLANNABLE, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual(Status.UNPLANNABLE, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(2).Status);
            Assert.AreEqual("ENEST", planner.PlannedCourses.ElementAt(1).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("WINDEV", planner.PlannedCourses.ElementAt(2).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.PlannedCourses.ElementAt(2).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(2).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ADCSB", planner.NotPlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.NotPlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("SECDEV", planner.NotPlannedCourses.ElementAt(1).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.NotPlannedCourses.ElementAt(1).CourseImplementations.ElementAt(0).Status);
        }


        
        [TestMethod]
        public void PlanFiveCourses_ThreeCoursesPlanned_Matrix()
        {
            //Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 1);

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
            Assert.AreEqual(3, planner.PlannedCourses.Count());
            Assert.AreEqual(1, planner.NotPlannedCourses.Count());

            Assert.AreEqual("SCRUMES", planner.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.UNPLANNABLE, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual(Status.UNPLANNABLE, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(2).Status);
            Assert.AreEqual("ENEST", planner.PlannedCourses.ElementAt(1).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual(Status.UNPLANNABLE, planner.PlannedCourses.ElementAt(1).CourseImplementations.ElementAt(2).Status);
            Assert.AreEqual("WINDEV", planner.PlannedCourses.ElementAt(2).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.PlannedCourses.ElementAt(2).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.UNPLANNABLE, planner.PlannedCourses.ElementAt(2).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(2).CourseImplementations.ElementAt(2).Status);
            Assert.AreEqual("ADCSB", planner.NotPlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.NotPlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);

        }

        [TestMethod]
        public void PlanTwoCourses_StartDayOfFirstImplementationBeforeStartDate_SecondCourseUnplannable_OneCoursesPlanned()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 1);

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithOneCourseImplementation("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }),
                CreateNewModelCourseWithTwoCourseImplementations("ENEST", 1,
                new DateTime[] { new DateTime(2016, 12, 26), new DateTime(2016, 12, 27), new DateTime(2016, 12, 28) },
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }),
            };

            // Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(1, planner.PlannedCourses.Count());
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual("SCRUMES", planner.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.NotPlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.UNPLANNABLE, planner.NotPlannedCourses.ElementAt(0).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual("ENEST", planner.NotPlannedCourses.ElementAt(0).Code);

            Assert.AreEqual(1, planner.NotPlannedCourses.Count());
        }

        [TestMethod]
        public void PlanOneCourses_StartDayOfFirstImplementationBeforeStartDate_OneCoursesPlanned()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 1);

            IEnumerable<models.Course> coursesToPlan = new List<models.Course>()
            {
                CreateNewModelCourseWithTreeCourseImplementations("SCRUMES", 1,
                new DateTime[] { new DateTime(2016, 1, 26), new DateTime(2016, 1, 27), new DateTime(2016, 1, 28) },
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) },
                new DateTime[] { new DateTime(2017, 2, 20), new DateTime(2017, 2, 21), new DateTime(2017, 2, 22) }),
            };

            // Act
            planner.PlanCourses(coursesToPlan);

            // Assert
            Assert.AreEqual(1, planner.PlannedCourses.Count());
            Assert.AreEqual("SCRUMES", planner.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(Status.UNPLANNABLE, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(1).Status);
            Assert.AreEqual(Status.NOTPLANNED, planner.PlannedCourses.ElementAt(0).CourseImplementations.ElementAt(2).Status);

            Assert.AreEqual(0, planner.NotPlannedCourses.Count());
        }
    }
}
