using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.models
{
    public class ManagementProperties
    {
        public decimal OLCPrice { get; set; }
        public int PeriodEducationPlanInDays { get; set; }
        public int PeriodAfterLastCourseEmployableInDays { get; set; }
        public int PeriodBeforeStartNotifiable { get; set; }
    }
}