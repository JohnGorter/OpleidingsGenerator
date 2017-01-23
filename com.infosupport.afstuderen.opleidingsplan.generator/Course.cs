using System;
using System.Collections.Generic;
using System.Linq;
using com.infosupport.afstuderen.opleidingsplan.models;
using log4net;
using System.Globalization;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public class Course
    {
        private static ILog _logger = LogManager.GetLogger(typeof(Course));
        private static readonly CultureInfo _culture = new CultureInfo("nl-NL");

        public string Code { get; set; }
        public string Commentary { get; set; }
        public int Priority { get; set; }
        public int? Duration { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<generator.CourseImplementation> CourseImplementations { get; set; }
        public IEnumerable<string> IntersectedCourseIds { get; private set; }

        public CourseImplementation PlannedImplementation
        {
            get
            {
                return this.CourseImplementations.FirstOrDefault(course => course.Status == Status.PLANNED);
            }
        }

        public static explicit operator Course(models.Course course)
        {
            _logger.Debug(string.Format(_culture, "Cast course {0}", course.Code));
            List<generator.CourseImplementation> courseImplementations = new List<CourseImplementation>();

            if (course.CourseImplementations != null)
            {
                foreach (var implementations in course.CourseImplementations)
                {
                    courseImplementations.Add((generator.CourseImplementation)implementations);
                }
            }

            return new Course
            {
                Code = course.Code,
                CourseImplementations = courseImplementations,
                Commentary = course.Commentary,
                Priority = course.Priority,
                Duration = course.Duration?.ToInt(),
                Name = course.Name,
                Price = course.Price,           
            };
        }

        public void MarkAllImplementations(Status status)
        {
            _logger.Debug(string.Format(_culture, "Mark all implementations from course {0} to {1}", Code, status));
            foreach (var implementation in this.CourseImplementations)
            {
                if(implementation.Status != Status.UNPLANNABLE)
                {
                    implementation.Status = status;
                }
            }
        }

        public bool HasOneAvailableImplementation(IEnumerable<generator.Course> courses)
        {
            _logger.Debug(string.Format(_culture, "HasOneAvailableImplementation in course {0}", Code));
            return GetCourseAvailableImplementation(courses).Count() == 1;
        }

        public bool HasOnlyImplementationsWithStatus(Status status)
        {
            _logger.Debug(string.Format(_culture, "HasOnlyImplementationsWithStatus with status {0} in course {1}", status, Code));
            return this.CourseImplementations.All(courseImplementation => courseImplementation.Status == status);
        }

        internal bool HasMultipleImplementationsWithStatus(Status status)
        {
            _logger.Debug(string.Format(_culture, "HasMultipleImplementationsWithStatus with status {0} in course {1}", status, Code));
            return this.CourseImplementations.Count(courseImplementation => courseImplementation.Status == status) > 1;
        }

        public bool HasOneImplementation()
        {
            _logger.Debug(string.Format(_culture, "HasOneImplementation in course {0}", Code));
            return this.CourseImplementations.Count(ci => ci.Status != Status.UNPLANNABLE) == 1;
        }
        
        public void MarkAllIntersectedOfPlannedImplementations(Status status, IEnumerable<generator.Course> courses)
        {
            _logger.Debug(string.Format(_culture, "MarkAllIntersectedOfPlannedImplementations to status {0} in course {1}", status, Code));
            var plannedCourseImplementation = this.CourseImplementations.FirstOrDefault(courseImplementation => courseImplementation.Status == Status.PLANNED);

            if(plannedCourseImplementation == null)
            {
                string errorMessage = string.Format(_culture, "There is no planned implementation in course {0}", Code);
                _logger.Error("AmountImplementationException: " + errorMessage);
                throw new AmountImplementationException(errorMessage);
            }

            var courseWithouSelf = courses.Where(course => course.Code != this.Code);
            var intersectedCourseImplementations = plannedCourseImplementation.GetIntersectedCourseImplementations(courseWithouSelf);

            foreach (var implementation in intersectedCourseImplementations)
            {
                implementation.Status = status;
            }

        }

        public bool HasAvailableImplementations(IEnumerable<Course> plannedCourses)
        {
            _logger.Debug(string.Format(_culture, "HasAvailableImplementations in course {0}", Code));
            return GetCourseAvailableImplementation(plannedCourses).Any();
        }

        public void MarkOnlyAvailableImplementationPlanned(IEnumerable<generator.Course> courses)
        {
            _logger.Debug(string.Format(_culture, "MarkOnlyAvailableImplementationPlanned in course {0}", Code));
            if (!HasOneAvailableImplementation(courses))
            {
                string errorMessage = string.Format(_culture, "There is not exactly one implementation available in course {0}", Code);
                _logger.Error("AmountImplementationException: " + errorMessage);
                throw new AmountImplementationException(errorMessage);
            }

            var availableImplementation = GetCourseAvailableImplementation(courses).First();
            availableImplementation.Status = Status.PLANNED;
        }

        /// <summary>
        /// If an implementation that intersects with a course that has an implementation without intersection exists, it will mark that implementation to Plannend
        /// else it will mark the first minimum intersected implementation to Planned
        /// </summary>
        /// <param name="courses"></param>
        public void MarkMinimumIntersectedFirstAvailableImplementationPlanned(IEnumerable<generator.Course> courses)
        {
            _logger.Debug(string.Format(_culture, "MarkMinimumIntersectedFirstAvailableImplementationPlanned in course {0}", Code));
            if (GetCourseAvailableImplementation(courses).None())
            {
                string errorMessage = string.Format(_culture, "No available implementations in course {0}", Code);
                _logger.Error("AmountImplementationException: " + errorMessage);
                throw new AmountImplementationException(errorMessage);
            }

            var availableImplementations = GetCourseAvailableImplementation(courses).OrderBy(courseImplementation => courseImplementation.StartDay);

            MarkFirstImplementationThatIntersectsCourseWithOneFreeImplementation(courses, availableImplementations);

            if(PlannedImplementation == null)
            {
                _logger.Debug("No planned implementation");

                var availableImplementation = GetCourseAvailableImplementation(courses)
                    .OrderBy(courseImplementation => courseImplementation.IntersectsWithStatusCount(courses, Status.AVAILABLE))
                    .ThenBy(courseImplementation => courseImplementation.StartDay)
                    .First();

                _logger.Debug(string.Format(_culture, "Set implementation to planned from course {0} and date {1}", Code, availableImplementation.StartDay.ToString("dd-MM-yyyy")));

                availableImplementation.Status = Status.PLANNED;
            }
        }

        public void AddIntersectedCourses(IEnumerable<generator.Course> plannedCourses)
        {
            _logger.Debug(string.Format(_culture, "AddIntersectedCourses in course {0}", Code));
            this.IntersectedCourseIds = plannedCourses.Where(course => course.Intersects(this)).Select(course => course.Code).ToList();
        }

        public bool Intersects(generator.Course course)
        {
            _logger.Debug(string.Format(_culture, "Intersects in course {0}", Code));
            if (course == null)
            {
                _logger.Error("ArgumentNullException course");
                throw new ArgumentNullException("course");
            }

            return course.CourseImplementations.Any(courseImplementation => courseImplementation.Intersects(this.CourseImplementations));
        }

        public bool IntersectsNotUnplannable(generator.Course course)
        {
            _logger.Debug(string.Format(_culture, "IntersectsNotUnplannable in course {0}", Code));
            if (course == null)
            {
                _logger.Error("ArgumentNullException course");
                throw new ArgumentNullException("course");
            }

            return course.CourseImplementations.Any(courseImplementation => courseImplementation.Intersects(this.CourseImplementations) && courseImplementation.Status != Status.UNPLANNABLE);
        }

        public bool IsPlannable(IEnumerable<Course> courses)
        {
            _logger.Debug(string.Format(_culture, "IsPlannable in course {0}", Code));
            return this.CourseImplementations.Any(courseImplementation => courseImplementation.IsPlannable(courses, this.Priority, this.Code) && courseImplementation.Status != Status.UNPLANNABLE);
        }

        public bool HasIntersectedCourseWithFreeImplementation(IEnumerable<Course> courses, int priority)
        {
            _logger.Debug(string.Format(_culture, "HasIntersectedCourseWithFreeImplementation in course {0}", Code));

            var intersectedCourses = this.GetIntersectedCoursesWithEqualOrHigherPriority(courses)
                .Where(course => 
                course.Code != this.Code && 
                !course.CourseImplementations.Any(ci => ci.Status == Status.PLANNED) && 
                !course.CourseImplementations.Any(ci => ci.Status == Status.UNPLANNABLE));

            return intersectedCourses.Any(course => course.HasImplementationsWithoutIntersection(courses, priority));
        }

        public bool HasImplementationsWithoutIntersection(IEnumerable<Course> courses, int priority)
        {
            _logger.Debug(string.Format(_culture, "HasImplementationsWithoutIntersection in course {0}", Code));
            var coursesWithoutSelf = courses.Where(course => course.Code != this.Code && this.Priority <= priority);
            return this.CourseImplementations.Any(courseImplementation => !courseImplementation.Intersects(coursesWithoutSelf));
        }


        /// <summary>
        /// Marks the first implementation that intersects with a course that has an implementation without intersection
        /// </summary>
        private void MarkFirstImplementationThatIntersectsCourseWithOneFreeImplementation(IEnumerable<Course> courses, IEnumerable<CourseImplementation> availableImplementations)
        {
            _logger.Debug(string.Format(_culture, "MarkFirstImplementationThatIntersectsCourseWithOneFreeImplementation in course {0}", Code));
            foreach (var courseImplementation in availableImplementations)
            {
                if(courseImplementation.IsPlannable(courses, this.Priority, this.Code))
                {
                    courseImplementation.Status = Status.PLANNED;
                    return;
                }
            }
        }

        private IEnumerable<generator.Course> GetIntersectedCoursesWithEqualOrHigherPriority(IEnumerable<Course> courses)
        {
            _logger.Debug(string.Format(_culture, "GetIntersectedCoursesWithEqualOrHigherPriority in course {0}", Code));
            return courses.Where(course => course.IntersectsNotUnplannable(this) && course.Priority <= this.Priority).ToList();
        }

        private IEnumerable<generator.CourseImplementation> GetCourseAvailableImplementation(IEnumerable<generator.Course> courses)
        {
            _logger.Debug(string.Format(_culture, "GetCourseAvailableImplementation in course {0}", Code));
            return this.CourseImplementations
                .Where(courseImplementation => !courseImplementation.IntersectsWithStatus(courses, Status.PLANNED) && courseImplementation.Status != Status.UNPLANNABLE);
        }
    }

}