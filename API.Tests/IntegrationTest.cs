﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InfoSupport.KC.OpleidingsplanGenerator.Api.Controllers;
using InfoSupport.KC.OpleidingsplanGenerator.Api.Models;
using InfoSupport.KC.OpleidingsplanGenerator.Api.Tests.Helpers;
using InfoSupport.KC.OpleidingsplanGenerator.Api.Managers;
using InfoSupport.KC.OpleidingsplanGenerator.Integration;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using InfoSupport.KC.OpleidingsplanGenerator.Dal;

namespace InfoSupport.KC.OpleidingsplanGenerator.Api.Tests
{
    [TestClass]
    public class IntegrationTest : EducationPlanTestHelper
    {
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            AutoMapperConfiguration.Configure();
        }

        [TestMethod]
        public void GenerateEducationPlan_IntegrationTest()
        {
            // Arrange
            var courses = new Collection<RestEducationPlanCourse> {
                new RestEducationPlanCourse {
                    Code = "2NETARCH"
                }
            };

            RestEducationPlan restEducationPlan = GetDummyRestEducationPlan(courses);
            var courseServiceMock = new Mock<ICourseService>(MockBehavior.Strict);
            courseServiceMock.Setup(service => service.FindCourses(new List<string> { "2NETARCH" })).Returns(
                new List<Integration.Course>() {
                    CreateNewIntegrationCourseWithTwoCourseImplementations("2NETARCH", 1,
                    new Collection<DateTime> { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                    new Collection<DateTime> { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) })
            });
            var dalConfig = DalConfiguration.Configuration;

            IEducationPlanManager manager = new EducationPlanManager(dalConfig.ProfilePath, courseServiceMock.Object, dalConfig.ManagementPropertiesPath, dalConfig.EducationPlanPath, dalConfig.EducationPlanUpdatedPath, dalConfig.ModulePath);

            // Act
            var result = manager.GenerateEducationPlan(restEducationPlan, null);


            // Assert
            courseServiceMock.Verify(service => service.FindCourses(new List<string> { "2NETARCH" }));

            Assert.AreEqual(7, result.PlannedCourses.Count());
            Assert.AreEqual(6, result.NotPlannedCourses.Count());

            Assert.AreEqual("OLC1", result.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(new DateTime(2016, 12, 5), result.PlannedCourses.ElementAt(0).Date);
            Assert.AreEqual(5, result.PlannedCourses.ElementAt(0).Days);

            Assert.AreEqual("OLC2", result.PlannedCourses.ElementAt(1).Code);
            Assert.AreEqual(new DateTime(2016, 12, 12), result.PlannedCourses.ElementAt(1).Date);
            Assert.AreEqual(5, result.PlannedCourses.ElementAt(1).Days);

            Assert.AreEqual("OLC3", result.PlannedCourses.ElementAt(2).Code);
            Assert.AreEqual(new DateTime(2016, 12, 19), result.PlannedCourses.ElementAt(2).Date);
            Assert.AreEqual(5, result.PlannedCourses.ElementAt(2).Days);

            Assert.AreEqual("OLC4", result.PlannedCourses.ElementAt(3).Code);
            Assert.AreEqual(new DateTime(2016, 12, 26), result.PlannedCourses.ElementAt(3).Date);
            Assert.AreEqual(5, result.PlannedCourses.ElementAt(3).Days);

            Assert.AreEqual("2NETARCH", result.PlannedCourses.ElementAt(4).Code);
            Assert.AreEqual(new DateTime(2017, 1, 2), result.PlannedCourses.ElementAt(4).Date);
            Assert.AreEqual(3, result.PlannedCourses.ElementAt(4).Days);

            Assert.AreEqual("OLC5", result.PlannedCourses.ElementAt(5).Code);
            Assert.AreEqual(new DateTime(2017, 1, 5), result.PlannedCourses.ElementAt(5).Date);
            Assert.AreEqual(2, result.PlannedCourses.ElementAt(5).Days);

            Assert.AreEqual("OLC6", result.PlannedCourses.ElementAt(6).Code);
            Assert.AreEqual(new DateTime(2017, 1, 9), result.PlannedCourses.ElementAt(6).Date);
            Assert.AreEqual(3, result.PlannedCourses.ElementAt(6).Days);
        }

        [TestMethod]
        public void GenerateEducationPlan_IntegrationTest_WithoutPriority()
        {
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

            RestEducationPlan restEducationPlan = GetDummyRestEducationPlan(courses);
            restEducationPlan.InPaymentFrom = new DateTime(2017, 3, 5);
            var courseServiceMock = new Mock<ICourseService>(MockBehavior.Strict);
            courseServiceMock.Setup(service => service.FindCourses(new List<string> { "2NETARCH", "ADCSB" })).Returns(
            new List<Integration.Course>() {
                CreateNewIntegrationCourseWithOneCourseImplementation("2NETARCH", 1,
                new Collection<DateTime> { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }),
                CreateNewIntegrationCourseWithOneCourseImplementation("ADCSB", 1,
                new Collection<DateTime> { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }),
            });
            var dalConfig = DalConfiguration.Configuration;

            IEducationPlanManager manager = new EducationPlanManager(dalConfig.ProfilePath, courseServiceMock.Object, dalConfig.ManagementPropertiesPath, dalConfig.EducationPlanPath, dalConfig.EducationPlanUpdatedPath, dalConfig.ModulePath);

            // Act
            var result = manager.GenerateEducationPlan(restEducationPlan, null);

            // Assert
            courseServiceMock.Verify(outputter => outputter.FindCourses(new List<string> { "2NETARCH", "ADCSB" }));

            Assert.AreEqual(3, result.PlannedCourses.Count());
            Assert.AreEqual("2NETARCH", result.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual("OLC1", result.PlannedCourses.ElementAt(1).Code);
            Assert.AreEqual("OLC2", result.PlannedCourses.ElementAt(2).Code);
            Assert.AreEqual(7, result.NotPlannedCourses.Count());
            Assert.AreEqual("ADCSB", result.NotPlannedCourses.ElementAt(0).Code);
        }


        [TestMethod]
        public void GenerateEducationPlan_IntegrationTest_WithPriority()
        {
            var courses = new Collection<RestEducationPlanCourse> {
                new RestEducationPlanCourse
                {
                    Code = "2NETARCH"

                },
                new RestEducationPlanCourse
                {
                    Code = "ADCSB",
                    Priority = -1,
                    Commentary = "Force to plan"
                },
            };

            RestEducationPlan restEducationPlan = GetDummyRestEducationPlan(courses);
            restEducationPlan.InPaymentFrom = new DateTime(2017, 3, 5);
            var courseServiceMock = new Mock<ICourseService>(MockBehavior.Strict);
            courseServiceMock.Setup(service => service.FindCourses(new List<string> { "2NETARCH", "ADCSB" })).Returns(
            new List<Integration.Course>() {
                CreateNewIntegrationCourseWithOneCourseImplementation("2NETARCH", 1,
                new Collection<DateTime> { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }),
                CreateNewIntegrationCourseWithOneCourseImplementation("ADCSB", 1,
                new Collection<DateTime> { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }),
            });
            var dalConfig = DalConfiguration.Configuration;

            IEducationPlanManager manager = new EducationPlanManager(dalConfig.ProfilePath, courseServiceMock.Object, dalConfig.ManagementPropertiesPath, dalConfig.EducationPlanPath, dalConfig.EducationPlanUpdatedPath, dalConfig.ModulePath);

            // Act
            var result = manager.GenerateEducationPlan(restEducationPlan, null);

            // Assert
            courseServiceMock.Verify(outputter => outputter.FindCourses(new List<string> { "2NETARCH", "ADCSB" }));

            Assert.AreEqual(3, result.PlannedCourses.Count());
            Assert.AreEqual("ADCSB", result.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual("Force to plan", result.PlannedCourses.ElementAt(0).Commentary);
            Assert.AreEqual("OLC1", result.PlannedCourses.ElementAt(1).Code);
            Assert.AreEqual("OLC2", result.PlannedCourses.ElementAt(2).Code);
            Assert.AreEqual(7, result.NotPlannedCourses.Count());
            Assert.AreEqual("2NETARCH", result.NotPlannedCourses.ElementAt(0).Code);
        }
    }
}
