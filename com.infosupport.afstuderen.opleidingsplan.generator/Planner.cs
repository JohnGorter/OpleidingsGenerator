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
                var course = (generator.Course)courseToPlan;
                var plannedCoursesTillNow = _coursePlanning.GetPlannedCourses();

                CourseImplementation firstAvailableImplementation = course.GetFirstAvailableCourseImplementation(plannedCoursesTillNow);

                if (firstAvailableImplementation != null)
                {
                    course.PlannedCourseImplementation = firstAvailableImplementation; 
                    _coursePlanning.AddToPlanned(course);
                }
                else
                {
                    var intersectedCourses = course.GetIntersectedCourses(plannedCoursesTillNow);

                    bool addToPlanned = false;
                    foreach (var intersectedCourse in intersectedCourses)
                    {
                        CourseImplementation firstAvailableImplementation1 = intersectedCourse.GetFirstAvailableCourseImplementation(plannedCoursesTillNow);
                        if (firstAvailableImplementation1 != null)
                        {
                            intersectedCourse.PlannedCourseImplementation = firstAvailableImplementation1;
                            firstAvailableImplementation = course.GetFirstAvailableCourseImplementation(plannedCoursesTillNow);
                            course.PlannedCourseImplementation = firstAvailableImplementation;              
                            _coursePlanning.AddToPlanned(course);
                            addToPlanned = true;
                            break;
                        }
                    }

                    if(!addToPlanned)
                    {
                        _coursePlanning.AddToNotPlanned(course);
                    }
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
