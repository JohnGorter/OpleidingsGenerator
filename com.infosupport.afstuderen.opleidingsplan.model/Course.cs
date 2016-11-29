using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.model
{
    public class Course
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string SupplierName { get; set; }
        public string Duration { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string Prerequisites { get; set; }
        public IEnumerable<CourseImplementation> CourseImplementations { get; set; }
        public decimal Price { get; set; }
        public int Priority { get; set; }
    }

    public class CourseImplementation
    {
        public string Location { get; set; }
        public DateTime StartDay { get { return Days.OrderBy(day => day).FirstOrDefault(); } set { } }
        public IEnumerable<DateTime> Days { get; set; }
    }
}
