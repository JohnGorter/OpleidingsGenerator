using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using com.infosupport.afstuderen.opleidingsplan.generator.tests.helpers;

namespace com.infosupport.afstuderen.opleidingsplan.generator.tests
{
    [TestClass]
    public class EducationPlanOutputterTest : CourseTestHelper
    {

        [TestMethod]
        public void GenerateEducationPlan_FourCoursesPlanned()
        {
            // Arrange
            Planner planner = new Planner();
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

            planner.PlanCourses(coursesToPlan);

            EducationPlanOutputter outputter = new EducationPlanOutputter(planner);
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

            Assert.AreEqual(2, result.NotPlannedCourses.ElementAt(0).IntersectedCourses.Count());
            Assert.AreEqual("SCRUMES", result.NotPlannedCourses.ElementAt(0).IntersectedCourses.ElementAt(0).Code);
            Assert.AreEqual("ENDEVN", result.NotPlannedCourses.ElementAt(0).IntersectedCourses.ElementAt(1).Code);
            Assert.AreEqual(2, result.NotPlannedCourses.ElementAt(1).IntersectedCourses.Count());
            Assert.AreEqual("SECDEV", result.NotPlannedCourses.ElementAt(1).IntersectedCourses.ElementAt(0).Code);
            Assert.AreEqual("XSD", result.NotPlannedCourses.ElementAt(1).IntersectedCourses.ElementAt(1).Code);

            Assert.AreEqual(new DateTime(2016, 11, 29), result.Created);
            Assert.AreEqual(new DateTime(2016, 12, 5), result.InPaymentFrom);
            Assert.AreEqual(new DateTime(2017, 2, 6), result.EmployableFrom);
            Assert.AreEqual("NET_Developer", result.Profile);
            Assert.AreEqual("Pim Verheij", result.NameEmployee);
            Assert.AreEqual("Felix Sedney", result.NameTeacher);
            Assert.AreEqual("MVC, DPAT, OOUML, SCRUMES", result.KnowledgeOf);
        }
    }
}
