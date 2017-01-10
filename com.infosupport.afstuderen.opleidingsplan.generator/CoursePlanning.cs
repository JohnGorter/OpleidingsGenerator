using com.infosupport.afstuderen.opleidingsplan.models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public class CoursePlanning
    {
        public List<Course> Courses { get; set; }
        public IEnumerable<Course> PlannedCourses
        {
            get
            {
                return Courses.Where(course => course.CourseImplementations
                .Any(courseImplementation => courseImplementation.Status == Status.PLANNED));
            }
        }

        public IEnumerable<Course> NotPlannedCourses
        {
            get
            {
                return Courses.Where(course => course.CourseImplementations
                .None(courseImplementation => courseImplementation.Status == Status.PLANNED));
            }
        }

        public IEnumerable<Course> AvailableCourses
        {
            get
            {
                return Courses.Where(course => course.CourseImplementations
                .Any(courseImplementation => courseImplementation.Status == Status.AVAILABLE));
            }
        }

        public CoursePlanning()
        {
            Courses = new List<Course>();
        }
    }
}