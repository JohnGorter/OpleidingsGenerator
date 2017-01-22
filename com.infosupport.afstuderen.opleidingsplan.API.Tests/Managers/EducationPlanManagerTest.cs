using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.infosupport.afstuderen.opleidingsplan.api;
using com.infosupport.afstuderen.opleidingsplan.api.managers;
using com.infosupport.afstuderen.opleidingsplan.integration;
using Moq;
using com.infosupport.afstuderen.opleidingsplan.api.models;
using System.Collections.Generic;
using com.infosupport.afstuderen.opleidingsplan.generator;
using com.infosupport.afstuderen.opleidingsplan.api.tests.helpers;
using com.infosupport.afstuderen.opleidingsplan.dal.mappers;
using com.infosupport.afstuderen.opleidingsplan.models;
using System.Linq;
using System.Collections.ObjectModel;
using com.infosupport.afstuderen.opleidingsplan.dal;

namespace com.infosupport.afstuderen.opleidingsplan.api.tests.managers
{
    [TestClass]
    public class EducationPlanManagerTest : EducationPlanTestHelper
    {
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            AutoMapperConfiguration.Configure();
        }


        [TestMethod]
        public void GenerateEducationPlan_Planner_Outputter_Service_DAL_Called()
        {
            // Arrange
            var courses = new Collection<string> { "2NETARCH", "ADCSB" };
            var educationPlanConverterMock = new Mock<IEducationPlanConverter>(MockBehavior.Strict);

            var educationPlanDataMapperMock = new Mock<IEducationPlanDataMapper>(MockBehavior.Strict);

            var educationPlanOutputterMock = new Mock<IEducationPlanOutputter>(MockBehavior.Strict);
            educationPlanOutputterMock.Setup(planner => planner.GenerateEducationPlan(It.IsAny<EducationPlanData>())).Returns(GetDummyEducationPlan());

            var plannerMock = new Mock<IPlanner>(MockBehavior.Strict);
            plannerMock.Setup(planner => planner.PlanCoursesWithOlc(It.IsAny<IEnumerable<opleidingsplan.models.Course>>()));
            plannerMock.SetupSet(planner => planner.StartDate = GetDummyRestEducationPlan(courses).InPaymentFrom).Verifiable();
            plannerMock.SetupSet(planner => planner.BlockedDates = It.IsAny<Collection<DateTime>>()).Verifiable();

            var courseServiceMock = new Mock<ICourseService>(MockBehavior.Strict);
            courseServiceMock.Setup(service => service.FindCourses(courses)).Returns(
                new List<integration.Course>() {
                    CreateNewIntegrationCourseWithTwoCourseImplementations("2NETARCH", 1,
                    new Collection<DateTime> { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                    new Collection<DateTime> { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) })
            });

            var profileDataMapperMock = new Mock<IDataMapper<CourseProfile>>(MockBehavior.Strict);
            profileDataMapperMock.Setup(dataMapper => dataMapper.FindById(1)).Returns(GetDummyDataProfiles().First());
            var dalConfig = DalConfiguration.Configuration;

            EducationPlanManager manager = new EducationPlanManager(courseServiceMock.Object, plannerMock.Object, educationPlanOutputterMock.Object, profileDataMapperMock.Object, educationPlanDataMapperMock.Object, educationPlanConverterMock.Object);
            RestEducationPlan educationPlan = GetDummyRestEducationPlan(courses);


            // Act
            var result = manager.GenerateEducationPlan(educationPlan);

            // Assert
            educationPlanOutputterMock.Verify(outputter => outputter.GenerateEducationPlan(It.IsAny<EducationPlanData>()));
            plannerMock.Verify(planner => planner.PlanCoursesWithOlc(It.IsAny<IEnumerable<opleidingsplan.models.Course>>()));
            courseServiceMock.Verify(outputter => outputter.FindCourses(courses));
            profileDataMapperMock.Verify(dataMapper => dataMapper.FindById(1));
            plannerMock.VerifySet(planner => planner.StartDate = GetDummyRestEducationPlan(courses).InPaymentFrom);
            plannerMock.VerifySet(planner => planner.BlockedDates = It.IsAny<Collection<DateTime>>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GenerateEducationPlan_null_ExceptionThrowed()
        {
            // Arrange
            Collection<string> courses = new Collection<string> { "2NETARCH", "ADCSB" };

            var educationPlanDataMapperMock = new Mock<IEducationPlanDataMapper>(MockBehavior.Strict);
            var educationPlanOutputterMock = new Mock<IEducationPlanOutputter>(MockBehavior.Strict);
            var plannerMock = new Mock<IPlanner>(MockBehavior.Strict);
            var courseServiceMock = new Mock<ICourseService>(MockBehavior.Strict);
            var profileDataMapperMock = new Mock<IDataMapper<CourseProfile>>(MockBehavior.Strict);
            var educationPlanConverterMock = new Mock<IEducationPlanConverter>(MockBehavior.Strict);

            EducationPlanManager manager = new EducationPlanManager(courseServiceMock.Object, plannerMock.Object, educationPlanOutputterMock.Object, profileDataMapperMock.Object, educationPlanDataMapperMock.Object, educationPlanConverterMock.Object);
            RestEducationPlan educationPlan = GetDummyRestEducationPlan(courses);

            // Act
            var result = manager.GenerateEducationPlan(null);

            // Assert ArgumentNullException
        }

        [TestMethod]
        public void SaveEducationPlan_Planner_Outputter_Service_DAL_Called()
        {
            // Arrange
            var courses = new Collection<string> { "2NETARCH", "ADCSB" };
            var educationPlanConverterMock = new Mock<IEducationPlanConverter>(MockBehavior.Strict);

            var educationPlanDataMapperMock = new Mock<IEducationPlanDataMapper>(MockBehavior.Strict);
            educationPlanDataMapperMock.Setup(dataMapper => dataMapper.Insert(It.IsAny<EducationPlan>())).Returns(1);

            var educationPlanOutputterMock = new Mock<IEducationPlanOutputter>(MockBehavior.Strict);
            educationPlanOutputterMock.Setup(planner => planner.GenerateEducationPlan(It.IsAny<EducationPlanData>())).Returns(GetDummyEducationPlan());

            var plannerMock = new Mock<IPlanner>(MockBehavior.Strict);
            plannerMock.Setup(planner => planner.PlanCoursesWithOlc(It.IsAny<IEnumerable<opleidingsplan.models.Course>>()));
            plannerMock.SetupSet(planner => planner.StartDate = GetDummyRestEducationPlan(courses).InPaymentFrom).Verifiable();
            plannerMock.SetupSet(planner => planner.BlockedDates = It.IsAny<Collection<DateTime>>()).Verifiable();

            var courseServiceMock = new Mock<ICourseService>(MockBehavior.Strict);
            courseServiceMock.Setup(service => service.FindCourses(courses)).Returns(
                new List<integration.Course>() {
                    CreateNewIntegrationCourseWithTwoCourseImplementations("2NETARCH", 1,
                    new Collection<DateTime> { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                    new Collection<DateTime> { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) })
            });

            var profileDataMapperMock = new Mock<IDataMapper<CourseProfile>>(MockBehavior.Strict);
            profileDataMapperMock.Setup(dataMapper => dataMapper.FindById(1)).Returns(GetDummyDataProfiles().First());

            EducationPlanManager manager = new EducationPlanManager(courseServiceMock.Object, plannerMock.Object, educationPlanOutputterMock.Object, profileDataMapperMock.Object, educationPlanDataMapperMock.Object, educationPlanConverterMock.Object);
            RestEducationPlan educationPlan = GetDummyRestEducationPlan(courses);


            // Act
            var result = manager.SaveEducationPlan(educationPlan);

            // Assert
            Assert.AreEqual(1, result);
            educationPlanOutputterMock.Verify(outputter => outputter.GenerateEducationPlan(It.IsAny<EducationPlanData>()));
            plannerMock.Verify(planner => planner.PlanCoursesWithOlc(It.IsAny<IEnumerable<opleidingsplan.models.Course>>()));
            courseServiceMock.Verify(outputter => outputter.FindCourses(courses));
            profileDataMapperMock.Verify(dataMapper => dataMapper.FindById(1));
            plannerMock.VerifySet(planner => planner.StartDate = GetDummyRestEducationPlan(courses).InPaymentFrom);
            plannerMock.VerifySet(planner => planner.BlockedDates = It.IsAny<Collection<DateTime>>());
            educationPlanDataMapperMock.Verify(dataMapper => dataMapper.Insert(It.IsAny<EducationPlan>()));
        }

        [TestMethod]
        public void UpdateEducationPlan_Planner_Outputter_Service_DAL_Called()
        {
            // Arrange
            var courses = new Collection<string> { "2NETARCH", "ADCSB" };
            var educationPlanConverterMock = new Mock<IEducationPlanConverter>(MockBehavior.Strict);

            var educationPlanDataMapperMock = new Mock<IEducationPlanDataMapper>(MockBehavior.Strict);
            educationPlanDataMapperMock.Setup(dataMapper => dataMapper.Update(It.IsAny<EducationPlan>())).Returns(1);

            var educationPlanOutputterMock = new Mock<IEducationPlanOutputter>(MockBehavior.Strict);
            educationPlanOutputterMock.Setup(planner => planner.GenerateEducationPlan(It.IsAny<EducationPlanData>())).Returns(GetDummyEducationPlan());

            var plannerMock = new Mock<IPlanner>(MockBehavior.Strict);
            plannerMock.Setup(planner => planner.PlanCoursesWithOlc(It.IsAny<IEnumerable<opleidingsplan.models.Course>>()));
            plannerMock.SetupSet(planner => planner.StartDate = GetDummyRestEducationPlan(courses).InPaymentFrom).Verifiable();
            plannerMock.SetupSet(planner => planner.BlockedDates = It.IsAny<Collection<DateTime>>()).Verifiable();

            var courseServiceMock = new Mock<ICourseService>(MockBehavior.Strict);
            courseServiceMock.Setup(service => service.FindCourses(courses)).Returns(
                new List<integration.Course>() {
                    CreateNewIntegrationCourseWithTwoCourseImplementations("2NETARCH", 1,
                    new Collection<DateTime> { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                    new Collection<DateTime> { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) })
            });

            var profileDataMapperMock = new Mock<IDataMapper<CourseProfile>>(MockBehavior.Strict);
            profileDataMapperMock.Setup(dataMapper => dataMapper.FindById(1)).Returns(GetDummyDataProfiles().First());

            EducationPlanManager manager = new EducationPlanManager(courseServiceMock.Object, plannerMock.Object, educationPlanOutputterMock.Object, profileDataMapperMock.Object, educationPlanDataMapperMock.Object, educationPlanConverterMock.Object);
            RestEducationPlan educationPlan = GetDummyRestEducationPlan(courses);


            // Act
            var result = manager.UpdateEducationPlan(educationPlan);

            // Assert
            Assert.AreEqual(1, result);
            educationPlanOutputterMock.Verify(outputter => outputter.GenerateEducationPlan(It.IsAny<EducationPlanData>()));
            plannerMock.Verify(planner => planner.PlanCoursesWithOlc(It.IsAny<IEnumerable<opleidingsplan.models.Course>>()));
            courseServiceMock.Verify(outputter => outputter.FindCourses(courses));
            profileDataMapperMock.Verify(dataMapper => dataMapper.FindById(1));
            plannerMock.VerifySet(planner => planner.StartDate = GetDummyRestEducationPlan(courses).InPaymentFrom);
            plannerMock.VerifySet(planner => planner.BlockedDates = It.IsAny<Collection<DateTime>>());
            educationPlanDataMapperMock.Verify(dataMapper => dataMapper.Update(It.IsAny<EducationPlan>()));
        }

        [TestMethod]
        public void FindByIdEducationPlan_DAL_Called()
        {
            // Arrange
            var courses = new Collection<string> { "2NETARCH", "ADCSB" };
            var educationPlanConverterMock = new Mock<IEducationPlanConverter>(MockBehavior.Strict);

            var educationPlanDataMapperMock = new Mock<IEducationPlanDataMapper>(MockBehavior.Strict);
            educationPlanDataMapperMock.Setup(dataMapper => dataMapper.FindById(1)).Returns(GetDummyEducationPlan());

            var educationPlanOutputterMock = new Mock<IEducationPlanOutputter>(MockBehavior.Strict);
            var plannerMock = new Mock<IPlanner>(MockBehavior.Strict);
            var courseServiceMock = new Mock<ICourseService>(MockBehavior.Strict);
        
            var profileDataMapperMock = new Mock<IDataMapper<CourseProfile>>(MockBehavior.Strict);

            EducationPlanManager manager = new EducationPlanManager(courseServiceMock.Object, plannerMock.Object, educationPlanOutputterMock.Object, profileDataMapperMock.Object, educationPlanDataMapperMock.Object, educationPlanConverterMock.Object);

            // Act
            manager.FindEducationPlan(1);

            // Assert
            educationPlanDataMapperMock.Verify(dataMapper => dataMapper.FindById(1));
        }

        [TestMethod]
        public void FindEducationPlans_DAL_Called()
        {
            // Arrange
            var courses = new Collection<string> { "2NETARCH", "ADCSB" };
            var educationPlanConverterMock = new Mock<IEducationPlanConverter>(MockBehavior.Strict);

            var educationPlanDataMapperMock = new Mock<IEducationPlanDataMapper>(MockBehavior.Strict);
            educationPlanDataMapperMock.Setup(dataMapper => dataMapper.Find(It.IsAny<Func<EducationPlan, bool>>())).Returns(new List<EducationPlan>() { GetDummyEducationPlan() });

            var educationPlanOutputterMock = new Mock<IEducationPlanOutputter>(MockBehavior.Strict);
            var plannerMock = new Mock<IPlanner>(MockBehavior.Strict);
            var courseServiceMock = new Mock<ICourseService>(MockBehavior.Strict);

            var profileDataMapperMock = new Mock<IDataMapper<CourseProfile>>(MockBehavior.Strict);

            EducationPlanManager manager = new EducationPlanManager(courseServiceMock.Object, plannerMock.Object, educationPlanOutputterMock.Object, profileDataMapperMock.Object, educationPlanDataMapperMock.Object, educationPlanConverterMock.Object);

            // Act
            manager.FindEducationPlans(new EducationPlanSearch());

            // Assert
            educationPlanDataMapperMock.Verify(dataMapper => dataMapper.Find(It.IsAny<Func<EducationPlan, bool>>()));
        }

        [TestMethod]
        public void GenerateWordFile_EducationPlanConverter_Called()
        {
            // Arrange
            var courses = new Collection<string> { "2NETARCH", "ADCSB" };

            var educationPlanDataMapperMock = new Mock<IEducationPlanDataMapper>(MockBehavior.Strict);

            var educationPlanConverterMock = new Mock<IEducationPlanConverter>(MockBehavior.Strict);
            educationPlanConverterMock.Setup(converter => converter.GenerateWord(It.IsAny<EducationPlan>())).Returns("path");

            var educationPlanOutputterMock = new Mock<IEducationPlanOutputter>(MockBehavior.Strict);
            var plannerMock = new Mock<IPlanner>(MockBehavior.Strict);
            var courseServiceMock = new Mock<ICourseService>(MockBehavior.Strict);

            var profileDataMapperMock = new Mock<IDataMapper<CourseProfile>>(MockBehavior.Strict);

            EducationPlanManager manager = new EducationPlanManager(courseServiceMock.Object, plannerMock.Object, educationPlanOutputterMock.Object, profileDataMapperMock.Object, educationPlanDataMapperMock.Object, educationPlanConverterMock.Object);

            // Act
            var result = manager.GenerateWordFile(GetDummyEducationPlan());

            // Assert
            Assert.AreEqual("path", result);
            educationPlanConverterMock.Verify(converter => converter.GenerateWord(It.IsAny<EducationPlan>()));
        }

    }
}
