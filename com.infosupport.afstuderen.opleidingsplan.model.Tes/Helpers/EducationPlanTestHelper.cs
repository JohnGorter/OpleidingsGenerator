using System;
using System.Collections.Generic;

namespace InfoSupport.KC.OpleidingsplanGenerator.Models.Tests.Helpers
{
    public class EducationPlanTestHelper
    {
        protected EducationPlan GetDummyEducationPlanWithPlannedCourses()
        {
            return new EducationPlan
            {
                Created = DateTime.Now,
                InPaymentFrom = DateTime.Now.AddDays(5),
                EmployableFrom = DateTime.Now.AddDays(90),
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
                StaffDiscountInPercentage = 80,
            };
        }

        protected EducationPlan GetDummyEducationPlanWithNotPlannedCourses()
        {
            return new EducationPlan
            {
                Created = DateTime.Now,
                InPaymentFrom = DateTime.Now.AddDays(5),
                EmployableFrom = DateTime.Now.AddDays(90),
                KnowledgeOf = "MVC, DPAT, OOUML, SCRUMES",
                NameEmployee = "Pim Verheij",
                NameTeacher = "Felix Sedney",
                Profile = "NET_Developer",
                PlannedCourses = new List<EducationPlanCourse>(),
                NotPlannedCourses = new List<EducationPlanCourse>() {
                    CreatePlannedEducationPlanCourse("2NETARCH", new DateTime(2016, 11, 29), ".NET for Architects and Project Managers", 2, "geen opmerkingen", 1150),
                    CreatePlannedEducationPlanCourse("ADCSB", new DateTime(2016, 12, 5), "Advanced C#", 2, "geen opmerkingen", 1050),
                }
            };
        }
    }
}