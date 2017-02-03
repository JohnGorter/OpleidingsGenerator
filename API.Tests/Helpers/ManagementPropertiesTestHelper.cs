using InfoSupport.KC.OpleidingsplanGenerator.Models;

namespace InfoSupport.KC.OpleidingsplanGenerator.Api.Tests.Helpers
{
    public class ManagementPropertiesTestHelper
    {
        internal ManagementProperties GetDummyDataManagementProperties()
        {
            return new ManagementProperties
            {
                OlcPrice = 150,
                PeriodAfterLastCourseEmployableInDays = 2,
                PeriodBeforeStartNotifiable = 4,
                PeriodEducationPlanInDays = 100,
                Footer = "new footer",
                StaffDiscount = 80,
            };
        }
    }
}