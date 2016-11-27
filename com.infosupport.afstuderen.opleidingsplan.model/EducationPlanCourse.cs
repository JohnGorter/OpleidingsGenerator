﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.model
{
    public class EducationPlanCourse
    {
        public string Code { get; set; }
        public DateTime? Date { get; set; }
        public string Name { get; set; }
        public int Days { get; set; }
        public string Commentary { get; set; }
        public decimal Price { get; set; }
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
                    Date = Date.Value.AddDays(3);
                }

                // Return the week of our adjusted day
                return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(Date.Value, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            }
        }
        public decimal PriceWithDiscount
        {
            get
            {
                return Price * 0.8M;
            }
        }
    }
}
