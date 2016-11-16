using System.Collections.Generic;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public class CoursePriority
    {
        public string CourseId { get; set; }
        public int Priority { get; set; }
        public IEnumerable<model.CourseImplementation> CourseImplementations { get; set; }
    }

}