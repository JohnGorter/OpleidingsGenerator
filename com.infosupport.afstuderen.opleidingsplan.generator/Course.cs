﻿using System;
using System.Collections.Generic;
using System.Linq;
using com.infosupport.afstuderen.opleidingsplan.model;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public class Course
    {
        public string Code { get; set; }
        public int Priority { get; set; }
        public generator.CourseImplementation PlannedCourseImplementation { get; set; }
        public IEnumerable<generator.CourseImplementation> CourseImplementations { get; set; }
        public IEnumerable<string> IntersectedCourseIds { get; private set; }

        public static explicit operator Course(model.Course course)
        {
            List<generator.CourseImplementation> courseImplementations = new List<CourseImplementation>();

            if (course.CourseImplementations != null)
            {
                foreach (var implementations in course.CourseImplementations)
                {
                    courseImplementations.Add((generator.CourseImplementation)implementations);
                }
            }

            return new Course
            {
                Code = course.Code,
                CourseImplementations = courseImplementations,
                Priority = course.Priority,
            };
        }

        public bool HasMultipleAvailableImplementations(IEnumerable<generator.Course> courses)
        {
            return GetCourseAvailableImplementation(courses).Count() > 1;
        }

        public void MarkAllImplementations(Status status)
        {
            foreach (var implementation in this.CourseImplementations)
            {
                implementation.Status = status;
            }
        }

        public bool HasOneAvailableImplementation(IEnumerable<generator.Course> courses)
        {
            return GetCourseAvailableImplementation(courses).Count() == 1;
        }

        public bool HasAvailableImplementations(IEnumerable<Course> plannedCourses)
        {
            return GetCourseAvailableImplementation(plannedCourses).Any();
        }

        public void MarkOnlyAvailableImplementationPlanned(IEnumerable<generator.Course> courses)
        {
            if(!HasOneAvailableImplementation(courses))
            {
                throw new AmountImplementationException("There is not exactly one implementation available.");
            }

            var availableImplementation = GetCourseAvailableImplementation(courses).First();
            availableImplementation.Status = Status.PLANNED;
        }

        public void MarkFirstAvailableImplementationPlanned(IEnumerable<generator.Course> courses)
        {
            if(GetCourseAvailableImplementation(courses).None())
            {
                throw new AmountImplementationException("No available implementations");
            }

            var availableImplementation = GetCourseAvailableImplementation(courses).OrderBy(course => course.StartDay).First();
            availableImplementation.Status = Status.PLANNED;
        }

        private IEnumerable<generator.CourseImplementation> GetCourseAvailableImplementation(IEnumerable<generator.Course> courses)
        {
            return this.CourseImplementations.Where(courseImplementation => !courseImplementation.IntersectsWithStatus(courses, Status.PLANNED));
        }


        public generator.CourseImplementation GetFirstAvailableCourseImplementation(IEnumerable<generator.Course> courses)
        {
            var plannedCourses = courses.Select(course => course.PlannedCourseImplementation);
            return this.CourseImplementations
                .OrderBy(course => course.StartDay)
                .FirstOrDefault(courseImplementation => !courseImplementation.Intersects(plannedCourses));
        }

        public IEnumerable<Course> GetAvailableIntersectedCoursesWithPlannedImplementation(IEnumerable<Course> courses)
        {
            return courses
                .Where(course => course.IntersectsPlannedImplementation(this) && course.Code != this.Code).ToList();
        }

        private bool IntersectsPlannedImplementation(Course course)
        {
            return course.CourseImplementations
                .Any(courseImplementation => courseImplementation.IntersectsPlannedImplementation(this.CourseImplementations) && courseImplementation.Status == Status.PLANNED);
        }

        public CourseImplementation GetPlannedImplementation()
        {
            CourseImplementation plannedCourseImplementation = this.CourseImplementations.FirstOrDefault(course => course.Status == Status.PLANNED);
            if (plannedCourseImplementation == null)
            {
                throw new AmountImplementationException("There is no planned implementation.");
            }

            return plannedCourseImplementation;
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

        public bool Intersects(generator.Course course)
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