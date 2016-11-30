using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.model
{
    public class Profile
    {
        public string Name { get; set; }
        public List<CoursePriority> Courses { get; set; }
    }
    public class CoursePriority
    {
        public string Code { get; set; }
        public string Priority { get; set; }
    }
}
