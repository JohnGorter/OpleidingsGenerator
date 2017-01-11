using com.infosupport.afstuderen.opleidingsplan.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.generator.tests.helpers
{
    public class EducationPlanTestHelper
    {
        protected EducationPlan GetDummyEducationPlan()
        {
            return new EducationPlan
            {
                Created = new DateTime(2016, 12, 12),
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
                NotPlannedCourses = new List<EducationPlanCourse>()
                {
                    CreatePlannedEducationPlanCourse("POLDEVEL", new DateTime(2017, 1, 11), "Developing Polymer Elements", 2, "geen opmerkingen", 1725),
                    CreatePlannedEducationPlanCourse("POLDEV", new DateTime(2017, 1, 25), "Developing Apps and Elements with Polymer", 2, "geen opmerkingen", 2625),
                },
                BlockedDates =  new List<DateTime>()
                {
                    new DateTime(2017, 2, 1),
                    new DateTime(2017, 2, 2),
                    new DateTime(2017, 3, 12),
                    new DateTime(2017, 3, 13),
                    new DateTime(2017, 3, 14),
                }
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
    }
}
