using InfoSupport.KC.OpleidingsplanGenerator.Models;
using System;
using System.Collections.Generic;

namespace InfoSupport.KC.OpleidingsplanGenerator.Dal.Mappers
{
    public interface IEducationPlanDataMapper
    {
        void Delete(long id);
        IEnumerable<EducationPlan> Find(Func<EducationPlan, bool> predicate);
        long Insert(EducationPlan educationPlan);
        long Update(EducationPlan educationPlan);
        EducationPlan FindById(long id);
        EducationPlanCompare FindUpdatedById(long id);
        IEnumerable<EducationPlanCompareSummary> FindAllUpdated();
        void RejectUpdatedEducationPlan(long id);
        void ApproveUpdatedEducationPlan(long id);
    }
}