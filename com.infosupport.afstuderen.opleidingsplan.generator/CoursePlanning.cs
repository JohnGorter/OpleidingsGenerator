using com.infosupport.afstuderen.opleidingsplan.model;
using System.Collections.Generic;
using System.Linq;
using System;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public class CoursePlanning
    {
        private List<Course> _coursesPlanned = new List<Course>();
        private List<Course> _coursesNotPlanned = new List<Course>();

        public void AddToPlanned(Course course)
        {
            _coursesPlanned.Add(course);
        }

        public void AddToNotPlanned(Course course)
        {
            _coursesNotPlanned.Add(course);
        }

        public IEnumerable<Course> GetPlannedCourses()
        {
            return _coursesPlanned;
        }

        public IEnumerable<Course> GetNotPlannedCourses()
        {
            return _coursesNotPlanned;
        }
    }
}