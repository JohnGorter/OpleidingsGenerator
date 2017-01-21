using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public class CourseImplementation
    {
        private static ILog _logger = LogManager.GetLogger(typeof(CourseImplementation));
        private static readonly CultureInfo _culture = new CultureInfo("nl-NL");

        public string Location { get; set; }
        public DateTime StartDay { get { return Days.FirstOrDefault(); }}
        public IEnumerable<DateTime> Days { get; set; }
        public Status Status { get; set; }

        public static explicit operator CourseImplementation(models.CourseImplementation courseImplementation)
        {
            _logger.Debug(string.Format(_culture, "Cast course implementation"));
            return new CourseImplementation
            {
                Location = courseImplementation.Location,
                Days = courseImplementation.Days,
                Status = Status.UNKNOWN,
            };
        }

        public bool Intersects(IEnumerable<CourseImplementation> courseImplementations)
        {
            _logger.Debug(string.Format(_culture, "Intersects course implementations"));
            return courseImplementations.Any(courseImplementation => courseImplementation.Days.Any(day => Days.Contains(day)));
        }

        public bool Intersects(IEnumerable<Course> courses)
        {
            _logger.Debug(string.Format(_culture, "Intersects courses"));
            return courses
                .SelectMany(course => course.CourseImplementations)
                .Any(courseImplementation => courseImplementation.Days.Any(day => Days.Contains(day)));
        }

        public bool IntersectsWithStatus(IEnumerable<Course> courses, Status status)
        {
            _logger.Debug(string.Format(_culture, "IntersectsWithStatus {0}", status));
            return GetIntersectedImplementationsWithStatus(courses, status).Any();
        }

        public int IntersectsWithStatusCount(IEnumerable<Course> courses, Status status)
        {
            _logger.Debug(string.Format(_culture, "IntersectsWithStatusCount {0}", status));
            return GetIntersectedImplementationsWithStatus(courses, status).Count();
        }

        public IEnumerable<CourseImplementation> GetIntersectedCourseImplementations(IEnumerable<Course> courses)
        {
            _logger.Debug(string.Format(_culture, "GetIntersectedCourseImplementations"));
            return courses
                 .SelectMany(course => course.CourseImplementations)
                 .Where(courseImplementation => courseImplementation.Days.Any(day => Days.Contains(day)));
        }

        public bool IsPlannable(IEnumerable<Course> courses, int priority, string code)
        {
            _logger.Debug(string.Format(_culture, "IsPlannable"));

            if (!this.Intersects(courses))
            {
                _logger.Debug(string.Format(_culture, "Course with code {0} and startdate {1} doesn't intersect with other course", code, StartDay.ToString("dd-MM-yyyy")));
                return true;
            }

            if(this.IntersectsOnlyWithStatus(courses, Status.UNPLANNABLE))
            {
                _logger.Debug(string.Format(_culture, "Course with code {0} and startdate {1} intersects only with unplannable implementations", code, StartDay.ToString("dd-MM-yyyy")));
                return true;
            }

            if (this.IntersectsWithStatus(courses, Status.PLANNED))
            {
                _logger.Debug(string.Format(_culture, "Course with code {0} and startdate {1} intersects with planned implementation", code, StartDay.ToString("dd-MM-yyyy")));
                return false;
            }

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

            _logger.Debug(string.Format(_culture, "Course with code {0} and startdate {1} returns IsPlannable", code, StartDay.ToString("dd-MM-yyyy")));

            return IsPlannable(coursesToCheck, scannedCourses, priority);

        }

        private bool IsPlannable(IEnumerable<Course> courses, List<string> scannedCourses, int priority)
        {
            _logger.Debug(string.Format(_culture, "private IsPlannable with startdate {0}", StartDay.ToString("dd-MM-yyyy")));
            bool plannable = false;

            if (this.GetIntersectedCourseImplementations(courses).All(ci => ci.Status == Status.PLANNED))
            {
                _logger.Debug(string.Format(_culture, "All intersected course implementations are planned"));
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
                    _logger.Debug(string.Format(_culture, "Inersected course {0} intersects with course with free implementation", intersectedCourse.Code));
                    return true;
                }

                if (!plannable)
                {
                    _logger.Debug(string.Format(_culture, "plannable is false recursive IsPlannable all implementations from instersected course {0}", intersectedCourse.Code));
                    plannable = intersectedCourse.CourseImplementations.Any(ci => ci.IsPlannable(courses, scannedCourses, priority));
                }
            }

            _logger.Debug(string.Format(_culture, "return plannable {0}", plannable));
            return plannable;
        }

        private IEnumerable<Course> GetIntersectedCourses(IEnumerable<Course> courses)
        {
            _logger.Debug(string.Format(_culture, "GetIntersectedCourses"));
            return courses
                .Where(course => course.CourseImplementations.Any(courseImplementation => courseImplementation.Days.Any(day => Days.Contains(day))));
        }

        private IEnumerable<CourseImplementation> GetIntersectedImplementationsWithStatus(IEnumerable<Course> courses, Status status)
        {
            _logger.Debug(string.Format(_culture, "GetIntersectedImplementationsWithStatus {0}", status));
            return GetIntersectedCourseImplementations(courses).Where(courseImplementation => courseImplementation.Status == status);
        }

        private bool IntersectsOnlyWithStatus(IEnumerable<Course> courses, Status status)
        {
            _logger.Debug(string.Format(_culture, "IntersectsOnlyWithStatus {0}", status));
            return GetIntersectedCourseImplementations(courses).All(courseImplementation => courseImplementation.Status == status);
        }
    }
}
