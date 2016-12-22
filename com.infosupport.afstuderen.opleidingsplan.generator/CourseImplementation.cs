using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public class CourseImplementation
    {
        public string Location { get; set; }
        public DateTime StartDay { get { return Days.FirstOrDefault(); } set { } }
        public IEnumerable<DateTime> Days { get; set; }
        public Status Status { get; set; }

        public static explicit operator CourseImplementation(models.CourseImplementation courseImplementation)
        {
            return new CourseImplementation
            {
                Location = courseImplementation.Location,
                Days = courseImplementation.Days,
                Status = Status.UNKNOWN,
            };
        }

        //TODO: Test
        public bool Intersects(IEnumerable<CourseImplementation> courseImplementations)
        {
            return courseImplementations.Any(courseImplementation => courseImplementation.Days.Any(day => Days.Contains(day)));
        }

        //TODO: Test
        public bool Intersects(IEnumerable<Course> courses)
        {
            return courses
                .SelectMany(course => course.CourseImplementations)
                .Any(courseImplementation => courseImplementation.Days.Any(day => Days.Contains(day)));
        }

        //TODO: Test
        public bool IntersectsWithStatus(IEnumerable<Course> courses, Status status)
        {
            return GetIntersectedImplementationsWithStatus(courses, status).Any();
        }

        //TODO: Test
        public int IntersectsWithStatusCount(IEnumerable<Course> courses, Status status)
        {
            return GetIntersectedImplementationsWithStatus(courses, status).Count();
        }

        //TODO: Test
        public IEnumerable<CourseImplementation> GetIntersectedCourseImplementations(IEnumerable<Course> courses)
        {
            return courses
                 .SelectMany(course => course.CourseImplementations)
                 .Where(courseImplementation => courseImplementation.Days.Any(day => Days.Contains(day)));
        }

        public bool IsPlannable(IEnumerable<Course> courses, int priority, string code)
        {
            if(!this.Intersects(courses))
            {
                return true;
            }

            if(this.IntersectsOnlyWithStatus(courses, Status.UNPLANNABLE))
            {
                return true;
            }

            //TODO: Als deze overlapt met een Planned -> return false

            List<string> scannedCourses = new List<string>();

            var coursesToCheck = courses.Where(course => course.Code != code).ToList();
            //Add self for intersection
            coursesToCheck.Add(new Course
            {
                Code = code,
                Priority = priority,
                CourseImplementations = new List<CourseImplementation>()
                {
                    this,
                },
            });
            return IsPlannable(coursesToCheck, scannedCourses, priority);

        }

        private bool IsPlannable(IEnumerable<Course> courses, List<string> scannedCourses, int priority)
        {
            bool plannable = false;

            if (this.GetIntersectedCourseImplementations(courses).All(ci => ci.Status == Status.PLANNED))
            {
                return false;
            }

            var notPlannedIntersectedCourses = this.GetIntersectedCourses(courses)
                .Where(course => course.CourseImplementations.All(ci => ci.Status != Status.PLANNED) && course.Priority <= priority);
            var notPlannedIntersectedCoursesWithoutScanned = notPlannedIntersectedCourses.Where(intersectedCourse => !scannedCourses.Contains(intersectedCourse.Code)).ToList();

            foreach (var intersectedCourse in notPlannedIntersectedCoursesWithoutScanned)
            {
                scannedCourses.Add(intersectedCourse.Code);

                if (intersectedCourse.HasIntersectedCourseWithFreeImplementation(courses, priority))
                {
                    return true;
                }

                if (!plannable)
                {
                    //TODO: ook checken of deze implementation niet unplannable is
                    plannable = intersectedCourse.CourseImplementations.Any(ci => ci.IsPlannable(courses, scannedCourses, priority));
                }
            }

            return plannable;
        }

        private IEnumerable<Course> GetIntersectedCourses(IEnumerable<Course> courses)
        {
            return courses
                .Where(course => course.CourseImplementations.Any(courseImplementation => courseImplementation.Days.Any(day => Days.Contains(day))));
        }

        private IEnumerable<CourseImplementation> GetIntersectedImplementationsWithStatus(IEnumerable<Course> courses, Status status)
        {
            return GetIntersectedCourseImplementations(courses).Where(courseImplementation => courseImplementation.Status == status);
        }

        private bool IntersectsOnlyWithStatus(IEnumerable<Course> courses, Status status)
        {
            return GetIntersectedCourseImplementations(courses).All(courseImplementation => courseImplementation.Status == status);
        }
    }
}
