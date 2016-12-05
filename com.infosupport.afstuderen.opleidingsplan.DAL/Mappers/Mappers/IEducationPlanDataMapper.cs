﻿using com.infosupport.afstuderen.opleidingsplan.model;
using System;
using System.Collections.Generic;

namespace com.infosupport.afstuderen.opleidingsplan.DAL.Mappers.Mappers
{
    public interface IEducationPlanDataMapper
    {
        void Delete(EducationPlan educationPlan);
        IEnumerable<EducationPlan> Find(Func<EducationPlan, bool> predicate);
        void Insert(EducationPlan educationPlan);
        void Update(EducationPlan educationPlan);
        EducationPlan FindById(long id);
        IEnumerable<EducationPlan> FindAllUpdated();
    }
}