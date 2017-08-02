using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSupport.KC.OpleidingsplanGenerator.Models
{
    public class Course
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string SupplierName { get; set; }
        public string Duration { get; set; }

        public string Description { get; set; }
        public int Days { get {
                return Convert.ToInt32(this.Duration.Substring(0, 1));
            }
        }
        public string ShortDescription { get; set; }
        public IEnumerable<CourseImplementation> CourseImplementations { get; set; }
        public decimal Price { get; set; }
        public int Priority { get; set; }
        public string Commentary { get; set; }

        public override string ToString()
        {
            return string.Format("Code: {0}, Name: {1}", Code, Name);
        }
    }

    public class CourseImplementation
    {
        public string Location { get; set; }
        public DateTime StartDay { get { return Days.OrderBy(day => day).FirstOrDefault(); } }
        public IEnumerable<DateTime> Days { get; set; }

        public override string ToString()
        {
            return string.Format("Location: {0}, StartDay: {1}", Location, StartDay);
        }
    }

    public class PinnedCourseImplementation
    {
        public String CourseCode;
        public Int32 CourseImplementationWeek;
    }

}
