using InfoSupport.KC.OpleidingsplanGenerator.Api.Models;
using InfoSupport.KC.OpleidingsplanGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSupport.KC.OpleidingsplanGenerator.Api.Managers
{
    public interface IEducationPlanManager
    {
        EducationPlan GenerateEducationPlan(RestEducationPlan educationPlan, EducationPlan oldEducationplan);
        EducationPlan PreviewEducationPlan(RestEducationPlan educationPlan);

        long SaveEducationPlan(RestEducationPlan restEducationPlan);
        long UpdateEducationPlan(RestEducationPlan restEducationPlan);
        EducationPlan FindEducationPlan(long id);
        void DeleteEducationPlan(long id);
        List<EducationPlan> FindEducationPlans(EducationPlanSearch search);
        string GenerateWordFile(EducationPlan educationPlan);
        List<EducationPlanCompareSummary> FindAllUpdated();
        EducationPlanCompare FindUpdatedById(long id);
        void RejectUpdatedEducationPlan(long id);
        void ApproveUpdatedEducationPlan(long id);
    }
}
