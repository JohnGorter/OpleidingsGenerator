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
                    test(course, plannedCoursesTillNow);
                    //var intersectedCourses = course.GetIntersectedCoursesWithEqualOrLowerPriority(plannedCoursesTillNow);

                    //bool addToPlanned = false;
                    //foreach (var intersectedCourse in intersectedCourses)
                    //{
                    //    CourseImplementation firstAvailableImplementation1 = intersectedCourse.GetFirstAvailableCourseImplementation(plannedCoursesTillNow);
                    //    if (firstAvailableImplementation1 != null)
                    //    {
                    //        intersectedCourse.PlannedCourseImplementation = firstAvailableImplementation1;
                    //        firstAvailableImplementation = course.GetFirstAvailableCourseImplementation(plannedCoursesTillNow);
                    //        course.PlannedCourseImplementation = firstAvailableImplementation;              
                    //        _coursePlanning.AddToPlanned(course);
                    //        addToPlanned = true;
                    //        break;
                    //    }
                    //}

                    //if(!addToPlanned)
                    //{
                    //    _coursePlanning.AddToNotPlanned(course);
                    //}
                }

            }

            var plannedCourses = _coursePlanning.GetPlannedCourses();

            foreach (var notPlannedCourse in _coursePlanning.GetNotPlannedCourses())
            {
                notPlannedCourse.AddIntersectedCourses(plannedCourses);
            }
        }

        private List<string> _scannedCoursesIds = new List<string>();
        //private List<Course> _oldPlannedCourses = new List<Course>();
        //Parameter plannedCourseTill Now kan wel en uit de planningCourse halen
        private void test(Course course, IEnumerable<Course> plannedCoursesTillNow)
        {
            var intersectedCourses = course.GetIntersectedCoursesWithEqualOrLowerPriority(plannedCoursesTillNow);
            var intersectedCoursesWithoutScanned = intersectedCourses.Where(intersectedCourse => !_scannedCoursesIds.Contains(intersectedCourse.Code)).ToList();

            bool addToPlanned = false;

            foreach (var intersectedCourse in intersectedCoursesWithoutScanned)
            {
                CourseImplementation firstAvailableImplementationIntersectedCourse = intersectedCourse.GetFirstAvailableCourseImplementation(plannedCoursesTillNow);

                if (firstAvailableImplementationIntersectedCourse != null)
                {
                    intersectedCourse.PlannedCourseImplementation = firstAvailableImplementationIntersectedCourse;
                    CourseImplementation firstAvailableImplementation = course.GetFirstAvailableCourseImplementation(plannedCoursesTillNow);
                    course.PlannedCourseImplementation = firstAvailableImplementation;
                    _coursePlanning.AddToPlanned(course);
                    addToPlanned = true;
                    break;
                }
                else
                {
                    _scannedCoursesIds.Add(intersectedCourse.Code);
                }
            }

            //bool t = course.IsPlannable(plannedCoursesTillNow);
            //KLOPT NIET
            //wordt altijd aan niet geplanned toegevoegd. Er moet eerst door de intersectedcourses gelooped worden en dan kijken.
            //if (!addToPlanned)
            //{
            //    _coursePlanning.AddToNotPlanned(course);
            //}
            //else
            //{
            //    foreach (var intersectedCourse in intersectedCourses)
            //    {
            //        test(intersectedCourse, plannedCoursesTillNow);
            //    }
            //}

            if (!addToPlanned)
            {
                _coursePlanning.AddToNotPlanned(course);
                foreach (var intersectedCourse in intersectedCourses)
                {
                   // test(intersectedCourse, plannedCoursesTillNow);
                }
            }
        }
    }
}
