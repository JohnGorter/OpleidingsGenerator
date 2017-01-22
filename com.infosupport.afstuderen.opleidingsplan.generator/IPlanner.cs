using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public interface IPlanner
    {
        DateTime StartDate { get; set; }
        Collection<DateTime> BlockedDates { get; set; }

        IEnumerable<Course> PlannedCourses { get; }
        IEnumerable<Course> NotPlannedCourses { get; }
        IEnumerable<Course> AllCourses { get; }
        void PlanCourses(IEnumerable<models.Course> coursesToPlan);
        void PlanCoursesWithOlc(IEnumerable<models.Course> coursesToPlan);
    }
}