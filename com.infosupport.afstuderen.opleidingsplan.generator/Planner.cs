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

        public void PlanCourses(IEnumerable<model.Course> coursesToPlan)
        {

            coursesToPlan = coursesToPlan.OrderBy(course => course.Priority);

            foreach (var courseToPlan in coursesToPlan)
            {
                var course = (generator.Course) courseToPlan;

                if(course.Intersects(_coursePlanning.GetPlannedCourses()))
                {
                    _coursePlanning.AddToNotPlanned(course);
                }
                else
                {
                    course.PlannedCourseImplementation = course.CourseImplementations.First();
                    _coursePlanning.AddToPlanned(course);
                }
            }

            var plannedCourses = _coursePlanning.GetPlannedCourses();

            foreach (var notPlannedCourse in _coursePlanning.GetNotPlannedCourses())
            {
                notPlannedCourse.AddIntersectedCourses(plannedCourses);
            }
        }
    }
}
