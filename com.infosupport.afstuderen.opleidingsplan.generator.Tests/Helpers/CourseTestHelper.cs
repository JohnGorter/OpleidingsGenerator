using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.generator.tests.helpers
{
    public abstract class CourseTestHelper
    {

        protected models.Course CreateNewModelCourseWithOneCourseImplementation(string Code, int priority, DateTime[] days)
        {
            return new models.Course
            {
                Code = Code,
                Priority = priority,
                Duration = days.Count() + " dagen",
                CourseImplementations = new List<models.CourseImplementation>()
                {
                    new models.CourseImplementation
                    {
                        Days = days.ToList(),
                        StartDay =  days.First(),
                    }
                }

            };
        }

        protected models.Course CreateNewModelCourseWithTwoCourseImplementations(string courseId, int priority, DateTime[] days1, DateTime[] days2)
        {
            return new models.Course
            {
                Code = courseId,
                Priority = priority,
                Duration = days1.Count() + " dagen",
                CourseImplementations = new List<models.CourseImplementation>()
                {
                    new models.CourseImplementation
                    {
                        Days = days1.ToList(),
                        StartDay =  days1.First(),
                    },
                    new models.CourseImplementation
                    {
                        Days = days2.ToList(),
                        StartDay =  days2.First(),
                    }
                }
            };
        }

        protected generator.Course CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus(string courseId, int priority, DateTime[] days1, Status status1, DateTime[] days2, Status status2)
        {
            return new generator.Course
            {
                Code = courseId,
                Priority = priority,
                CourseImplementations = new List<generator.CourseImplementation>()
                {
                    new generator.CourseImplementation
                    {
                        Days = days1.ToList(),
                        StartDay =  days1.First(),
                        Status = status1,
                    },
                    new generator.CourseImplementation
                    {
                        Days = days2.ToList(),
                        StartDay =  days2.First(),
                        Status = status2,
                    }
                },
            };
        }

        protected generator.Course CreateNewGeneratorCourseWithOneCourseImplementationAndStatus(string courseId, int priority, DateTime[] days, Status status)
        {
            return new generator.Course
            {
                Code = courseId,
                Priority = priority,
                CourseImplementations = new List<generator.CourseImplementation>()
                {
                    new generator.CourseImplementation
                    {
                        Days = days.ToList(),
                        StartDay =  days.First(),
                        Status = status,
                    },
                },
            };
        }

        protected generator.Course CreateNewGeneratorCourseWithOneCourseImplementation(string courseId, int priority, DateTime[] days)
        {
            return new generator.Course
            {
                Code = courseId,
                Priority = priority,
                CourseImplementations = new List<generator.CourseImplementation>()
                {
                    new generator.CourseImplementation
                    {
                        Days = days.ToList(),
                        StartDay =  days.First(),
                    },
                },
            };
        }

        protected generator.Course CreateNewGeneratorCourseWithTwoCourseImplementations(string courseId, int priority, DateTime[] days1, DateTime[] days2)
        {
            return new generator.Course
            {
                Code = courseId,
                Priority = priority,
                CourseImplementations = new List<generator.CourseImplementation>()
                {
                    new generator.CourseImplementation
                    {
                        Days = days1.ToList(),
                        StartDay =  days1.First(),
                    },
                    new generator.CourseImplementation
                    {
                        Days = days2.ToList(),
                        StartDay =  days2.First(),
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

        protected generator.CourseImplementation CreateNewGeneratorCourseImplementation(DateTime[] days)
        {
            return new generator.CourseImplementation
            {
                Days = days.ToList(),
                StartDay = days.First(),
            };            
        }
    }
}
