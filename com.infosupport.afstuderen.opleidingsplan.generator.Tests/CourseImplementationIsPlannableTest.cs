using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using InfoSupport.KC.OpleidingsplanGenerator.Generator.Tests.Helpers;

namespace InfoSupport.KC.OpleidingsplanGenerator.Generator.Tests
{
    [TestClass]
    public class CourseImplementationIsPlannableTest : CourseTestHelper
    {
        [TestMethod]
        public void CI_IsPlannable_NoPlannedCourses_ResultIsTrue()
        {
            //Arrange
            Generator.CourseImplementation courseImplementation =
                CreateNewGeneratorCourseImplementation(new DateTime[] { new DateTime(2017, 2, 14), new DateTime(2017, 2, 15), new DateTime(2017, 2, 16) });
            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>();

            //Act
            bool result = courseImplementation.IsPlannable(coursesPlanned, 1, "ENDEVN");

            //Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void CI_IsPlannable_OnePlannedCourse_NoOverlap_ResultIsTrue()
        {
            //Arrange
            Generator.CourseImplementation courseImplementation =
                CreateNewGeneratorCourseImplementation(new DateTime[] { new DateTime(2017, 2, 14), new DateTime(2017, 2, 15), new DateTime(2017, 2, 16) });

            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>()
            {
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }, Status.UNKNOWN,
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }, Status.UNKNOWN),
            };

            //Act
            bool result = courseImplementation.IsPlannable(coursesPlanned, 1, "ENDEVN");

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CI_IsPlannable_OnePlannedCourseWithOverLap_ResultIsFalse()
        {
            //Arrange
            Generator.CourseImplementation courseImplementation =
               CreateNewGeneratorCourseImplementation(new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) });

            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>()
            {
                CreateNewGeneratorCourseWithOneCourseImplementationAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }, Status.PLANNED),
            };

            //Act
            bool result = courseImplementation.IsPlannable(coursesPlanned, 1, "ENDEVN");

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CI_IsPlannable_TwoCoursesAvailable_OverlapOfOverlapFree_ResultIsTrue()
        {
            //Arrange
            Generator.CourseImplementation courseImplementation =
              CreateNewGeneratorCourseImplementation(new DateTime[] { new DateTime(2017, 2, 14), new DateTime(2017, 2, 15), new DateTime(2017, 2, 16) });

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
            bool result = courseImplementation.IsPlannable(coursesPlanned, 1, "ENDEVN");

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CI_IsPlannable_TwoCourses_OnePlannedCourse_OverlapOfOverlapPlanned_ResultIsFalse()
        {
            //Arrange
            Generator.CourseImplementation courseImplementation =
             CreateNewGeneratorCourseImplementation(new DateTime[] { new DateTime(2017, 2, 14), new DateTime(2017, 2, 15), new DateTime(2017, 2, 16) });

            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>()
            {
                CreateNewGeneratorCourseWithOneCourseImplementationAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }, Status.PLANNED),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3)}, Status.AVAILABLE,
                new DateTime[] { new DateTime(2017, 2, 13), new DateTime(2017, 2, 14)}, Status.AVAILABLE),
            };

            //Act
            bool result = courseImplementation.IsPlannable(coursesPlanned, 1, "ENDEVN");

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CI_IsPlannable_ThreePlannedCourses_ResultIsTrue()
        {
            //Arrange
            Generator.CourseImplementation courseImplementation =
            CreateNewGeneratorCourseImplementation(new DateTime[] { new DateTime(2017, 2, 14), new DateTime(2017, 2, 15), new DateTime(2017, 2, 16) });

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
            bool result = courseImplementation.IsPlannable(coursesPlanned, 1, "ENDEVN");

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CI_IsPlannable_FourPlannedCourses_IntersectedLastTreeReturnsFalse_ResultIsTrue()
        {
            //Arrange
            Generator.CourseImplementation courseImplementation =
          CreateNewGeneratorCourseImplementation(new DateTime[] { new DateTime(2017, 2, 14), new DateTime(2017, 2, 15), new DateTime(2017, 2, 16) });

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
            bool result = courseImplementation.IsPlannable(coursesPlanned, 1, "ENDEVN");

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CI_IsPlannable_FourPlannedCourses_IntersectedLastTreeReturnsFalse_ResultIsFalse()
        {
            //Arrange
            Generator.CourseImplementation courseImplementation =
            CreateNewGeneratorCourseImplementation(new DateTime[] { new DateTime(2017, 2, 14), new DateTime(2017, 2, 15), new DateTime(2017, 2, 16) });

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
            bool result = courseImplementation.IsPlannable(coursesPlanned, 1, "ENDEVN");

            //Assert
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void CI_IsPlannable_OnePlannedCourseWithLowerPriority_ResultIsFalse()
        {
            //Arrange
            Generator.CourseImplementation courseImplementation =
            CreateNewGeneratorCourseImplementation(new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) });

            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>()
            {
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }, Status.PLANNED,
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }, Status.NOTPLANNED),
            };

            //Act
            bool result = courseImplementation.IsPlannable(coursesPlanned, 1, "ENDEVN");

            //Assert
            Assert.IsFalse(result);
        }
    }
}
