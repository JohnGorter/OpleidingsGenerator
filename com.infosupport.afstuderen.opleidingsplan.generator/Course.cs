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
        public IEnumerable<string> IntersectedCourseIds { get; private set; }

        public static explicit operator Course(model.Course course)
        {
            return new Course
            {
                Code = course.Code,
                CourseImplementations = course.CourseImplementations,
                Priority = course.Priority,
            };
        }

        public CourseImplementation GetFirstAvailableCourseImplementation(IEnumerable<generator.Course> courses)
        {
            var plannedCourses = courses.Select(course => course.PlannedCourseImplementation);
            return this.CourseImplementations
                .OrderBy(course => course.StartDay)
                .FirstOrDefault(courseImplementation => !courseImplementation.Intersects(plannedCourses));
        }

        public void AddIntersectedCourses(IEnumerable<generator.Course> plannedCourses)
        {
            this.IntersectedCourseIds = plannedCourses.Where(course => course.Intersects(this)).Select(course => course.Code).ToList();
        }

        public IEnumerable<generator.Course> GetIntersectedCoursesWithEqualOrLowerPriority(IEnumerable<Course> plannedCourses)
        {
            return plannedCourses.Where(course => course.Intersects(this) && course.Priority >= this.Priority && course.Code != this.Code).ToList();
        }
        public bool IsPlannable(IEnumerable<Course> plannedCourses)
        {
            List<string> scannedCourses = new List<string>();
            return IsPlannable(plannedCourses, scannedCourses);
        }

        //public void GetFreeCourse(IEnumerable<Course> plannedCourses)
        //{
        //    var intersectedCourses = this.GetIntersectedCoursesWithEqualOrLowerPriority(plannedCourses);

        //    foreach (var intersectedCourse in intersectedCourses)
        //    {
        //        CourseImplementation firstAvailableImplementationForIntersectedCourse = intersectedCourse.GetFirstAvailableCourseImplementation(plannedCourses);
        //        if (firstAvailableImplementationForIntersectedCourse != null)
        //        {
        //            intersectedCourse.PlannedCourseImplementation = firstAvailableImplementationForIntersectedCourse;
        //            CourseImplementation firstAvailableImplementation = this.GetFirstAvailableCourseImplementation(plannedCourses);
        //            this.PlannedCourseImplementation = firstAvailableImplementation;

        //            break;
        //        }
        //        else
        //        {
        //            //GEEN VRIJE IMPLEMENTATIONS
        //            intersectedCourse.GetFreeCourse(plannedCourses);
        //        }
        //    }

        //}

        private bool Intersects(generator.Course course)
        {
            return course.CourseImplementations.Any(courseImplementation => courseImplementation.Intersects(this.CourseImplementations));
        }

        private bool HasAvailableCourseImplementation(IEnumerable<generator.Course> courses)
        {
            var plannedCourses = courses.Select(course => course.PlannedCourseImplementation);
            return this.CourseImplementations.Any(courseImplementation => !courseImplementation.Intersects(plannedCourses));
        }

        private bool IsPlannable(IEnumerable<Course> plannedCourses, List<string> scannedCourses)
        {
            bool plannable = false;

            if (this.HasAvailableCourseImplementation(plannedCourses))
            {
                plannable = true;
            }

            var intersectedCourses = this.GetIntersectedCoursesWithEqualOrLowerPriority(plannedCourses);
            var intersectedCoursesWithoutScanned = intersectedCourses.Where(intersectedCourse => !scannedCourses.Contains(intersectedCourse.Code)).ToList();

            foreach (var intersectedCourse in intersectedCoursesWithoutScanned)
            {
                scannedCourses.Add(intersectedCourse.Code);
                if (!plannable)
                {
                    plannable = intersectedCourse.IsPlannable(plannedCourses, scannedCourses);
                }
            }

            return plannable;
        }
    }

}