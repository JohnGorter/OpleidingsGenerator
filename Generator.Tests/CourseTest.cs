using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;
using InfoSupport.KC.OpleidingsplanGenerator.Generator.Tests.Helpers;

namespace InfoSupport.KC.OpleidingsplanGenerator.Generator.Tests
{
    [TestClass]
    public class CourseTest : CourseTestHelper
    {

        [TestMethod]
        public void ExplicitCastTest()
        {
            // Arrange
            Models.Course course = CreateNewModelCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 2, 14), new DateTime(2017, 2, 15), new DateTime(2017, 2, 16) });

            // Act
            var result = (Generator.Course)course;

            // Assert
            Assert.AreEqual("ENDEVN", result.Code);
            Assert.AreEqual(1, result.Priority);
            Assert.AreEqual(new DateTime(2017, 2, 14), result.CourseImplementations.First().Days.ElementAt(0));
            Assert.AreEqual(new DateTime(2017, 2, 15), result.CourseImplementations.First().Days.ElementAt(1));
            Assert.AreEqual(new DateTime(2017, 2, 16), result.CourseImplementations.First().Days.ElementAt(2));
        }

        [TestMethod]
        public void MarkAllImplementations_TwoImplementations()
        {
            // Arrange
            Generator.Course course = CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 3, 8), new DateTime(2017, 3, 9) }, Status.UNKNOWN,
                new DateTime[] { new DateTime(2017, 1, 5), new DateTime(2017, 1, 6) }, Status.UNKNOWN);

            // Act
            course.MarkAllImplementations(Status.AVAILABLE);

            // Assert
            Assert.AreEqual(Status.AVAILABLE, course.CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.AVAILABLE, course.CourseImplementations.ElementAt(1).Status);
        }


        [TestMethod]
        public void HasOneAvailableImplementation_OnePlannedCourse_NoAvailableImplementations()
        {
            // Arrange
            Generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>()
            {
                CreateNewGeneratorCourseWithOneCourseImplementationAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }, Status.PLANNED),
            };

            // Act
            var result = course.HasOneAvailableImplementation(coursesPlanned);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasOneAvailableImplementation_OnePlannedCourse_OneAvailableImplementation()
        {
            // Arrange
            Generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>()
            {
                CreateNewGeneratorCourseWithOneCourseImplementationAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }, Status.AVAILABLE),
            };

            // Act
            var result = course.HasOneAvailableImplementation(coursesPlanned);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HasOneAvailableImplementation_NoPlannedCourse_TwovailableImplementations()
        {
            // Arrange
            Generator.Course course = CreateNewGeneratorCourseWithTwoCourseImplementations("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 4, 17), new DateTime(2017, 4, 18), new DateTime(2017, 4, 19) },
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>();

            // Act
            var result = course.HasOneAvailableImplementation(coursesPlanned);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasOneAvailableImplementation_FourPlannedCourse_OnevailableImplementations()
        {
            // Arrange
            Generator.Course course = CreateNewGeneratorCourseWithTwoCourseImplementations("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 3, 7), new DateTime(2017, 3, 8), new DateTime(2017, 3, 9) },
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>()
            {
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ALMUVS", 1,
                new DateTime[] { new DateTime(2017, 3, 8), new DateTime(2017, 3, 9)}, Status.PLANNED,
                new DateTime[] { new DateTime(2017, 5, 15), new DateTime(2017, 5, 16), new DateTime(2017, 5, 17)}, Status.NOTPLANNED),
                CreateNewGeneratorCourseWithOneCourseImplementationAndStatus("MS20461", 1,
                new DateTime[] { new DateTime(2017, 3, 7) }, Status.PLANNED),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }, Status.PLANNED,
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }, Status.NOTPLANNED),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3)}, Status.NOTPLANNED,
                new DateTime[] { new DateTime(2017, 2, 13), new DateTime(2017, 2, 14)}, Status.PLANNED),
            };

            // Act
            var result = course.HasOneAvailableImplementation(coursesPlanned);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HasOneAvailableImplementation_FourPlannedCourse_TwoAvailableImplementations()
        {
            // Arrange
            Generator.Course course = CreateNewGeneratorCourseWithTwoCourseImplementations("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 3, 8), new DateTime(2017, 3, 9) },
                new DateTime[] { new DateTime(2017, 1, 5), new DateTime(2017, 1, 6) });

            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>()
            {
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ALMUVS", 1,
                new DateTime[] { new DateTime(2017, 3, 8), new DateTime(2017, 3, 9)}, Status.NOTPLANNED,
                new DateTime[] { new DateTime(2017, 5, 15), new DateTime(2017, 5, 16), new DateTime(2017, 5, 17)}, Status.PLANNED),
                CreateNewGeneratorCourseWithOneCourseImplementationAndStatus("MS20461", 1,
                new DateTime[] { new DateTime(2017, 3, 7) }, Status.PLANNED),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }, Status.PLANNED,
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }, Status.NOTPLANNED),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3)}, Status.NOTPLANNED,
                new DateTime[] { new DateTime(2017, 2, 13), new DateTime(2017, 2, 14)}, Status.PLANNED),
            };

            // Act
            var result = course.HasOneAvailableImplementation(coursesPlanned);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasAvailableImplementations_OnePlannedCourse_NoAvailableImplementations()
        {
            // Arrange
            Generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>()
            {
                CreateNewGeneratorCourseWithOneCourseImplementationAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }, Status.PLANNED),
            };

            // Act
            var result = course.HasAvailableImplementations(coursesPlanned);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasAvailableImplementations_OnePlannedCourse_OneAvailableImplementation()
        {
            // Arrange
            Generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>()
            {
                CreateNewGeneratorCourseWithOneCourseImplementationAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }, Status.AVAILABLE),
            };

            // Act
            var result = course.HasAvailableImplementations(coursesPlanned);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(AmountImplementationException))]
        public void MarkOnlyAvailableImplementationPlanned_NoPlannedCourse_TwovailableImplementations_ExceptionThrowed()
        {
            // Arrange
            Generator.Course course = CreateNewGeneratorCourseWithTwoCourseImplementations("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 4, 17), new DateTime(2017, 4, 18), new DateTime(2017, 4, 19) },
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>();

            // Act
            course.MarkOnlyAvailableImplementationPlanned(coursesPlanned);

            // Assert throw AmountImplementationException
        }

        [TestMethod]
        [ExpectedException(typeof(AmountImplementationException))]
        public void MarkOnlyAvailableImplementationPlanned_OnePlannedCourse_NoAvailableImplementations_ExceptionThrowed()
        {
            // Arrange
            Generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>()
            {
                CreateNewGeneratorCourseWithOneCourseImplementationAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }, Status.PLANNED),
            };

            // Act
            course.MarkOnlyAvailableImplementationPlanned(coursesPlanned);

            // Assert throw AmountImplementationException
        }

        [TestMethod]
        public void MarkOnlyAvailableImplementationPlanned_OnePlannedCourse_OneAvailableImplementation_SetToPlanned()
        {
            // Arrange
            Generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>()
            {
                CreateNewGeneratorCourseWithOneCourseImplementationAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }, Status.AVAILABLE),
            };

            // Act
            course.MarkOnlyAvailableImplementationPlanned(coursesPlanned);

            // Assert
            Assert.AreEqual(Status.PLANNED, course.CourseImplementations.ElementAt(0).Status);
        }

        [TestMethod]
        public void MarkOnlyAvailableImplementationPlanned_OneAvailableImplementation_SetToPlanned()
        {
            // Arrange
            Generator.Course course = CreateNewGeneratorCourseWithTwoCourseImplementations("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 3, 7), new DateTime(2017, 3, 8), new DateTime(2017, 3, 9) },
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>()
            {
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ALMUVS", 1,
                new DateTime[] { new DateTime(2017, 3, 8), new DateTime(2017, 3, 9)}, Status.PLANNED,
                new DateTime[] { new DateTime(2017, 5, 15), new DateTime(2017, 5, 16), new DateTime(2017, 5, 17)}, Status.NOTPLANNED),
                CreateNewGeneratorCourseWithOneCourseImplementationAndStatus("MS20461", 1,
                new DateTime[] { new DateTime(2017, 3, 7) }, Status.PLANNED),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }, Status.PLANNED,
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }, Status.NOTPLANNED),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3)}, Status.NOTPLANNED,
                new DateTime[] { new DateTime(2017, 2, 13), new DateTime(2017, 2, 14)}, Status.PLANNED),
            };

            // Act
            course.MarkOnlyAvailableImplementationPlanned(coursesPlanned);

            // Assert
            Assert.AreEqual(Status.UNKNOWN, course.CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, course.CourseImplementations.ElementAt(1).Status);
        }

        [TestMethod]
        [ExpectedException(typeof(AmountImplementationException))]
        public void MarkMinimumIntersectedFirstAvailableImplementationPlanned_OnePlannedCourse_NoAvailableImplementations_ExceptionThrowed()
        {
            // Arrange
            Generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>()
            {
                CreateNewGeneratorCourseWithOneCourseImplementationAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }, Status.PLANNED),
            };

            // Act
            course.MarkMinimumIntersectedFirstAvailableImplementationPlanned(coursesPlanned);

            // Assert throw AmountImplementationException
        }

        [TestMethod]
        public void MarkMinimumIntersectedFirstAvailableImplementationPlanned_NoPlannedCourse_Order_TwovailableImplementations()
        {
            // Arrange
            Generator.Course course = CreateNewGeneratorCourseWithTwoCourseImplementations("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 4, 17), new DateTime(2017, 4, 18), new DateTime(2017, 4, 19) },
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>();

            // Act
            course.MarkMinimumIntersectedFirstAvailableImplementationPlanned(coursesPlanned);

            // Assert
            Assert.AreEqual(Status.UNKNOWN, course.CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, course.CourseImplementations.ElementAt(1).Status);
        }

        [TestMethod]
        public void MarkMinimumIntersectedFirstAvailableImplementationPlanned_TwoPlannedCourse_FirstImplementationHasIntersectedCourseWithFreeImplementation_FirstImplementationPlanned()
        {
            // Arrange
            Generator.Course course = CreateNewGeneratorCourseWithTwoCourseImplementations("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) },
                new DateTime[] { new DateTime(2017, 4, 17), new DateTime(2017, 4, 18), new DateTime(2017, 4, 19) });

            IEnumerable<Generator.Course> coursesPlanned = new List<Generator.Course>()
            {
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) }, Status.AVAILABLE,
                new DateTime[] { new DateTime(2017, 4, 19), new DateTime(2017, 4, 20), new DateTime(2017, 4, 21) }, Status.AVAILABLE),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3)}, Status.AVAILABLE,
                new DateTime[] { new DateTime(2017, 2, 13), new DateTime(2017, 2, 14)}, Status.AVAILABLE),
            };

            // Act
            course.MarkMinimumIntersectedFirstAvailableImplementationPlanned(coursesPlanned);

            // Assert
            Assert.AreEqual(Status.PLANNED, course.CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.UNKNOWN, course.CourseImplementations.ElementAt(1).Status);
        }

        [TestMethod]
        public void GetPlannedImplementation_Order()
        {
            // Arrange
            Generator.Course course = CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 4, 17), new DateTime(2017, 4, 18), new DateTime(2017, 4, 19) }, Status.NOTPLANNED,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) }, Status.PLANNED);

            // Act
            var result = course.PlannedImplementation;

            // Assert
            Assert.AreEqual(new DateTime(2017, 1, 3), result.StartDay);
        }

        [TestMethod]
        public void GetPlannedImplementation_NoPlannedImplementation_ExceptionThrowed()
        {
            // Arrange
            Generator.Course course = CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 4, 17), new DateTime(2017, 4, 18), new DateTime(2017, 4, 19) }, Status.AVAILABLE,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) }, Status.AVAILABLE);

            // Act
            var result = course.PlannedImplementation;

            // Assert 
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AddIntersectedCourses_AddTwoIntersectedCourses()
        {
            // Arrange
            Generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementationAndStatus("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) }, Status.PLANNED);

            IEnumerable<Generator.Course> intersectedCourses = new List<Generator.Course>()
            {
                CreateNewGeneratorCourseWithOneCourseImplementationAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }, Status.PLANNED),
                CreateNewGeneratorCourseWithOneCourseImplementationAndStatus("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) }, Status.PLANNED),
            };

            // Act
            course.AddIntersectedCourses(intersectedCourses);

            // Assert
            Assert.AreEqual(2, course.IntersectedCourseIds.Count());
            Assert.AreEqual("SCRUMES", course.IntersectedCourseIds.ElementAt(0));
            Assert.AreEqual("ENEST", course.IntersectedCourseIds.ElementAt(1));
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Intersects_Course_null_ExceptionThrowed()
        {
            // Arrange
            Generator.Course course = CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 4, 17), new DateTime(2017, 4, 18), new DateTime(2017, 4, 19) }, Status.NOTPLANNED,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) }, Status.PLANNED);

            // Act
            course.Intersects(null);

            // Assert ArgumentNullException
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IntersectsNotUnplannable_Course_null_ExceptionThrowed()
        {
            // Arrange
            Generator.Course course = CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 4, 17), new DateTime(2017, 4, 18), new DateTime(2017, 4, 19) }, Status.NOTPLANNED,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) }, Status.PLANNED);

            // Act
            course.IntersectsNotUnplannable(null);

            // Assert ArgumentNullException
        }
    }
}
