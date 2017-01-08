using com.infosupport.afstuderen.opleidingsplan.models;

namespace com.infosupport.afstuderen.opleidingsplan.api.tests.helpers
{
    public class ManagementPropertiesTestHelper
    {
        internal ManagementProperties GetDummyDataManagementProperties()
        {
            return new ManagementProperties
            {
                OLCPrice = 150,
                PeriodAfterLastCourseEmployableInDays = 2,
                PeriodBeforeStartNotifiable = 4,
                PeriodEducationPlanInDays = 100,
            };
        }
    }
}