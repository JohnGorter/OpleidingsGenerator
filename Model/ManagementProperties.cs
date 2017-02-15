using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSupport.KC.OpleidingsplanGenerator.Models
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

        public override string ToString()
        {
            return string.Format("OlcPrice: {0}, PeriodEducationPlanInDays: {1}, PeriodAfterLastCourseEmployableInDays: {2}, PeriodBeforeStartNotifiable: {3}, Footer: {4}, StaffDiscount: {5}", OlcPrice, PeriodEducationPlanInDays, PeriodAfterLastCourseEmployableInDays, PeriodBeforeStartNotifiable, Footer, StaffDiscount);
        }
    }
}