using com.infosupport.afstuderen.opleidingsplan.api.models;
using com.infosupport.afstuderen.opleidingsplan.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.api.tests.helpers
{
    public abstract class EducationPlanTestHelper : ProfileTestHelper
    {
        protected RestEducationPlan GetDummyRestEducationPlan(string[] courses)
        {
            return new RestEducationPlan
            {
                InPaymentFrom = new DateTime(2016, 12, 5),
                EmployableFrom = new DateTime(2017, 2, 6),
                ProfileId = 1,
                NameEmployee = "Pim Verheij",
                NameTeacher = "Felix Sedney",
                KnowledgeOf = "MVC, DPAT, OOUML, SCRUMES",
                Courses = courses,
            };
        }
        
        protected EducationPlan GetDummyEducationPlan()
        {
            return new EducationPlan
            {
                Created = DateTime.Now,
                InPaymentFrom = new DateTime(2017, 1, 1),
                EmployableFrom = new DateTime(2017, 4, 1),
                KnowledgeOf = "MVC, DPAT, OOUML, SCRUMES",
                NameEmployee = "Pim Verheij",
                NameTeacher = "Felix Sedney",
                Profile = "NET_Developer",
                PlannedCourses = new List<EducationPlanCourse>()
                {
                    CreatePlannedEducationPlanCourse("2NETARCH", new DateTime(2016, 11, 29), ".NET for Architects and Project Managers", 2, "geen opmerkingen", 1150),
                    CreatePlannedEducationPlanCourse("ADCSB", new DateTime(2016, 12, 5), "Advanced C#", 2, "geen opmerkingen", 1050),
                },
                NotPlannedCourses = new List<EducationPlanCourse>(),
            };
        }

        protected EducationPlanCourse CreatePlannedEducationPlanCourse(string code, DateTime? date, string name, int days, string commentary, decimal price)
        {
            return new EducationPlanCourse
            {
                Code = code,
                Date = date,
                Name = name,
                Days = days,
                Commentary = commentary,
                Price = price,
            };
        }

        protected integration.Course CreateNewIntegrationCourseWithTwoCourseImplementations(string courseId, int priority, DateTime[] days1, DateTime[] days2)
        {
            return new integration.Course
            {
                Code = courseId,
                Duration = days1.Count() + " dagen",
                CourseImplementations = new integration.CourseImplementation[]
                {
                    new integration.CourseImplementation
                    {
                        Days = days1,
                        Location = "Veenendaal",
                    },
                    new integration.CourseImplementation
                    {
                        Days = days2,
                        Location = "Veenendaal",
                    },
                },
            };
        }
    }
}
