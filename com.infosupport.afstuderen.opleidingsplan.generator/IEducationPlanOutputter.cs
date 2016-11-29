using com.infosupport.afstuderen.opleidingsplan.model;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public interface IEducationPlanOutputter
    {
        EducationPlan GenerateEducationPlan(EducationPlanData educationPlanData);

    }
}