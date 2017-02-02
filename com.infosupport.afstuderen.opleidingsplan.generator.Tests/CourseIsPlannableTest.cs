
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using InfoSupport.KC.OpleidingsplanGenerator.Generator.Tests.Helpers;

namespace InfoSupport.KC.OpleidingsplanGenerator.Generator.Tests
{
    [TestClass]
    public class CourseIsPlannableTest : CourseTestHelper
    {
        [TestMethod]
        public void IsPlannable_NoPlannedCourses_ResultIsTrue()
        {
            //Arrange
            Generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 2, 14), new DateTime(2017, 2, 15), new DateTime(2017, 2, 16) });
            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>();

            //Act
            bool result = course.IsPlannable(coursesPlanned);

            //Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void IsPlannable_OnePlannedCourse_NoOverlap_ResultIsTrue()
        {
            //Arrange
            Generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 2, 14), new DateTime(2017, 2, 15), new DateTime(2017, 2, 16) });
            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>()
            {
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }, Status.UNKNOWN,
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }, Status.UNKNOWN),
            };

            //Act
            bool result = course.IsPlannable(coursesPlanned);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsPlannable_OnePlannedCourseWithOverLap_ResultIsFalse()
        {
            //Arrange
            Generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) });
            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>()
            {
                CreateNewGeneratorCourseWithOneCourseImplementationAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }, Status.PLANNED),
            };

            //Act
            bool result = course.IsPlannable(coursesPlanned);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsPlannable_TwoCoursesAvailable_OverlapOfOverlapFree_ResultIsTrue()
        {
            //Arrange
            Generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 2, 14), new DateTime(2017, 2, 15), new DateTime(2017, 2, 16) });

            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>()
            {
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }, Status.AVAILABLE,
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }, Status.AVAILABLE),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3)}, Status.AVAILABLE,
                new DateTime[] { new DateTime(2017, 2, 13), new DateTime(2017, 2, 14)}, Status.AVAILABLE),
            };

            //Act
            bool result = course.IsPlannable(coursesPlanned);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsPlannable_TwoCourses_OnePlannedCourse_OverlapOfOverlapPlanned_ResultIsFalse()
        {
            //Arrange
            Generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 2, 14), new DateTime(2017, 2, 15), new DateTime(2017, 2, 16) });

            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>()
            {
                CreateNewGeneratorCourseWithOneCourseImplementationAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }, Status.PLANNED),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3)}, Status.AVAILABLE,
                new DateTime[] { new DateTime(2017, 2, 13), new DateTime(2017, 2, 14)}, Status.AVAILABLE),
            };

            //Act
            bool result = course.IsPlannable(coursesPlanned);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsPlannable_ThreePlannedCourses_ResultIsTrue()
        {
            //Arrange
            Generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 2, 14), new DateTime(2017, 2, 15), new DateTime(2017, 2, 16) });

            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>()
            {
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ALMUVS", 1,
                new DateTime[] { new DateTime(2017, 3, 7), new DateTime(2017, 3, 8), new DateTime(2017, 3, 9)}, Status.AVAILABLE,
                new DateTime[] { new DateTime(2017, 5, 15), new DateTime(2017, 5, 16), new DateTime(2017, 5, 17)}, Status.AVAILABLE),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }, Status.AVAILABLE,
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }, Status.AVAILABLE),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3)}, Status.AVAILABLE,
                new DateTime[] { new DateTime(2017, 2, 13), new DateTime(2017, 2, 14)}, Status.AVAILABLE),
            };

            //Act
            bool result = course.IsPlannable(coursesPlanned);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsPlannable_FourPlannedCourses_IntersectedLastTreeReturnsFalse_ResultIsTrue()
        {
            //Arrange
            Generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 2, 14), new DateTime(2017, 2, 15), new DateTime(2017, 2, 16) });

            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>()
            {
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ALMUVS", 1,
                new DateTime[] { new DateTime(2017, 3, 8), new DateTime(2017, 3, 9)}, Status.AVAILABLE,
                new DateTime[] { new DateTime(2017, 5, 15), new DateTime(2017, 5, 16), new DateTime(2017, 5, 17)}, Status.AVAILABLE),
                CreateNewGeneratorCourseWithOneCourseImplementationAndStatus("MS20461", 1,
                new DateTime[] { new DateTime(2017, 3, 7) }, Status.AVAILABLE),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }, Status.AVAILABLE,
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }, Status.AVAILABLE),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3)}, Status.AVAILABLE,
                new DateTime[] { new DateTime(2017, 2, 13), new DateTime(2017, 2, 14)}, Status.AVAILABLE),
            };

            //Act
            bool result = course.IsPlannable(coursesPlanned);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsPlannable_FourPlannedCourses_IntersectedLastTreeReturnsFalse_ResultIsFalse()
        {
            //Arrange
            Generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 2, 14), new DateTime(2017, 2, 15), new DateTime(2017, 2, 16) });

            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>()
            {
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ALMUVS", 1,
                new DateTime[] { new DateTime(2017, 3, 8), new DateTime(2017, 3, 9)}, Status.AVAILABLE,
                new DateTime[] { new DateTime(2017, 5, 15), new DateTime(2017, 5, 16), new DateTime(2017, 5, 17)}, Status.PLANNED),
                CreateNewGeneratorCourseWithOneCourseImplementationAndStatus("MS20461", 1,
                new DateTime[] { new DateTime(2017, 3, 7) }, Status.AVAILABLE),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }, Status.AVAILABLE,
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }, Status.AVAILABLE),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3)}, Status.AVAILABLE,
                new DateTime[] { new DateTime(2017, 2, 13), new DateTime(2017, 2, 14)}, Status.AVAILABLE),
            };

            //Act
            bool result = course.IsPlannable(coursesPlanned);

            //Assert
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void IsPlannable_OnePlannedCourseWithLowerPriority_ResultIsFalse()
        {
            //Arrange
            Generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 2,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) });

            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>()
            {
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }, Status.PLANNED,
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }, Status.NOTPLANNED),
            };

            //Act
            bool result = course.IsPlannable(coursesPlanned);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsPlannable_ThreeCoursesSameImplementation_AlreadyPlannedImplementation_ResultIsFalse()
        {
            //Arrange
            Generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) });

            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>()
            {
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }, Status.UNPLANNABLE,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }, Status.UNPLANNABLE),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ANGULARJS", 1,
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }, Status.PLANNED,
                new DateTime[] { new DateTime(2017, 4, 24), new DateTime(2017, 4, 25), new DateTime(2017, 4, 26) }, Status.UNPLANNABLE),
            };

            //Act
            bool result = course.IsPlannable(coursesPlanned);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsPlannable_PlannedIntersectedCourse_ResultIsFalse()
        {
            //Arrange
            Generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 2, 14), new DateTime(2017, 2, 15), new DateTime(2017, 2, 16) });

            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>()
            {
                CreateNewGeneratorCourseWithOneCourseImplementationAndStatus("MS20461", 1,
                new DateTime[] { new DateTime(2017, 2, 14), new DateTime(2017, 2, 15) }, Status.PLANNED),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ALMUVS", 1,
                new DateTime[] { new DateTime(2017, 2, 14), new DateTime(2017, 2, 15) }, Status.UNKNOWN,
                new DateTime[] { new DateTime(2017, 5, 15), new DateTime(2017, 5, 16) }, Status.AVAILABLE),
            };

            //Act
            bool result = course.IsPlannable(coursesPlanned);

            //Assert
            Assert.IsFalse(result);
        }
    }
}

