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
        public void HasMultipleAvailableImplementations_OnePlannedCourse_NoAvailableImplementations()
        {
            // Arrange
            generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>()
            {
                CreateNewGeneratorCourseWithOneCourseImplementationAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }, Status.PLANNED),
            };

            // Act
            var result = course.HasMultipleAvailableImplementations(coursesPlanned);

            // Assert
            Assert.IsFalse(result);
        }


        [TestMethod]
        public void HasMultipleAvailableImplementations_OnePlannedCourse_OneAvailableImplementation()
        {
            // Arrange
            generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>()
            {
                CreateNewGeneratorCourseWithOneCourseImplementationAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }, Status.AVAILABLE),
            };

            // Act
            var result = course.HasMultipleAvailableImplementations(coursesPlanned);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasMultipleAvailableImplementations_NoPlannedCourse_TwovailableImplementations()
        {
            // Arrange
            generator.Course course = CreateNewGeneratorCourseWithTwoCourseImplementations("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 4, 17), new DateTime(2017, 4, 18), new DateTime(2017, 4, 19) },
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>();

            // Act
            var result = course.HasMultipleAvailableImplementations(coursesPlanned);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HasMultipleAvailableImplementations_FourPlannedCourse_OnevailableImplementations()
        {
            // Arrange
            generator.Course course = CreateNewGeneratorCourseWithTwoCourseImplementations("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 3, 7), new DateTime(2017, 3, 8), new DateTime(2017, 3, 9) },
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>()
            {
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ALMUVS", 1,
                new DateTime[] { new DateTime(2017, 3, 8), new DateTime(2017, 3, 9)}, Status.PLANNED,
                new DateTime[] { new DateTime(2017, 5, 15), new DateTime(2017, 5, 16), new DateTime(2017, 5, 17)}, Status.NOTUSED),
                CreateNewGeneratorCourseWithOneCourseImplementationAndStatus("MS20461", 1,
                new DateTime[] { new DateTime(2017, 3, 7) }, Status.PLANNED),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }, Status.PLANNED,
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }, Status.NOTUSED),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3)}, Status.NOTUSED,
                new DateTime[] { new DateTime(2017, 2, 13), new DateTime(2017, 2, 14)}, Status.PLANNED),
            };

            // Act
            var result = course.HasMultipleAvailableImplementations(coursesPlanned);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasMultipleAvailableImplementations_FourPlannedCourse_TwoAvailableImplementations()
        {
            // Arrange
            generator.Course course = CreateNewGeneratorCourseWithTwoCourseImplementations("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 3, 8), new DateTime(2017, 3, 9) },
                new DateTime[] { new DateTime(2017, 1, 5), new DateTime(2017, 1, 6) });

            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>()
            {
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ALMUVS", 1,
                new DateTime[] { new DateTime(2017, 3, 8), new DateTime(2017, 3, 9)}, Status.NOTUSED,
                new DateTime[] { new DateTime(2017, 5, 15), new DateTime(2017, 5, 16), new DateTime(2017, 5, 17)}, Status.PLANNED),
                CreateNewGeneratorCourseWithOneCourseImplementationAndStatus("MS20461", 1,
                new DateTime[] { new DateTime(2017, 3, 7) }, Status.PLANNED),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }, Status.PLANNED,
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }, Status.NOTUSED),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3)}, Status.NOTUSED,
                new DateTime[] { new DateTime(2017, 2, 13), new DateTime(2017, 2, 14)}, Status.PLANNED),
            };

            // Act
            var result = course.HasMultipleAvailableImplementations(coursesPlanned);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void MarkAllImplementations_TwoImplementations()
        {
            // Arrange
            generator.Course course = CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ENDEVN", 1,
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
            generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>()
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
            generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>()
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
            generator.Course course = CreateNewGeneratorCourseWithTwoCourseImplementations("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 4, 17), new DateTime(2017, 4, 18), new DateTime(2017, 4, 19) },
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>();

            // Act
            var result = course.HasOneAvailableImplementation(coursesPlanned);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasOneAvailableImplementation_FourPlannedCourse_OnevailableImplementations()
        {
            // Arrange
            generator.Course course = CreateNewGeneratorCourseWithTwoCourseImplementations("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 3, 7), new DateTime(2017, 3, 8), new DateTime(2017, 3, 9) },
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>()
            {
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ALMUVS", 1,
                new DateTime[] { new DateTime(2017, 3, 8), new DateTime(2017, 3, 9)}, Status.PLANNED,
                new DateTime[] { new DateTime(2017, 5, 15), new DateTime(2017, 5, 16), new DateTime(2017, 5, 17)}, Status.NOTUSED),
                CreateNewGeneratorCourseWithOneCourseImplementationAndStatus("MS20461", 1,
                new DateTime[] { new DateTime(2017, 3, 7) }, Status.PLANNED),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }, Status.PLANNED,
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }, Status.NOTUSED),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3)}, Status.NOTUSED,
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
            generator.Course course = CreateNewGeneratorCourseWithTwoCourseImplementations("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 3, 8), new DateTime(2017, 3, 9) },
                new DateTime[] { new DateTime(2017, 1, 5), new DateTime(2017, 1, 6) });

            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>()
            {
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ALMUVS", 1,
                new DateTime[] { new DateTime(2017, 3, 8), new DateTime(2017, 3, 9)}, Status.NOTUSED,
                new DateTime[] { new DateTime(2017, 5, 15), new DateTime(2017, 5, 16), new DateTime(2017, 5, 17)}, Status.PLANNED),
                CreateNewGeneratorCourseWithOneCourseImplementationAndStatus("MS20461", 1,
                new DateTime[] { new DateTime(2017, 3, 7) }, Status.PLANNED),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }, Status.PLANNED,
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }, Status.NOTUSED),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3)}, Status.NOTUSED,
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
            generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>()
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
            generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>()
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
            generator.Course course = CreateNewGeneratorCourseWithTwoCourseImplementations("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 4, 17), new DateTime(2017, 4, 18), new DateTime(2017, 4, 19) },
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>();

            // Act
            course.MarkOnlyAvailableImplementationPlanned(coursesPlanned);

            // Assert throw AmountImplementationException
        }

        [TestMethod]
        [ExpectedException(typeof(AmountImplementationException))]
        public void MarkOnlyAvailableImplementationPlanned_OnePlannedCourse_NoAvailableImplementations_ExceptionThrowed()
        {
            // Arrange
            generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>()
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
            generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>()
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
            generator.Course course = CreateNewGeneratorCourseWithTwoCourseImplementations("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 3, 7), new DateTime(2017, 3, 8), new DateTime(2017, 3, 9) },
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>()
            {
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ALMUVS", 1,
                new DateTime[] { new DateTime(2017, 3, 8), new DateTime(2017, 3, 9)}, Status.PLANNED,
                new DateTime[] { new DateTime(2017, 5, 15), new DateTime(2017, 5, 16), new DateTime(2017, 5, 17)}, Status.NOTUSED),
                CreateNewGeneratorCourseWithOneCourseImplementationAndStatus("MS20461", 1,
                new DateTime[] { new DateTime(2017, 3, 7) }, Status.PLANNED),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 9), new DateTime(2017, 1, 10), new DateTime(2017, 1, 11) }, Status.PLANNED,
                new DateTime[] { new DateTime(2017, 3, 6), new DateTime(2017, 3, 7), new DateTime(2017, 3, 8) }, Status.NOTUSED),
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ENEST", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3)}, Status.NOTUSED,
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
        public void MarkFirstImplementationPlanned_OnePlannedCourse_NoAvailableImplementations_ExceptionThrowed()
        {
            // Arrange
            generator.Course course = CreateNewGeneratorCourseWithOneCourseImplementation("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>()
            {
                CreateNewGeneratorCourseWithOneCourseImplementationAndStatus("SCRUMES", 1,
                new DateTime[] { new DateTime(2017, 1, 2), new DateTime(2017, 1, 3), new DateTime(2017, 1, 4) }, Status.PLANNED),
            };

            // Act
            course.MarkFirstAvailableImplementationPlanned(coursesPlanned);

            // Assert throw AmountImplementationException
        }

        [TestMethod]
        public void MarkFirstImplementationPlanned_NoPlannedCourse_Order_TwovailableImplementations()
        {
            // Arrange
            generator.Course course = CreateNewGeneratorCourseWithTwoCourseImplementations("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 4, 17), new DateTime(2017, 4, 18), new DateTime(2017, 4, 19) },
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) });

            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>();

            // Act
            course.MarkFirstAvailableImplementationPlanned(coursesPlanned);

            // Assert
            Assert.AreEqual(Status.UNKNOWN, course.CourseImplementations.ElementAt(0).Status);
            Assert.AreEqual(Status.PLANNED, course.CourseImplementations.ElementAt(1).Status);
        }

        [TestMethod]
        public void GetPlannedImplementation_Order()
        {
            // Arrange
            generator.Course course = CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 4, 17), new DateTime(2017, 4, 18), new DateTime(2017, 4, 19) }, Status.NOTUSED,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) }, Status.PLANNED);

            // Act
            var result = course.GetPlannedImplementation();

            // Assert
            Assert.AreEqual(new DateTime(2017, 1, 3), result.StartDay);
        }

        [TestMethod]
        [ExpectedException(typeof(AmountImplementationException))]
        public void GetPlannedImplementation_NoPlannedImplementation_ExceptionThrowed()
        {
            // Arrange
            generator.Course course = CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 4, 17), new DateTime(2017, 4, 18), new DateTime(2017, 4, 19) }, Status.AVAILABLE,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) }, Status.AVAILABLE);

            // Act
            var result = course.GetPlannedImplementation();

            // Assert throw AmountImplementationException
        }

        [TestMethod]
        public void GetAvailableIntersectedCoursesWithPlannedImplementation_OnePlannedCourse_OneAvailable()
        {
            // Arrange
            generator.Course course = CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 4, 17), new DateTime(2017, 4, 18), new DateTime(2017, 4, 19) }, Status.PLANNED,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) }, Status.NOTUSED);


            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>()
            {
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ENEST", 1,
                new DateTime[] { new DateTime(2017, 4, 17), new DateTime(2017, 4, 18),}, Status.AVAILABLE,
                new DateTime[] { new DateTime(2017, 2, 13), new DateTime(2017, 2, 14)}, Status.AVAILABLE),
            };

            // Act
            var result = course.GetAvailableIntersectedCoursesWithPlannedImplementation(coursesPlanned);

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void GetAvailableIntersectedCoursesWithPlannedImplementation_OnePlannedCourse_NoAvailable()
        {
            // Arrange
            generator.Course course = CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ENDEVN", 1,
                new DateTime[] { new DateTime(2017, 4, 17), new DateTime(2017, 4, 18), new DateTime(2017, 4, 19) }, Status.PLANNED,
                new DateTime[] { new DateTime(2017, 1, 3), new DateTime(2017, 1, 4), new DateTime(2017, 1, 5) }, Status.NOTUSED);


            IEnumerable<generator.Course> coursesPlanned = new List<generator.Course>()
            {
                CreateNewGeneratorCourseWithTwoCourseImplementationsAndStatus("ENEST", 1,
                new DateTime[] { new DateTime(2017, 4, 17), new DateTime(2017, 4, 18),}, Status.NOTPLANNED,
                new DateTime[] { new DateTime(2017, 2, 13), new DateTime(2017, 2, 14)}, Status.NOTPLANNED),
            };

            // Act
            var result = course.GetAvailableIntersectedCoursesWithPlannedImplementation(coursesPlanned);

            // Assert
            Assert.AreEqual(0, result.Count());
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
