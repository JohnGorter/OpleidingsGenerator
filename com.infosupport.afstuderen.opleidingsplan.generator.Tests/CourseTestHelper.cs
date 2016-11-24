using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.generator.Tests
{
    public abstract class CourseTestHelper
    {

        protected model.Course CreateNewModelCourseWithOneCourseImplementation(string Code, int priority, DateTime[] days)
        {
            return new model.Course
            {
                Code = Code,
                Priority = priority,
                CourseImplementations = new List<model.CourseImplementation>()
                    {
                        new model.CourseImplementation
                        {
                            Days = days.ToList(),
                            StartDay =  days.First(),
                        }
                    }
            };
        }

        protected model.Course CreateNewModelCourseWithTwoCourseImplementations(string courseId, int priority, DateTime[] days1, DateTime[] days2)
        {
            return new model.Course
            {
                Code = courseId,
                Priority = priority,
                CourseImplementations = new List<model.CourseImplementation>()
                    {
                        new model.CourseImplementation
                        {
                            Days = days1.ToList(),
                            StartDay =  days1.First(),
                        },
                        new model.CourseImplementation
                        {
                            Days = days2.ToList(),
                            StartDay =  days2.First(),
                        }
                    }
            };
        }

        protected generator.Course CreateNewGeneratorCourseWithTwoCourseImplementationsAndPlanned(string courseId, int priority, DateTime[] days1, DateTime[] days2, DateTime[] planned)
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
                    }
                },
                PlannedCourseImplementation = new generator.CourseImplementation
                {
                    Days = planned.ToList(),
                    StartDay = planned.First(),
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

        protected generator.Course CreateNewGeneratorCourseWithOneCourseImplementationAndPlanned(string courseId, int priority, DateTime[] days, DateTime[] planned)
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
                PlannedCourseImplementation = new generator.CourseImplementation
                {
                    Days = planned.ToList(),
                    StartDay = planned.First(),
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
    }
}
