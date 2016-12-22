using com.infosupport.afstuderen.opleidingsplan.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public class Planner : IPlanner
    {
        private CoursePlanning _coursePlanning = new CoursePlanning();
        private DateTime _startDate = DateTime.Now;
        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                if (value != null)
                {
                    _startDate = value;
                }
            }
        }

        public List<DateTime> BlockedDates { get; set; } = new List<DateTime>();


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

        public void PlanCourses(IEnumerable<models.Course> coursesToPlan)
        {
            coursesToPlan = coursesToPlan.OrderBy(course => course.Priority);

            foreach (var courseToPlan in coursesToPlan)
            {
                var course = (generator.Course)courseToPlan;
                var knownCourses = _coursePlanning.GetCourses();

                MarkCourseImplementations(course, knownCourses);
                _coursePlanning.AddCourse(course);
            }

            var availableCourses = _coursePlanning.GetAvailableCourses();
            if (availableCourses.Any())
            {
                MarkAvailableCourseImplementations();
            }

            var plannedCourses = _coursePlanning.GetPlannedCourses();
            var notPlannedCourses = _coursePlanning.GetNotPlannedCourses();
            foreach (var notPlannedCourse in notPlannedCourses)
            {
                notPlannedCourse.AddIntersectedCourses(plannedCourses);
            }
        }

        private generator.Course RemoveBlockedImplementations(generator.Course course)
        {
            if (BlockedDates != null)
            {
                DateTime endDate = StartDate.GetEndDay();

                var blockedImplementations = course.CourseImplementations.Where(ci => ci.StartDay < StartDate || ci.StartDay > endDate || ci.Days.Any(day => BlockedDates.Select(bd => bd.Date).Contains(day)));

                foreach (var implementation in blockedImplementations)
                {
                    implementation.Status = Status.UNPLANNABLE;
                }
            }

            return course;
        }

        private void MarkCourseImplementations(generator.Course course, IEnumerable<generator.Course> knownCourses)
        {
            course = RemoveBlockedImplementations(course);

            if (course.HasOnlyImplementationsWithStatus(Status.UNPLANNABLE))
            {
                return;
            }

            if (course.HasOneImplementation())
            {
                if (course.IsPlannable(knownCourses))
                {
                    course.MarkAllImplementations(Status.NOTPLANNED);
                    course.MarkOnlyAvailableImplementationPlanned(knownCourses);
                    course.MarkAllIntersectedOfPlannedImplementations(Status.UNPLANNABLE, knownCourses);
                }
                else
                {
                    course.MarkAllImplementations(Status.UNPLANNABLE);
                }
            }
            else if (course.HasMultipleImplementationsWithStatus(Status.UNKNOWN))
            {
                course.MarkAllImplementations(Status.AVAILABLE);
            }
            else
            {
                course.MarkAllImplementations(Status.UNPLANNABLE);
            }
        }


        private void MarkAvailableCourseImplementations()
        {
            var availableCourses = _coursePlanning.GetAvailableCourses();
            var plannedCourses = _coursePlanning.GetCourses();

            while (availableCourses.Any())
            {
                var course = availableCourses.First();

                if (course.HasAvailableImplementations(plannedCourses))
                {
                    course.MarkAllImplementations(Status.NOTPLANNED);
                    course.MarkMinimumIntersectedFirstAvailableImplementationPlanned(plannedCourses);
                    course.MarkAllIntersectedOfPlannedImplementations(Status.UNPLANNABLE, plannedCourses);
                }
                else
                {
                    course.MarkAllImplementations(Status.UNPLANNABLE);
                }
            }

        }
    }
}
