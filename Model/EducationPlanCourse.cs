using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSupport.KC.OpleidingsplanGenerator.Models
{
    public class EducationPlanCourse
    {
        public string Code { get; set; }
        public DateTime? Date { get; set; }
        public string Name { get; set; }
        public int Days { get; set; }
        public string Commentary { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<EducationPlanCourse> IntersectedCourses { get; set; }

        public int Week
        {
            get
            {
                if(Date == null)
                {
                    return -1;
                }

                DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(Date.Value);
                if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
                {
                    return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(Date.Value.AddDays(3), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                }

                // Return the week of our adjusted day
                return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(Date.Value, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            }
        }

        public decimal PriceWithDiscount
        {
            get
            {
                return (Price/100) * StaffDiscountInPercentage;
            }
        }

        public decimal StaffDiscountInPercentage { get; set; }
    }
}
