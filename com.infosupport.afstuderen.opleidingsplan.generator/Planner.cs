using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public class Planner
    {
        public CoursePlanningCollection CoursesToFollow { get; } = new CoursePlanningCollection();
        public CoursePlanningCollection CoursesNotPlanned { get; } = new CoursePlanningCollection();

        public Planner()
        {
        }

        public void PlanCourses(IEnumerable<CoursePriority> coursesToPlan)
        {
            coursesToPlan = coursesToPlan.OrderBy(course => course.Priority);

            foreach (var courseToPlan in coursesToPlan)
            {
                if (CoursesToFollow.Overlap(courseToPlan))
                {
                    CoursesNotPlanned.Add(new CoursePlanning(courseToPlan.CourseId, courseToPlan.CourseImplementations.First(), courseToPlan.Priority));
                }
                else
                {
                    CoursesToFollow.Add(new CoursePlanning(courseToPlan.CourseId, courseToPlan.CourseImplementations.First(), courseToPlan.Priority));
                }
            }

            AddNotPlannedCourseIds(coursesToPlan);
        }

        private void AddNotPlannedCourseIds(IEnumerable<CoursePriority> coursesToPlan)
        {
            foreach (var courseNotPlanned in CoursesNotPlanned)
            {
                var coursePriority = coursesToPlan.First(course => course.CourseId == courseNotPlanned.CourseId);
                var overlapCourseIdsFollow = CoursesToFollow.GetOverlapCourses(coursePriority);

                courseNotPlanned.CourseIdsOverlap = overlapCourseIdsFollow.ToList();
            }
        }
    }
}
