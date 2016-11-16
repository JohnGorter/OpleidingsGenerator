using com.infosupport.afstuderen.opleidingsplan.model;
using System.Collections.Generic;
using System.Linq;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public class CoursePlanning
    {
        public string CourseId { get; set; }
        public int Priority { get; set; }
        public model.CourseImplementation CourseImplementation { get; set; }
        public List<string> CourseIdsOverlap { get; set; }

        public CoursePlanning(string courseId, model.CourseImplementation courseImplementation, int priority)
        {
            CourseId = courseId;
            CourseImplementation = courseImplementation;
            Priority = priority;
        }

        public bool Overlap(IEnumerable<CourseImplementation> courseImplementations)
        {
            return courseImplementations.Any(implementation => implementation.Days.Any(day => CourseImplementation.Days.Contains(day)));
        }
        //public bool Overlap(CourseImplementation courseImplementation)
        //{
        //    return courseImplementation.Days.Any(day => CourseImplementation.Days.Contains(day));
        //}

    }
}