﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using InfoSupport.KC.OpleidingsplanGenerator.Generator.Tests.Helpers;
using InfoSupport.KC.OpleidingsplanGenerator.Dal.Mappers;
using Moq;
using InfoSupport.KC.OpleidingsplanGenerator.Models;

namespace InfoSupport.KC.OpleidingsplanGenerator.Generator.Tests
{
    [TestClass]
    public class EducationPlanOutputterTest : CourseTestHelper
    {

        [TestMethod]
        public void GenerateEducationPlan_FourCoursesPlanned()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 1);

            IEnumerable<Models.Course> coursesToPlan = new List<Models.Course>()
            {
                CreateNewModelCourseWithOneCourseImplementation("SCRUMES", 1, new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewModelCourseWithOneCourseImplementation("ENEST", 1, new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5), new DateTime(2017, 1, 6) }),
                CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1, new DateTime[] { new DateTime(2017, 1, 6) }),
                CreateNewModelCourseWithOneCourseImplementation("SECDEV", 1, new DateTime[] { new DateTime(2017, 1, 16), new DateTime(2017, 1, 17) }),
                CreateNewModelCourseWithOneCourseImplementation("XSD", 1, new DateTime[] { new DateTime(2017, 1, 18) }),
                CreateNewModelCourseWithOneCourseImplementation("MVC", 1, new DateTime[] { new DateTime(2017, 1, 17), new DateTime(2017, 1, 18) }),
            };

            planner.PlanCourses(coursesToPlan);


            EducationPlanOutputter outputter = new EducationPlanOutputter(planner, managementPropertiesDataMapperMock.Object);
            EducationPlanData data = GetDummyEducationPlanData();

            // Act
            var result = outputter.GenerateEducationPlan(data);

            // Assert
            Assert.AreEqual(4, result.PlannedCourses.Count());
            Assert.AreEqual("SCRUMES", result.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual("ENDEVN", result.PlannedCourses.ElementAt(1).Code);
            Assert.AreEqual("SECDEV", result.PlannedCourses.ElementAt(2).Code);
            Assert.AreEqual("XSD", result.PlannedCourses.ElementAt(3).Code);
            Assert.AreEqual(2, result.NotPlannedCourses.Count());
            Assert.AreEqual("ENEST", result.NotPlannedCourses.ElementAt(0).Code);
            Assert.AreEqual("MVC", result.NotPlannedCourses.ElementAt(1).Code);

            Assert.AreEqual(DateTime.Now.Date, result.Created.Date);
            Assert.AreEqual(new DateTime(2016, 12, 5), result.InPaymentFrom);
            Assert.AreEqual(new DateTime(2017, 1, 26), result.EmployableFrom);
            Assert.AreEqual("NET_Developer", result.Profile);
            Assert.AreEqual("Pim Verheij", result.NameEmployee);
            Assert.AreEqual("Felix Sedney", result.NameTeacher);
            Assert.AreEqual("MVC, DPAT, OOUML, SCRUMES", result.KnowledgeOf);
        }

        [TestMethod]
        public void GenerateEducationPlan_NoCoursePlanned()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            EducationPlanData data = GetDummyEducationPlanData();
            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = data.InPaymentFrom;

            IEnumerable<Models.Course> coursesToPlan = new List<Models.Course>();

            planner.PlanCourses(coursesToPlan);

            
            EducationPlanOutputter outputter = new EducationPlanOutputter(planner, managementPropertiesDataMapperMock.Object);

            // Act
            var result = outputter.GenerateEducationPlan(data);

            // Assert
            Assert.AreEqual(0, result.PlannedCourses.Count());

            Assert.AreEqual(DateTime.Now.Date, result.Created.Date);
            Assert.AreEqual(new DateTime(2016, 12, 5), result.InPaymentFrom);
            Assert.AreEqual(new DateTime(2016, 12, 12), result.EmployableFrom);
            Assert.AreEqual("NET_Developer", result.Profile);
            Assert.AreEqual("Pim Verheij", result.NameEmployee);
            Assert.AreEqual("Felix Sedney", result.NameTeacher);
            Assert.AreEqual("MVC, DPAT, OOUML, SCRUMES", result.KnowledgeOf);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GenerateEducationPlan_null_ExceptionThrowed()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            EducationPlanData data = GetDummyEducationPlanData();
            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = data.InPaymentFrom;

            IEnumerable<Models.Course> coursesToPlan = new List<Models.Course>();

            planner.PlanCourses(coursesToPlan);


            EducationPlanOutputter outputter = new EducationPlanOutputter(planner, managementPropertiesDataMapperMock.Object);

            // Act
            var result = outputter.GenerateEducationPlan(null);

            // Assert ArgumentNullException
        }

        [TestMethod]
        public void GenerateEducationPlan_TwoCoursesJustBeforeStartDate()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 5);

            IEnumerable<Models.Course> coursesToPlan = new List<Models.Course>()
            {
                CreateNewModelCourseWithTwoCourseImplementations("SCRUMES", 1,new DateTime[] { new DateTime(2016, 1, 2), new DateTime(2016, 1, 3), new DateTime(2016, 1, 4) }, new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewModelCourseWithOneCourseImplementation("ENEST", 1, new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5), new DateTime(2017, 1, 6) }),
                CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1, new DateTime[] { new DateTime(2017, 1, 6) }),
            };

            planner.PlanCourses(coursesToPlan);

            EducationPlanOutputter outputter = new EducationPlanOutputter(planner, managementPropertiesDataMapperMock.Object);
            EducationPlanData data = GetDummyEducationPlanData();

            // Act
            var result = outputter.GenerateEducationPlan(data);

            // Assert
            Assert.AreEqual(1, result.PlannedCourses.Count());
            Assert.AreEqual("ENDEVN", result.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(2, result.NotPlannedCourses.Count());
            Assert.AreEqual("SCRUMES", result.NotPlannedCourses.ElementAt(0).Code);
            Assert.AreEqual("ENEST", result.NotPlannedCourses.ElementAt(1).Code);
            Assert.AreEqual(2, result.CoursesJustBeforeStart.Count());
            Assert.AreEqual("SCRUMES", result.CoursesJustBeforeStart.ElementAt(0).Code);
            Assert.AreEqual("ENEST", result.CoursesJustBeforeStart.ElementAt(1).Code);
            Assert.AreEqual(new DateTime(2017, 1, 2), result.CoursesJustBeforeStart.ElementAt(0).Date);
            Assert.AreEqual(new DateTime(2017, 1, 4), result.CoursesJustBeforeStart.ElementAt(1).Date);
        }

        [TestMethod]
        public void GenerateEducationPlan_OneCoursesJustAfterEducationPlanPeriod()
        {
            // Arrange
            var managementPropertiesDataMapperMock = new Mock<IManagementPropertiesDataMapper>(MockBehavior.Strict);
            managementPropertiesDataMapperMock.Setup(dataMapper => dataMapper.FindManagementProperties()).Returns(GetDummyDataManagementProperties());

            Planner planner = new Planner(managementPropertiesDataMapperMock.Object);
            planner.StartDate = new DateTime(2017, 1, 5);

            IEnumerable<Models.Course> coursesToPlan = new List<Models.Course>()
            {
                CreateNewModelCourseWithOneCourseImplementation("SCRUMES", 1, new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewModelCourseWithOneCourseImplementation("ENEST", 1, new DateTime[] { new DateTime(2017, 5, 15), new DateTime(2017, 5, 16), new DateTime(2017, 5, 17) }),
                CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1, new DateTime[] { new DateTime(2017, 1, 6) }),
            };

            planner.PlanCourses(coursesToPlan);

            EducationPlanOutputter outputter = new EducationPlanOutputter(planner, managementPropertiesDataMapperMock.Object);
            EducationPlanData data = GetDummyEducationPlanData();

            // Act
            var result = outputter.GenerateEducationPlan(data);

            // Assert
            Assert.AreEqual(1, result.PlannedCourses.Count());
            Assert.AreEqual("ENDEVN", result.PlannedCourses.ElementAt(0).Code);
            Assert.AreEqual(2, result.NotPlannedCourses.Count());
            Assert.AreEqual("SCRUMES", result.NotPlannedCourses.ElementAt(0).Code);
            Assert.AreEqual("ENEST", result.NotPlannedCourses.ElementAt(1).Code);
            Assert.AreEqual(1, result.CoursesJustBeforeStart.Count());
            Assert.AreEqual("SCRUMES", result.CoursesJustBeforeStart.ElementAt(0).Code);
            Assert.AreEqual(new DateTime(2017, 1, 2), result.CoursesJustBeforeStart.ElementAt(0).Date);
        }
    }
}
