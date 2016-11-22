using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;

namespace com.infosupport.afstuderen.opleidingsplan.generator.Tests
{
    [TestClass]
    public class CourseTest : CourseTestHelper
    {

        [TestMethod]
        public void ExplicitCastTest()
        {
            // Arrange
            model.Course course = CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 2, 14), new DateTime(2017, 2, 15), new DateTime(2017, 2, 16) });

            // Act
            var result = (generator.Course)course;

            // Assert
            Assert.AreEqual("ENDEVN", result.Code);
            Assert.AreEqual(1, result.Priority);
            Assert.AreEqual(new DateTime(2017, 2, 14), result.CourseImplementations.First().Days.ElementAt(0));
            Assert.AreEqual(new DateTime(2017, 2, 15), result.CourseImplementations.First().Days.ElementAt(1));
            Assert.AreEqual(new DateTime(2017, 2, 16), result.CourseImplementations.First().Days.ElementAt(2));
        }


        [TestMethod]
        public void GetFirstAvailableCourseImplementation_OnePlannedCourse_NoAvailableImplementations()
        {
            // Arrange
            generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>()
            {
                CreateNewGeneratorCourseWithOneCourseImplementationAndPlanned("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
            };

            // Act
            var result = course.GetFirstAvailableCourseImplementation(coursesPlanned);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetFirstAvailableCourseImplementation_OnePlannedCourse_OneAvailableImplementation()
        {
            // Arrange
            generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>()
            {
                CreateNewGeneratorCourseWithOneCourseImplementationAndPlanned("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) },
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }),
            };

            // Act
            var result = course.GetFirstAvailableCourseImplementation(coursesPlanned);

            // Assert
            Assert.AreEqual(new DateTime(2017, 1, 3), result.StartDay);
        }

        [TestMethod]
        public void GetFirstAvailableCourseImplementation_NoPlannedCourse_AvailableImplementations_NotChronological()
        {
            // Arrange
            generator.Course course = CreateNewGeneratorCourseWithTwoCourseImplementations("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 4, 17), new DateTime(2017, 4, 18), new DateTime(2017, 4, 19) },
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>();

            // Act
            var result = course.GetFirstAvailableCourseImplementation(coursesPlanned);

            // Assert
            Assert.AreEqual(new DateTime(2017, 1, 3), result.StartDay);
        }

        [TestMethod]
        public void AddIntersectedCourses_AddTwoIntersectedCourses()
        {
            // Arrange
            generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementationAndPlanned("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) },
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<generator.Course> intersectedCourses = new List<generator.Course>()
            {
                CreateNewGeneratorCourseWithOneCourseImplementationAndPlanned("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) },
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }),
                CreateNewGeneratorCourseWithOneCourseImplementationAndPlanned("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) },
                new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) }),
            };

            // Act
            course.AddIntersectedCourses(intersectedCourses);

            // Assert
            Assert.AreEqual(2, course.IntersectedCourseIds.Count());
            Assert.AreEqual("SCRUMES", course.IntersectedCourseIds.ElementAt(0));
            Assert.AreEqual("ENEST", course.IntersectedCourseIds.ElementAt(1));
        }

        [TestMethod]
        public void GetIntersectedCoursesWithEqualOrLowerPriority_TwoPlannedCourses_OneIntersected()
        {
            // Arrange
            generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementationAndPlanned("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) },
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>()
            {
                CreateNewGeneratorCourseWithOneCourseImplementationAndPlanned("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) },
                new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) }),
                CreateNewGeneratorCourseWithOneCourseImplementationAndPlanned("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) },
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }),
            };

            // Act
            var result = course.GetIntersectedCoursesWithEqualOrLowerPriority(coursesPlanned);

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("SCRUMES", result.ElementAt(0).Code);
            Assert.AreEqual(new DateTime(2017, 1, 4), result.ElementAt(0).PlannedCourseImplementation.StartDay);
        }
    }
}
