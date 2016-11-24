using com.infosupport.afstuderen.opleidingsplan.model;
using System.Collections.Generic;
using System.Linq;
using System;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public class CoursePlanning
    {
        private List<Course> _courses = new List<Course>();

        public void AddCourse(Course course)
        {
            _courses.Add(course);
        }

        public IEnumerable<Course> GetCourses()
        {
            return _courses;
        }

        public IEnumerable<Course> GetPlannedCourses()
        {
            return _courses
                .Where(course => course.CourseImplementations
                .Any(courseImplementation => courseImplementation.Status == Status.PLANNED));
        }

        public IEnumerable<Course> GetNotPlannedCourses()
        {
            return _courses
                .Where(course => course.CourseImplementations
                .None(courseImplementation => courseImplementation.Status == Status.PLANNED));
        }

        public IEnumerable<Course> GetAvailableCourses()
        {
            return _courses
                .Where(course => course.CourseImplementations
                .Any(courseImplementation => courseImplementation.Status == Status.AVAILABLE));
        }
    }
}