using System;
using System.Collections.Generic;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public interface IPlanner
    {
        DateTime StartDate { get; set; }
        List<DateTime> BlockedDates { get; set; }

        IEnumerable<Course> GetPlannedCourses();
        IEnumerable<Course> GetNotPlannedCourses();
        IEnumerable<Course> GetAllCourses();
        void PlanCourses(IEnumerable<models.Course> coursesToPlan);
        void PlanCoursesWithOLC(IEnumerable<models.Course> coursesToPlan);
    }
}