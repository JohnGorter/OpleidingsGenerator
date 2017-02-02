using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InfoSupport.KC.OpleidingsplanGenerator.Models.Tests.Helpers;

namespace InfoSupport.KC.OpleidingsplanGenerator.Models.Tests
{
    [TestClass]
    public class ModelTests : EducationPlanTestHelper
    {

        [TestMethod]
        public void EducationPlan_CalculatedProperties_PlannedCourses()
        {
            // Arrange
            EducationPlan educationPlan = GetDummyEducationPlanWithPlannedCourses();

            // Assert
            Assert.AreEqual(2200, educationPlan.PlannedCoursesTotalPrice);
            Assert.AreEqual(1760, educationPlan.PlannedCoursesTotalPriceWithDiscount);
            Assert.AreEqual(0, educationPlan.NotPlannedCoursesTotalPrice);
            Assert.AreEqual(0, educationPlan.NotPlannedCoursesTotalPriceWithDiscount);

        }

        [TestMethod]
        public void EducationPlan_CalculatedProperties_NotPlannedCourses()
        {
            // Arrange
            EducationPlan educationPlan = GetDummyEducationPlanWithNotPlannedCourses();

            // Assert
            Assert.AreEqual(0, educationPlan.PlannedCoursesTotalPrice);
            Assert.AreEqual(0, educationPlan.PlannedCoursesTotalPriceWithDiscount);
            Assert.AreEqual(2200, educationPlan.NotPlannedCoursesTotalPrice);
            Assert.AreEqual(1760, educationPlan.NotPlannedCoursesTotalPriceWithDiscount);
        }


        [TestMethod]
        public void EducationPlanCourse_CalculatedProperties_WithDate()
        {
            // Arrange
            EducationPlanCourse course = CreatePlannedEducationPlanCourse("2NETARCH", new DateTime(2016, 11, 29), ".NET for Architects and Project Managers", 2, "geen opmerkingen", 1150);

            // Assert
            Assert.AreEqual(920, course.PriceWithDiscount);
            Assert.AreEqual(48, course.Week);
        }

        [TestMethod]
        public void EducationPlanCourse_CalculatedProperties_WithoutDate()
        {
            // Arrange
            EducationPlanCourse course = CreatePlannedEducationPlanCourse("2NETARCH", null, ".NET for Architects and Project Managers", 2, "geen opmerkingen", 1150);

            // Assert
            Assert.AreEqual(920, course.PriceWithDiscount);
            Assert.AreEqual(-1, course.Week);
        }


        [TestMethod]
        public void CourseImplementation_StartDay()
        {
            // Arrange
            CourseImplementation implementation = new CourseImplementation
            {
                Days = new DateTime[]
                {
                    new DateTime(2016, 12, 5),
                    new DateTime(2016, 12, 6),
                    new DateTime(2016, 12, 7),
                    new DateTime(2016, 12, 8),
                }
            };

            // Assert
            Assert.AreEqual(new DateTime(2016, 12, 5), implementation.StartDay);
        }

        [TestMethod]
        public void CourseImplementation_StartDay_ImplementationOrder()
        {
            // Arrange
            CourseImplementation implementation = new CourseImplementation
            {
                Days = new DateTime[]
                {
                    new DateTime(2016, 12, 7),
                    new DateTime(2016, 12, 6),
                    new DateTime(2016, 12, 8),
                    new DateTime(2016, 12, 5),
                }
            };

            // Assert
            Assert.AreEqual(new DateTime(2016, 12, 5), implementation.StartDay);
        }
    }
}
