using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.models
{
    public class ManagementProperties
    {
        [Required]
        public decimal OlcPrice { get; set; }
        [Required]
        public int PeriodEducationPlanInDays { get; set; }
        [Required]
        public int PeriodAfterLastCourseEmployableInDays { get; set; }
        [Required]
        public int PeriodBeforeStartNotifiable { get; set; }
        [Required]
        public string Footer { get; set; }
        [Required]
        public decimal StaffDiscount { get; set; }
    }
}