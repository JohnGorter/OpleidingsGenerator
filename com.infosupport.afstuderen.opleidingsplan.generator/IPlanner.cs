﻿using System;
using System.Collections.Generic;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public interface IPlanner
    {
        IEnumerable<Course> GetPlannedCourses();
        IEnumerable<Course> GetNotPlannedCourses();
        IEnumerable<Course> GetAllCourses();
        void PlanCourses(IEnumerable<models.Course> coursesToPlan);
    }
}