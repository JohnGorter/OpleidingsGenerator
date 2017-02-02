using InfoSupport.KC.OpleidingsplanGenerator.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.ObjectModel;

namespace InfoSupport.KC.OpleidingsplanGenerator.Generator
{
    public class CoursePlanning
    {
        public Collection<Course> Courses { get; }
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
            Courses = new Collection<Course>();
        }
    }
}