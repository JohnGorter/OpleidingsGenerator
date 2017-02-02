using InfoSupport.KC.OpleidingsplanGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSupport.KC.OpleidingsplanGenerator.Generator.Tests.Helpers
{
    public abstract class CourseTestHelper
    {

        protected Models.Course CreateNewModelCourseWithOneCourseImplementation(string Code, int priority, DateTime[] days)
        {
            return new Models.Course
            {
                Code = Code,
                Priority = priority,
                Duration = days.Count() + " dagen",
                CourseImplementations = new List<Models.CourseImplementation>()
                {
                    new Models.CourseImplementation
                    {
                        Days = days.ToList(),
                    }
                }

            };
        }

        protected Models.Course CreateNewModelCourseWithTwoCourseImplementations(string courseId, int priority, DateTime[] days1, DateTime[] days2)
        {
            return new Models.Course
            {
                Code = courseId,
                Priority = priority,
                Duration = days1.Count() + " dagen",
                CourseImplementations = new List<Models.CourseImplementation>()
                {
                    new Models.CourseImplementation
                    {
                        Days = days1.ToList(),
                    },
                    new Models.CourseImplementation
                    {
                        Days = days2.ToList(),
                    }
                }
            };
        }

        protected Models.Course CreateNewModelCourseWithTreeCourseImplementations(string courseId, int priority, DateTime[] days1, DateTime[] days2, DateTime[] days3)
        {
            return new Models.Course
            {
                Code = courseId,
                Priority = priority,
                Duration = days1.Count() + " dagen",
                CourseImplementations = new List<Models.CourseImplementation>()
                {
                    new Models.CourseImplementation
                    {
                        Days = days1.ToList(),
                    },
                    new Models.CourseImplementation
                    {
                        Days = days2.ToList(),
                    },
                    new Models.CourseImplementation
                    {
                        Days = days3.ToList(),
                    }
                }
            };
        }

        protected Generator.Course CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus(string courseId, int priority, DateTime[] days1, Status status1, DateTime[] days2, Status status2)
        {
            return new Generator.Course
            {
                Code = courseId,
                Priority = priority,
                CourseImplementations = new List<Generator.CourseImplementation>()
                {
                    new Generator.CourseImplementation
                    {
                        Days = days1.ToList(),
                        Status = status1,
                    },
                    new Generator.CourseImplementation
                    {
                        Days = days2.ToList(),
                        Status = status2,
                    }
                },
            };
        }

        protected Generator.Course CreateNewGeneratorCourseWithThreeCourseImplementationsAndStatus(string courseId, int priority, DateTime[] days1, Status status1, DateTime[] days2, Status status2, DateTime[] days3, Status status3)
        {
            return new Generator.Course
            {
                Code = courseId,
                Priority = priority,
                CourseImplementations = new List<Generator.CourseImplementation>()
                {
                    new Generator.CourseImplementation
                    {
                        Days = days1.ToList(),
                        Status = status1,
                    },
                    new Generator.CourseImplementation
                    {
                        Days = days2.ToList(),
                        Status = status2,
                    },
                    new Generator.CourseImplementation
                    {
                        Days = days3.ToList(),
                        Status = status3,
                    }
                },
            };
        }

        protected Generator.Course CreateNewGeneratorCourseWithOneCourseImplementationAndStatus(string courseId, int priority, DateTime[] days, Status status)
        {
            return new Generator.Course
            {
                Code = courseId,
                Priority = priority,
                CourseImplementations = new List<Generator.CourseImplementation>()
                {
                    new Generator.CourseImplementation
                    {
                        Days = days.ToList(),
                        Status = status,
                    },
                },
            };
        }

        protected Generator.Course CreateNewGeneratorCourseWithOneCourseImplementation(string courseId, int priority, DateTime[] days)
        {
            return new Generator.Course
            {
                Code = courseId,
                Priority = priority,
                CourseImplementations = new List<Generator.CourseImplementation>()
                {
                    new Generator.CourseImplementation
                    {
                        Days = days.ToList(),
                    },
                },
            };
        }

        protected Generator.Course CreateNewGeneratorCourseWithTwoCourseImplementations(string courseId, int priority, DateTime[] days1, DateTime[] days2)
        {
            return new Generator.Course
            {
                Code = courseId,
                Priority = priority,
                CourseImplementations = new List<Generator.CourseImplementation>()
                {
                    new Generator.CourseImplementation
                    {
                        Days = days1.ToList(),
                    },
                    new Generator.CourseImplementation
                    {
                        Days = days2.ToList(),
                    },
                },
            };
        }

        protected EducationPlanData GetDummyEducationPlanData()
        {
            return new EducationPlanData
            {
                Created = new DateTime(2016, 11, 29),
                InPaymentFrom = new DateTime(2016, 12, 5),
                EmployableFrom = new DateTime(2017, 2, 6),
                Profile = "NET_Developer",
                NameEmployee = "Pim Verheij",
                NameTeacher = "Felix Sedney",
                KnowledgeOf = "MVC, DPAT, OOUML, SCRUMES",
            };
        }

        protected Generator.CourseImplementation CreateNewGeneratorCourseImplementation(DateTime[] days)
        {
            return new Generator.CourseImplementation
            {
                Days = days.ToList(),
            };
        }

        protected ManagementProperties GetDummyDataManagementProperties()
        {
            return new ManagementProperties
            {
                OlcPrice = 125,
                PeriodAfterLastCourseEmployableInDays = 7,
                PeriodBeforeStartNotifiable = 7,
                PeriodEducationPlanInDays = 90,
            };
        }
    }
}
