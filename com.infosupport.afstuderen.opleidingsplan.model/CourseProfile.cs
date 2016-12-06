using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.models
{
    public class CourseProfile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CoursePriority> Courses { get; set; } = new List<CoursePriority>();
    }
    public class CoursePriority
    {
        public string Code { get; set; }
        public int Priority { get; set; }
    }
}
