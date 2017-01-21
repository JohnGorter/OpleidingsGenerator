using com.infosupport.afstuderen.opleidingsplan.models;
using System;
using System.Collections.Generic;

namespace com.infosupport.afstuderen.opleidingsplan.dal.mappers
{
    public interface IEducationPlanDataMapper
    {
        void Delete(long id);
        IEnumerable<EducationPlan> Find(Func<EducationPlan, bool> predicate);
        long Insert(EducationPlan educationPlan);
        long Update(EducationPlan educationPlan);
        EducationPlan FindById(long id);
        IEnumerable<EducationPlanCompare> FindAllUpdated();
    }
}