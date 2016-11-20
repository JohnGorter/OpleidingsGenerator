using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace com.infosupport.afstuderen.opleidingsplan.generator.Tests
{
    [TestClass]
    public class CourseTest : CourseTestHelper
    {
        [TestMethod]
        public void IsPlannable_NoPlannedCourses_ResultIsTrue()
        {
            generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 2, 14), new DateTime(2017, 2, 15), new DateTime(2017, 2, 16) });
            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>();

            bool result = course.IsPlannable(coursesPlanned);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsPlannable_OnePlannedCourse_ResultIsTrue()
        {
            generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 2, 14), new DateTime(2017, 2, 15), new DateTime(2017, 2, 16) });
            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>()
            {
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndPlanned("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) },
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
            };

            bool result = course.IsPlannable(coursesPlanned);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsPlannable_OnePlannedCourse_ResultIsFalse()
        {
            generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) });
            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>()
            {
                CreateNewGeneratorCourseWithOneCourseImplementationAndPlanned("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
            };

            bool result = course.IsPlannable(coursesPlanned);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsPlannable_TwoPlannedCourses_ResultIsTrue()
        {
            generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 2, 14), new DateTime(2017, 2, 15), new DateTime(2017, 2, 16) });

            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>()
            {
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndPlanned("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) },
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndPlanned("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3)},
                new DateTime[] { new DateTime(2017, 2, 13), new DateTime(2017, 2, 14)},
                new DateTime[] { new DateTime(2017, 2, 13), new DateTime(2017, 2, 14)}),
            };

            bool result = course.IsPlannable(coursesPlanned);

            Assert.IsTrue(result);
        }
        [TestMethod]
        public void IsPlannable_TwoPlannedCourses_ResultIsFalse()
        {
            generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 2, 14), new DateTime(2017, 2, 15), new DateTime(2017, 2, 16) });

            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>()
            {
                CreateNewGeneratorCourseWithOneCourseImplementationAndPlanned("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndPlanned("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3)},
                new DateTime[] { new DateTime(2017, 2, 13), new DateTime(2017, 2, 14)},
                new DateTime[] { new DateTime(2017, 2, 13), new DateTime(2017, 2, 14)}),
            };

            bool result = course.IsPlannable(coursesPlanned);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsPlannable_ThreePlannedCourses_ResultIsTrue()
        {
            generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 2, 14), new DateTime(2017, 2, 15), new DateTime(2017, 2, 16) });

            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>()
            {
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndPlanned("ALMUVS", 1,
                new DateTime[] { new DateTime(2017, 3, 7), new DateTime(2017, 3, 8), new DateTime(2017, 3, 9)},
                new DateTime[] { new DateTime(2017, 5, 15), new DateTime(2017, 5, 16), new DateTime(2017, 5, 17)},
                new DateTime[] { new DateTime(2017, 3, 7), new DateTime(2017, 3, 8), new DateTime(2017, 3, 9)}),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndPlanned("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) },
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndPlanned("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3)},
                new DateTime[] { new DateTime(2017, 2, 13), new DateTime(2017, 2, 14)},
                new DateTime[] { new DateTime(2017, 2, 13), new DateTime(2017, 2, 14)}),
            };

            bool result = course.IsPlannable(coursesPlanned);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsPlannable_FourPlannedCourses_IntersectedLastTreeReturnsFalse_ResultIsTrue()
        {
            generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 2, 14), new DateTime(2017, 2, 15), new DateTime(2017, 2, 16) });

            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>()
            {
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndPlanned("ALMUVS", 1,
                new DateTime[] { new DateTime(2017, 3, 8), new DateTime(2017, 3, 9)},
                new DateTime[] { new DateTime(2017, 5, 15), new DateTime(2017, 5, 16), new DateTime(2017, 5, 17)},
                new DateTime[] { new DateTime(2017, 3, 8), new DateTime(2017, 3, 9)}),
                CreateNewGeneratorCourseWithOneCourseImplementationAndPlanned("MS20461", 1,
                new DateTime[] { new DateTime(2017, 3, 7) },
                new DateTime[] { new DateTime(2017, 3, 7) }),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndPlanned("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) },
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndPlanned("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3)},
                new DateTime[] { new DateTime(2017, 2, 13), new DateTime(2017, 2, 14)},
                new DateTime[] { new DateTime(2017, 2, 13), new DateTime(2017, 2, 14)}),
            };

            bool result = course.IsPlannable(coursesPlanned);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsPlannable_OnePlannedCourseWithHigherPriority_ResultIsFalse()
        {
            generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 2,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) });

            IEnumerable< generator.Course> coursesPlanned = new List<generator.Course>()
            {
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndPlanned("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) },
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
            };

            bool result = course.IsPlannable(coursesPlanned);

            Assert.IsFalse(result);
        }

    }
}
