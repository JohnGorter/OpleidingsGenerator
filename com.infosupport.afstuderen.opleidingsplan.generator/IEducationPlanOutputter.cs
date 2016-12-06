using com.infosupport.afstuderen.opleidingsplan.models;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public interface IEducationPlanOutputter
    {
        EducationPlan GenerateEducationPlan(EducationPlanData educationPlanData);

    }
}