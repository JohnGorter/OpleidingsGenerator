using com.infosupport.afstuderen.opleidingsplan.model;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public class CoursePlanningCollection : IEnumerable<CoursePlanning>
    {
        private List<CoursePlanning> _courses = new List<CoursePlanning>();
        public int Count
        {
            get
            {
                return _courses.Count;
            }
        }

        public CoursePlanning this[int index]
        {
            get
            {
                return _courses[index];
            }
            set
            {
                _courses[index] = value;
            }
        }

        public void Add(CoursePlanning coursePlanning)
        {
            _courses.Add(coursePlanning);
        }
        //public void ForceAdd(CoursePlanning coursePlanning)
        //{
        //    var overlapedCourses = GetOverlapCourses(coursePlanning.CourseImplementation).ToList();
        //    RemoveCourses(overlapedCourses);
        //    _courses.Add(coursePlanning);
        //}

        //private void RemoveCourses(List<CoursePlanning> overlapedCourses)
        //{
        //    for (int i = overlapedCourses.Count() - 1; i >= 0; i--)
        //    {
        //        _courses.Remove(overlapedCourses[i]);
        //    }

        //}

        public bool Overlap(CoursePriority courseToPlan)
        {
            return _courses.Any(courseToFollow => courseToFollow.Overlap(courseToPlan.CourseImplementations));
        }

        public IEnumerable<string> GetOverlapCourses(CoursePriority courseToPlan)
        {
            return _courses.Where(courseToFollow => courseToFollow.Overlap(courseToPlan.CourseImplementations)).Select(c => c.CourseId);
        }

        public IEnumerator<CoursePlanning> GetEnumerator()
        {
            foreach (var course in _courses)
            {
                yield return course;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        //public IEnumerable<CoursePlanning> GetOverlapCourses(CourseImplementation courseImplementation)
        //{
        //    return _courses.Where(courseToFollow => courseToFollow.Overlap(courseImplementation));
        //}

    }

}