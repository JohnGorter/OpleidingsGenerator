using System;
using System.Collections.Generic;
using System.Linq;
using InfoSupport.KC.OpleidingsplanGenerator.Models;
using log4net;
using System.Globalization;
using System.Diagnostics;

namespace InfoSupport.KC.OpleidingsplanGenerator.Generator
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
        public IEnumerable<Generator.CourseImplementation> CourseImplementations { get; set; }
        public IEnumerable<string> IntersectedCourseIds { get; private set; }

        public CourseImplementation PlannedImplementation
        {
            get
            {
                return this.CourseImplementations.FirstOrDefault(course => course.Status == Status.PLANNED);
            }
        }

        public static explicit operator Course(Models.Course course)
        {
            _logger.Debug(string.Format(_culture, "Cast course {0}", course.Code));
            List<Generator.CourseImplementation> courseImplementations = new List<CourseImplementation>();

            if (course.CourseImplementations != null)
            {
                foreach (var implementations in course.CourseImplementations)
                {
                    courseImplementations.Add((Generator.CourseImplementation)implementations);
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

        public CourseImplementation getImplementationByWeekNumber(Int32 WeekNmber) {
            foreach (var ci in this.CourseImplementations)
                if (GetWeekNumberFromDateTime(ci.StartDay) == WeekNmber) return ci;
            return null;
        }

        private Int32 GetWeekNumberFromDateTime(DateTime time) {
                // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
                // be the same week# as whatever Thursday, Friday or Saturday are,
                // and we always get those right
                DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
                if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
                {
                    time = time.AddDays(3);
                }

                // Return the week of our adjusted day
                return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
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

        public bool HasOneAvailableImplementation(IEnumerable<Generator.Course> courses)
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
        
        public void MarkAllIntersectedOfPlannedImplementations(Status status, IEnumerable<Generator.Course> courses)
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

            System.Diagnostics.Debug.WriteLine("COURSE: " + this.Code);
            foreach (var implementation in intersectedCourseImplementations)
            {
                var target = courses.FirstOrDefault(c => c.CourseImplementations.Contains(implementation));
                Debug.WriteLine("invalidates course " + target?.Code + " of " + implementation.Location + ", " + implementation.StartDay);
                implementation.Status = status;
            }

        }

        public bool HasAvailableImplementations(IEnumerable<Course> plannedCourses)
        {
            _logger.Debug(string.Format(_culture, "HasAvailableImplementations in course {0}", Code));
            return GetCourseAvailableImplementation(plannedCourses).Any();
        }

        public void MarkOnlyAvailableImplementationPlanned(IEnumerable<Generator.Course> courses)
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
        public void MarkMinimumIntersectedFirstAvailableImplementationPlanned(IEnumerable<Generator.Course> courses)
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

        public void AddIntersectedCourses(IEnumerable<Generator.Course> plannedCourses)
        {
            _logger.Debug(string.Format(_culture, "AddIntersectedCourses in course {0}", Code));
            this.IntersectedCourseIds = plannedCourses.Where(course => course.Intersects(this)).Select(course => course.Code).ToList();
        }

        public bool Intersects(Generator.Course course)
        {
            _logger.Debug(string.Format(_culture, "Intersects in course {0}", Code));
            if (course == null)
            {
                _logger.Error("ArgumentNullException course");
                throw new ArgumentNullException("course");
            }

            return course.CourseImplementations.Any(courseImplementation => courseImplementation.Intersects(this.CourseImplementations));
        }

        public bool IntersectsNotUnplannable(Generator.Course course)
        {
            _logger.Debug(string.Format(_culture, "IntersectsNotUnplannable in course {0}", Code));
            if (course == null)
            {
                _logger.Error("ArgumentNullException course");
                throw new ArgumentNullException("course");
            }

            return course.CourseImplementations.Any(courseImplementation => courseImplementation.Intersects(this.CourseImplementations) && courseImplementation.Status != Status.UNPLANNABLE);
        }

        public bool IntersectsWithPlanned(Generator.Course course)
        {
            _logger.Debug(string.Format(_culture, "IntersectsWithPlanned in course {0}", Code));
            if (course == null)
            {
                _logger.Error("ArgumentNullException course");
                throw new ArgumentNullException("course");
            }

            return course.PlannedImplementation.Intersects((this.CourseImplementations));
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

            //try to get the best possible option (the implementation with the least intersected courses)
            var implementations = availableImplementations.Select(ci => new { ci = ci, intersects = ci.GetIntersectedImplementationsWithStatus(courses, Status.AVAILABLE).Count() }).OrderBy(ci => ci.intersects);

            foreach (var courseImplementation in implementations)
            {
                if(courseImplementation.ci.IsPlannable(courses, this.Priority, this.Code))
                {
                    courseImplementation.ci.Status = Status.PLANNED;
                    return;
                }
            }
        }

        private IEnumerable<Generator.Course> GetIntersectedCoursesWithEqualOrHigherPriority(IEnumerable<Course> courses)
        {
            _logger.Debug(string.Format(_culture, "GetIntersectedCoursesWithEqualOrHigherPriority in course {0}", Code));
            return courses.Where(course => course.IntersectsNotUnplannable(this) && course.Priority <= this.Priority).ToList();
        }

        private IEnumerable<Generator.CourseImplementation> GetCourseAvailableImplementation(IEnumerable<Generator.Course> courses)
        {
            _logger.Debug(string.Format(_culture, "GetCourseAvailableImplementation in course {0}", Code));
            return this.CourseImplementations
                .Where(courseImplementation => !courseImplementation.IntersectsWithStatus(courses, Status.PLANNED) && courseImplementation.Status != Status.UNPLANNABLE);
        }
    }

}