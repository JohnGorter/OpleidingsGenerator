using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InfoSupport.KC.OpleidingsplanGenerator.Api;
using InfoSupport.KC.OpleidingsplanGenerator.Api.Managers;
using InfoSupport.KC.OpleidingsplanGenerator.Integration;
using Moq;
using InfoSupport.KC.OpleidingsplanGenerator.Api.Models;
using System.Collections.Generic;
using InfoSupport.KC.OpleidingsplanGenerator.Generator;
using InfoSupport.KC.OpleidingsplanGenerator.Api.Tests.Helpers;
using InfoSupport.KC.OpleidingsplanGenerator.Dal.Mappers;
using InfoSupport.KC.OpleidingsplanGenerator.Models;
using System.Linq;
using System.Collections.ObjectModel;
using InfoSupport.KC.OpleidingsplanGenerator.Dal;

namespace InfoSupport.KC.OpleidingsplanGenerator.Api.Tests.Managers
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
            var courses = new Collection<RestEducationPlanCourse> {
                new RestEducationPlanCourse
                {
                    Code = "2NETARCH"
                },
                new RestEducationPlanCourse
                {
                    Code = "ADCSB"
                },
            };
            var educationPlanConverterMock = new Mock<IEducationPlanConverter>(MockBehavior.Strict);

            var educationPlanDataMapperMock = new Mock<IEducationPlanDataMapper>(MockBehavior.Strict);

            var educationPlanOutputterMock = new Mock<IEducationPlanOutputter>(MockBehavior.Strict);
            educationPlanOutputterMock.Setup(planner => planner.GenerateEducationPlan(It.IsAny<EducationPlanData>())).Returns(GetDummyEducationPlan());

            var plannerMock = new Mock<IPlanner>(MockBehavior.Strict);
            plannerMock.Setup(planner => planner.PlanCoursesWithOlc(It.IsAny<IEnumerable<OpleidingsplanGenerator.Models.Course>>()));
            plannerMock.SetupSet(planner => planner.StartDate = GetDummyRestEducationPlan(courses).InPaymentFrom).Verifiable();
            plannerMock.SetupSet(planner => planner.BlockedDates = It.IsAny<Collection<DateTime>>()).Verifiable();
            plannerMock.SetupGet(planner => planner.AllCourses).Returns(GetDummyGeneratorCourses());

            var courseServiceMock = new Mock<ICourseService>(MockBehavior.Strict);
            courseServiceMock.Setup(service => service.FindCourses(new List<string> { "2NETARCH", "ADCSB" })).Returns(
                new List<Integration.Course>() {
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
            var result = manager.GenerateEducationPlan(educationPlan, null);

            // Assert
            educationPlanOutputterMock.Verify(outputter => outputter.GenerateEducationPlan(It.IsAny<EducationPlanData>()));
            plannerMock.Verify(planner => planner.PlanCoursesWithOlc(It.IsAny<IEnumerable<OpleidingsplanGenerator.Models.Course>>()));
            courseServiceMock.Verify(outputter => outputter.FindCourses(new List<string> { "2NETARCH", "ADCSB" }));
            profileDataMapperMock.Verify(dataMapper => dataMapper.FindById(1));
            plannerMock.VerifySet(planner => planner.StartDate = GetDummyRestEducationPlan(courses).InPaymentFrom);
            plannerMock.VerifySet(planner => planner.BlockedDates = It.IsAny<Collection<DateTime>>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GenerateEducationPlan_null_ExceptionThrowed()
        {
            // Arrange
            var courses = new Collection<RestEducationPlanCourse> {
                new RestEducationPlanCourse
                {
                    Code = "2NETARCH"
                },
                new RestEducationPlanCourse
                {
                    Code = "ADCSB"
                },
            };

            var educationPlanDataMapperMock = new Mock<IEducationPlanDataMapper>(MockBehavior.Strict);
            var educationPlanOutputterMock = new Mock<IEducationPlanOutputter>(MockBehavior.Strict);
            var plannerMock = new Mock<IPlanner>(MockBehavior.Strict);
            var courseServiceMock = new Mock<ICourseService>(MockBehavior.Strict);
            var profileDataMapperMock = new Mock<IDataMapper<CourseProfile>>(MockBehavior.Strict);
            var educationPlanConverterMock = new Mock<IEducationPlanConverter>(MockBehavior.Strict);

            EducationPlanManager manager = new EducationPlanManager(courseServiceMock.Object, plannerMock.Object, educationPlanOutputterMock.Object, profileDataMapperMock.Object, educationPlanDataMapperMock.Object, educationPlanConverterMock.Object);
            RestEducationPlan educationPlan = GetDummyRestEducationPlan(courses);

            // Act
            var result = manager.GenerateEducationPlan(null, null);

            // Assert ArgumentNullException
        }

        [TestMethod]
        public void SaveEducationPlan_Planner_Outputter_Service_DAL_Called()
        {
            // Arrange
            var courses = new Collection<RestEducationPlanCourse> {
                new RestEducationPlanCourse
                {
                    Code = "2NETARCH"
                },
                new RestEducationPlanCourse
                {
                    Code = "ADCSB"
                },
            };
            var educationPlanConverterMock = new Mock<IEducationPlanConverter>(MockBehavior.Strict);

            var educationPlanDataMapperMock = new Mock<IEducationPlanDataMapper>(MockBehavior.Strict);
            educationPlanDataMapperMock.Setup(dataMapper => dataMapper.Insert(It.IsAny<EducationPlan>())).Returns(1);

            var educationPlanOutputterMock = new Mock<IEducationPlanOutputter>(MockBehavior.Strict);
            educationPlanOutputterMock.Setup(planner => planner.GenerateEducationPlan(It.IsAny<EducationPlanData>())).Returns(GetDummyEducationPlan());

            var plannerMock = new Mock<IPlanner>(MockBehavior.Strict);
            plannerMock.Setup(planner => planner.PlanCoursesWithOlc(It.IsAny<IEnumerable<OpleidingsplanGenerator.Models.Course>>()));
            plannerMock.SetupSet(planner => planner.StartDate = GetDummyRestEducationPlan(courses).InPaymentFrom).Verifiable();
            plannerMock.SetupSet(planner => planner.BlockedDates = It.IsAny<Collection<DateTime>>()).Verifiable();
            plannerMock.SetupGet(planner => planner.AllCourses).Returns(GetDummyGeneratorCourses());

            var courseServiceMock = new Mock<ICourseService>(MockBehavior.Strict);
            courseServiceMock.Setup(service => service.FindCourses(new List<string> { "2NETARCH", "ADCSB" })).Returns(
                new List<Integration.Course>() {
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
            plannerMock.Verify(planner => planner.PlanCoursesWithOlc(It.IsAny<IEnumerable<OpleidingsplanGenerator.Models.Course>>()));
            courseServiceMock.Verify(outputter => outputter.FindCourses(new List<string> { "2NETARCH", "ADCSB" }));
            profileDataMapperMock.Verify(dataMapper => dataMapper.FindById(1));
            plannerMock.VerifySet(planner => planner.StartDate = GetDummyRestEducationPlan(courses).InPaymentFrom);
            plannerMock.VerifySet(planner => planner.BlockedDates = It.IsAny<Collection<DateTime>>());
            educationPlanDataMapperMock.Verify(dataMapper => dataMapper.Insert(It.IsAny<EducationPlan>()));
        }

        [TestMethod]
        public void UpdateEducationPlan_Planner_Outputter_Service_DAL_Called()
        {
            // Arrange
            var courses = new Collection<RestEducationPlanCourse> {
                new RestEducationPlanCourse
                {
                    Code = "2NETARCH"
                },
                new RestEducationPlanCourse
                {
                    Code = "ADCSB"
                },
            };
            var educationPlanConverterMock = new Mock<IEducationPlanConverter>(MockBehavior.Strict);

            var educationPlanDataMapperMock = new Mock<IEducationPlanDataMapper>(MockBehavior.Strict);
            educationPlanDataMapperMock.Setup(dataMapper => dataMapper.Update(It.IsAny<EducationPlan>())).Returns(1);
            educationPlanDataMapperMock.Setup(dataMapper => dataMapper.FindById(It.IsAny<long>())).Returns(GetDummyEducationPlan());

            var educationPlanOutputterMock = new Mock<IEducationPlanOutputter>(MockBehavior.Strict);
            educationPlanOutputterMock.Setup(planner => planner.GenerateEducationPlan(It.IsAny<EducationPlanData>())).Returns(GetDummyEducationPlan());

            var plannerMock = new Mock<IPlanner>(MockBehavior.Strict);
            plannerMock.Setup(planner => planner.PlanCoursesWithOlcInOldEducationPlan(It.IsAny<IEnumerable<OpleidingsplanGenerator.Models.Course>>(), It.IsAny<EducationPlan>()));
            plannerMock.SetupSet(planner => planner.StartDate = GetDummyRestEducationPlan(courses).InPaymentFrom).Verifiable();
            plannerMock.SetupSet(planner => planner.BlockedDates = It.IsAny<Collection<DateTime>>()).Verifiable();
            plannerMock.SetupGet(planner => planner.AllCourses).Returns(GetDummyGeneratorCourses());

            var courseServiceMock = new Mock<ICourseService>(MockBehavior.Strict);
            courseServiceMock.Setup(service => service.FindCourses(new List<string> { "2NETARCH", "ADCSB" })).Returns(
                new List<Integration.Course>() {
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
            courseServiceMock.Verify(outputter => outputter.FindCourses(new List<string> { "2NETARCH", "ADCSB" }));
            profileDataMapperMock.Verify(dataMapper => dataMapper.FindById(1));
            plannerMock.Verify(planner => planner.PlanCoursesWithOlcInOldEducationPlan(It.IsAny<IEnumerable<OpleidingsplanGenerator.Models.Course>>(), It.IsAny<EducationPlan>()));
            plannerMock.VerifySet(planner => planner.StartDate = GetDummyRestEducationPlan(courses).InPaymentFrom);
            plannerMock.VerifySet(planner => planner.BlockedDates = It.IsAny<Collection<DateTime>>());
            educationPlanDataMapperMock.Verify(dataMapper => dataMapper.Update(It.IsAny<EducationPlan>()));
            educationPlanDataMapperMock.Verify(dataMapper => dataMapper.FindById(It.IsAny<long>()));
        }

        [TestMethod]
        public void FindByIdEducationPlan_DAL_Called()
        {
            // Arrange
            var courses = new Collection<RestEducationPlanCourse> {
                new RestEducationPlanCourse
                {
                    Code = "2NETARCH"
                },
                new RestEducationPlanCourse
                {
                    Code = "ADCSB"
                },
            };
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
            var courses = new Collection<RestEducationPlanCourse> {
                new RestEducationPlanCourse
                {
                    Code = "2NETARCH"
                },
                new RestEducationPlanCourse
                {
                    Code = "ADCSB"
                },
            };
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
            var courses = new Collection<RestEducationPlanCourse> {
                new RestEducationPlanCourse
                {
                    Code = "2NETARCH"
                },
                new RestEducationPlanCourse
                {
                    Code = "ADCSB"
                },
            };
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

        [TestMethod]
        public void DeleteEducationPlan_DAL_Called()
        {
            // Arrange
            var courses = new Collection<RestEducationPlanCourse> {
                new RestEducationPlanCourse
                {
                    Code = "2NETARCH"
                },
                new RestEducationPlanCourse
                {
                    Code = "ADCSB"
                },
            };
            var educationPlanConverterMock = new Mock<IEducationPlanConverter>(MockBehavior.Strict);

            var educationPlanDataMapperMock = new Mock<IEducationPlanDataMapper>(MockBehavior.Strict);
            educationPlanDataMapperMock.Setup(dataMapper => dataMapper.Delete(1));

            var educationPlanOutputterMock = new Mock<IEducationPlanOutputter>(MockBehavior.Strict);
            var plannerMock = new Mock<IPlanner>(MockBehavior.Strict);
            var courseServiceMock = new Mock<ICourseService>(MockBehavior.Strict);

            var profileDataMapperMock = new Mock<IDataMapper<CourseProfile>>(MockBehavior.Strict);

            EducationPlanManager manager = new EducationPlanManager(courseServiceMock.Object, plannerMock.Object, educationPlanOutputterMock.Object, profileDataMapperMock.Object, educationPlanDataMapperMock.Object, educationPlanConverterMock.Object);

            // Act
            manager.DeleteEducationPlan(1);

            // Assert
            educationPlanDataMapperMock.Verify(dataMapper => dataMapper.Delete(1));
        }

        [TestMethod]
        public void FindAllUpdated_DAL_Called()
        {
            // Arrange
            var educationPlanConverterMock = new Mock<IEducationPlanConverter>(MockBehavior.Strict);

            var educationPlanDataMapperMock = new Mock<IEducationPlanDataMapper>(MockBehavior.Strict);
            educationPlanDataMapperMock.Setup(dataMapper => dataMapper.FindAllUpdated()).Returns(GetDummyEducationPlanCompareList());

            var educationPlanOutputterMock = new Mock<IEducationPlanOutputter>(MockBehavior.Strict);
            var plannerMock = new Mock<IPlanner>(MockBehavior.Strict);
            var courseServiceMock = new Mock<ICourseService>(MockBehavior.Strict);

            var profileDataMapperMock = new Mock<IDataMapper<CourseProfile>>(MockBehavior.Strict);

            EducationPlanManager manager = new EducationPlanManager(courseServiceMock.Object, plannerMock.Object, educationPlanOutputterMock.Object, profileDataMapperMock.Object, educationPlanDataMapperMock.Object, educationPlanConverterMock.Object);

            // Act
            var result = manager.FindAllUpdated();

            // Assert
            educationPlanDataMapperMock.Verify(dataMapper => dataMapper.FindAllUpdated());
        }

    }
}
