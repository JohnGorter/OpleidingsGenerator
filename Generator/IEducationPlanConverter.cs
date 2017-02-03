using InfoSupport.KC.OpleidingsplanGenerator.Models;

namespace InfoSupport.KC.OpleidingsplanGenerator.Generator
{
    public interface IEducationPlanConverter
    {
        string GenerateWord(EducationPlan educationPlan);
    }
}