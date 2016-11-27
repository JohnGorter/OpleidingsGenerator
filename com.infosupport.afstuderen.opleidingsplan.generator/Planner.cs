using com.infosupport.afstuderen.opleidingsplan.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public class Planner
    {
        private CoursePlanning _coursePlanning = new CoursePlanning();

        public Planner()
        {
        }

        public IEnumerable<Course> GetPlannedCourses()
        {
            return _coursePlanning.GetPlannedCourses();
        }
        public IEnumerable<Course> GetNotPlannedCourses()
        {
            return _coursePlanning.GetNotPlannedCourses();
        }

        public IEnumerable<Course> GetAllCourses()
        {
            return _coursePlanning.GetCourses();
        }

        public void PlanCourses(IEnumerable<model.Course> coursesToPlan)
        {
            coursesToPlan = coursesToPlan.OrderBy(course => course.Priority);

            foreach (var courseToPlan in coursesToPlan)
            {
                var course = (generator.Course)courseToPlan;
                var knownCourses = _coursePlanning.GetCourses();

                MutateCourseImplementations(course, knownCourses);
                _coursePlanning.AddCourse(course);
            }

            var availableCourses = _coursePlanning.GetAvailableCourses();
            if (availableCourses.Any())
            {
                MutateCourseImplementations();
            }

            var plannedCourses = _coursePlanning.GetPlannedCourses();
            var notPlannedCourses = _coursePlanning.GetNotPlannedCourses();
            foreach (var notPlannedCourse in notPlannedCourses)
            {
                notPlannedCourse.AddIntersectedCourses(plannedCourses);
            }
        }


        private void MutateCourseImplementations(generator.Course course, IEnumerable<generator.Course> knownCourses)
        {
            if (course.HasMultipleAvailableImplementations(knownCourses))
            {
                course.MarkAllImplementations(Status.AVAILABLE);
            }
            else if (course.HasOneAvailableImplementation(knownCourses))
            {
                course.MarkAllImplementations(Status.NOTUSED);
                course.MarkOnlyAvailableImplementationPlanned(knownCourses);
            }
            else
            {
                course.MarkAllImplementations(Status.NOTPLANNED);
            }
        }


        private void MutateCourseImplementations()
        {
            var availableCourses = _coursePlanning.GetAvailableCourses(); //TODO: First courses with one available implementation
            var plannedCourses = _coursePlanning.GetCourses();


            while (availableCourses.Any())
            {
                var course = availableCourses.First();

                if(course.HasAvailableImplementations(plannedCourses))
                {
                    course.MarkAllImplementations(Status.NOTUSED);
                    course.MarkFirstAvailableImplementationPlanned(plannedCourses);
                    MarkOnlyAvailablePlanned(course, plannedCourses);
                }
                else
                {
                    course.MarkAllImplementations(Status.NOTPLANNED);
                }
            }

        }

        private void MarkOnlyAvailablePlanned(generator.Course course, IEnumerable<generator.Course> knownCourses)
        {
            IEnumerable<Course> intersectedCourses = course.GetAvailableIntersectedCoursesWithPlannedImplementation(knownCourses);

            foreach (var intersectedCourse in intersectedCourses)
            {
                if (intersectedCourse.HasAvailableImplementations(knownCourses))
                {
                    intersectedCourse.MarkAllImplementations(Status.NOTUSED);
                    intersectedCourse.MarkFirstAvailableImplementationPlanned(knownCourses);
                    MarkOnlyAvailablePlanned(intersectedCourse, knownCourses);
                }
                else
                {
                    intersectedCourse.MarkAllImplementations(Status.NOTPLANNED);
                }
            }


        }
    }
}
