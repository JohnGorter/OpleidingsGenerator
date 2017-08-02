using InfoSupport.KC.OpleidingsplanGenerator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace InfoSupport.KC.OpleidingsplanGenerator.Generator
{
    public interface IPlanner
    {
        DateTime StartDate { get; set; }
        DateTime? EndDate { get; set; }
        Collection<DateTime> BlockedDates { get; set; }

        IEnumerable<Course> PlannedCourses { get; }
        IEnumerable<Course> NotPlannedCourses { get; }
        IEnumerable<Course> AllCourses { get; }
        void PlanCourses(IEnumerable<Models.Course> coursesToPlan, IEnumerable<PinnedCourseImplementation> implementationConstraints);
        void PlanCoursesWithOlc(IEnumerable<Models.Course> coursesToPlan, IEnumerable<PinnedCourseImplementation> implementationConstraints);
        void PlanCoursesWithOlcInOldEducationPlan(IEnumerable<Models.Course> coursesToPlan, EducationPlan oldEducationplan, IEnumerable<PinnedCourseImplementation> implementationConstraints);
        void AddModules(IEnumerable<Module> enumerable);
    }
}