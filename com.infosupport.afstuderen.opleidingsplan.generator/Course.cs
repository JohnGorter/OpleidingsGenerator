using System;
using System.Collections.Generic;
using System.Linq;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public class Course
    {
        public string Code { get; set; }
        public int Priority { get; set; }
        public model.CourseImplementation PlannedCourseImplementation { get; set; }
        public IEnumerable<model.CourseImplementation> CourseImplementations { get; set; }
        public IEnumerable<string> IntersectedCourseIds { get; set; }

        public static explicit operator Course(model.Course course)
        {
            return new Course
            {
                Code = course.Code,
                CourseImplementations = course.CourseImplementations,
                Priority = course.Priority,
            };
        }

        public bool Intersects(IEnumerable<generator.Course> courses)
        {
            return courses.Any(course => course.Intersects(this));
        }


        private bool Intersects(generator.Course course)
        {
            return course.CourseImplementations.Any(courseImplementation => courseImplementation.Intersects(this.CourseImplementations));
        }

        public void AddIntersectedCourses(IEnumerable<generator.Course> plannedCourses)
        {
            this.IntersectedCourseIds = plannedCourses.Where(course => course.Intersects(this)).Select(course => course.Code).ToList();
        }
    }

}