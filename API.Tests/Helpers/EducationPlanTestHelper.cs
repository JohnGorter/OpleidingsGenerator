using InfoSupport.KC.OpleidingsplanGenerator.Api.Models;
using InfoSupport.KC.OpleidingsplanGenerator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSupport.KC.OpleidingsplanGenerator.Api.Tests.Helpers
{
    public abstract class EducationPlanTestHelper : ProfileTestHelper
    {
        protected RestEducationPlan GetDummyRestEducationPlan(Collection<RestEducationPlanCourse> courses)
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

        protected List<EducationPlanCompare> GetDummyEducationPlanCompareList()
        {
            return new List<EducationPlanCompare>
            {
                new EducationPlanCompare
                {
                    EducationPlanNew = GetDummyEducationPlan(),
                    EducationPlanOld = GetDummyEducationPlan(),
                },               
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

        protected Integration.Course CreateNewIntegrationCourseWithTwoCourseImplementations(string courseId, int priority, Collection<DateTime> days1, Collection<DateTime> days2)
        {
            return new Integration.Course
            {
                Code = courseId,
                Duration = days1.Count() + " dagen",
                CourseImplementations = new Collection<Integration.CourseImplementation>
                {
                    new Integration.CourseImplementation
                    {
                        Days = days1,
                        Location = "Veenendaal",
                    },
                    new Integration.CourseImplementation
                    {
                        Days = days2,
                        Location = "Veenendaal",
                    },
                },
            };
        }

        protected Integration.Course CreateNewIntegrationCourseWithOneCourseImplementation(string courseId, int priority, Collection<DateTime> days1)
        {
            return new Integration.Course
            {
                Code = courseId,
                Duration = days1.Count() + " dagen",
                CourseImplementations = new Collection<Integration.CourseImplementation>
                {
                    new Integration.CourseImplementation
                    {
                        Days = days1,
                        Location = "Veenendaal",
                    }
                },
            };
        }

        protected List<Generator.Course> GetDummyGeneratorCourses()
        {
            return new List<Generator.Course>
            {
                new Generator.Course {
                    Code = "POLDEVEL",
                    Priority = 1,
                    CourseImplementations = new List<Generator.CourseImplementation>()
                    {
                        new Generator.CourseImplementation
                        {
                            Days = new List<DateTime> {
                                new DateTime(2017, 1, 1)
                            },
                        },
                    },
                }
            };
        }
    }
}
