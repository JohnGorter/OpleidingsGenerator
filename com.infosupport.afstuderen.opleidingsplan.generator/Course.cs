using System;
using System.Collections.Generic;
using System.Linq;
using com.infosupport.afstuderen.opleidingsplan.model;

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

        public CourseImplementation GetFirstAvailableCourseImplementation(IEnumerable<generator.Course> courses)
        {
            var plannedCourses = courses.Select(course => course.PlannedCourseImplementation);
            return this.CourseImplementations.FirstOrDefault(courseImplementation => !courseImplementation.Intersects(plannedCourses));
        }
        public void AddIntersectedCourses(IEnumerable<generator.Course> plannedCourses)
        {
            this.IntersectedCourseIds = GetIntersectedCourses(plannedCourses).Select(course => course.Code).ToList();
        }

        public IEnumerable<generator.Course> GetIntersectedCourses(IEnumerable<Course> plannedCourses)
        {
            return plannedCourses.Where(course => course.Intersects(this)).ToList();
        }

        private bool Intersects(generator.Course course)
        {
            return course.CourseImplementations.Any(courseImplementation => courseImplementation.Intersects(this.CourseImplementations));
        }


    }

}