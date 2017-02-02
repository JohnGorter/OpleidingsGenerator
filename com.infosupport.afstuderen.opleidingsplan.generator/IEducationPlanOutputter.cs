using InfoSupport.KC.OpleidingsplanGenerator.Models;

namespace InfoSupport.KC.OpleidingsplanGenerator.Generator
{
    public interface IEducationPlanOutputter
    {
        EducationPlan GenerateEducationPlan(EducationPlanData educationPlanData);

    }
}