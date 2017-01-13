using com.infosupport.afstuderen.opleidingsplan.models;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public interface IEducationPlanConverter
    {
        string GenerateWord(EducationPlan educationPlan);
    }
}