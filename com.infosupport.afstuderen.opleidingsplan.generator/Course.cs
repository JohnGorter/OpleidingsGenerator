using System;
using System.Collections.Generic;
using System.Linq;
using com.infosupport.afstuderen.opleidingsplan.models;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public class Course
    {
        public string Code { get; set; }
        public int Priority { get; set; }
        public int? Duration { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<generator.CourseImplementation> CourseImplementations { get; set; }
        public IEnumerable<string> IntersectedCourseIds { get; private set; }

        public static explicit operator Course(models.Course course)
        {
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
                Priority = course.Priority,
                Duration = course.Duration?.ToInt(),
                Name = course.Name,
                Price = course.Price,           
            };
        }

        public void MarkAllImplementations(Status status)
        {
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
            return GetCourseAvailableImplementation(courses).Count() == 1;
        }

        //TODO: Test
        public bool HasOnlyImplementationsWithStatus(Status status)
        {
            return this.CourseImplementations.All(courseImplementation => courseImplementation.Status == status);
        }

        //TODO: Test
        public bool HasOneImplementation()
        {
            return this.CourseImplementations.Count() == 1;
        }
        
        //TODO: Test
        public void MarkAllPlannedIntersectedImplementations(Status status, IEnumerable<generator.Course> courses)
        {
            var plannedCourseImplementation = this.CourseImplementations.FirstOrDefault(courseImplementation => courseImplementation.Status == Status.PLANNED);

            if(plannedCourseImplementation == null)
            {
                throw new AmountImplementationException("There is no planned implementation");
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
            return GetCourseAvailableImplementation(plannedCourses).Any();
        }

        public void MarkOnlyAvailableImplementationPlanned(IEnumerable<generator.Course> courses)
        {
            if(!HasOneAvailableImplementation(courses))
            {
                throw new AmountImplementationException("There is not exactly one implementation available.");
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
            if (GetCourseAvailableImplementation(courses).None())
            {
                throw new AmountImplementationException("No available implementations");
            }

            var availableImplementations = GetCourseAvailableImplementation(courses).OrderBy(courseImplementation => courseImplementation.StartDay);

            MarkFirstImplementationThatIntersectsCourseWithOneFreeImplementation(courses, availableImplementations);

            if(GetPlannedImplementation() == null)
            {
                var availableImplementation = GetCourseAvailableImplementation(courses)
                    .OrderBy(courseImplementation => courseImplementation.IntersectsWithStatusCount(courses, Status.AVAILABLE))
                    .ThenBy(courseImplementation => courseImplementation.StartDay)
                    .First();
                availableImplementation.Status = Status.PLANNED;
            }
        }

        public CourseImplementation GetPlannedImplementation()
        {
            CourseImplementation plannedCourseImplementation = this.CourseImplementations.FirstOrDefault(course => course.Status == Status.PLANNED);
            return plannedCourseImplementation;
        }

        public void AddIntersectedCourses(IEnumerable<generator.Course> plannedCourses)
        {
            this.IntersectedCourseIds = plannedCourses.Where(course => course.Intersects(this)).Select(course => course.Code).ToList();
        }

        //TODO: Test
        public bool Intersects(generator.Course course)
        {
            return course.CourseImplementations.Any(courseImplementation => courseImplementation.Intersects(this.CourseImplementations));
        }

        //TODO: Test
        public bool IntersectsNotUnplannable(generator.Course course)
        {
            return course.CourseImplementations.Any(courseImplementation => courseImplementation.Intersects(this.CourseImplementations) && courseImplementation.Status != Status.UNPLANNABLE);
        }

        public bool IsPlannable(IEnumerable<Course> courses)
        {
            return this.CourseImplementations.Any(courseImplementation => courseImplementation.IsPlannable(courses, this.Priority, this.Code));
        }

        //TODO: Test
        public bool HasIntersectedCourseWithFreeImplementation(IEnumerable<Course> courses, int priority)
        {
            var intersectedCourses = this.GetIntersectedCoursesWithEqualOrHigherPriority(courses)
                .Where(course => course.Code != this.Code && !course.CourseImplementations.Any(ci => ci.Status == Status.PLANNED));

            return intersectedCourses.Any(course => course.HasImplementationsWithoutIntersection(courses, priority));
        }

        //TODO: Test
        public bool HasImplementationsWithoutIntersection(IEnumerable<Course> courses, int priority)
        {

            //coursesWithoutSelfcoursesWithoutSelfcoursesWithoutSelfcoursesWithoutSelfcoursesWithoutSelfcoursesWithoutSelfcoursesWithoutSelfcoursesWithoutSelf
            var coursesWithoutSelf = courses.Where(course => course.Code != this.Code && this.Priority <= priority);
            return this.CourseImplementations.Any(courseImplementation => !courseImplementation.Intersects(coursesWithoutSelf));
        }


        /// <summary>
        /// Marks the first implementation that intersects with a course that has an implementation without intersection
        /// </summary>
        private void MarkFirstImplementationThatIntersectsCourseWithOneFreeImplementation(IEnumerable<Course> courses, IEnumerable<CourseImplementation> availableImplementations)
        {
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
            return courses.Where(course => course.IntersectsNotUnplannable(this) && course.Priority <= this.Priority).ToList();
        }

        private IEnumerable<generator.CourseImplementation> GetCourseAvailableImplementation(IEnumerable<generator.Course> courses)
        {
            return this.CourseImplementations
                .Where(courseImplementation => !courseImplementation.IntersectsWithStatus(courses, Status.PLANNED));
        }
    }

}