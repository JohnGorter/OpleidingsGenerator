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

        public bool Intersects(IEnumerable<CourseImplementation> courseImplementations)
        {
            return courseImplementations.Any(courseImplementation => courseImplementation.Days.Any(day => Days.Contains(day)));
        }

        public bool IntersectsPlannedImplementation(IEnumerable<CourseImplementation> courseImplementations)
        {
            return courseImplementations.Any(courseImplementation => courseImplementation.Days.Any(day => Days.Contains(day) && courseImplementation.Status == Status.AVAILABLE));
        }

        public bool IntersectsWithStatus(IEnumerable<Course> courses, Status status)
        {
            return courses
                .SelectMany(course => course.CourseImplementations)
                .Any(courseImplementation => courseImplementation.Days.Any(day => Days.Contains(day) && courseImplementation.Status == status));
        }
    }
}
