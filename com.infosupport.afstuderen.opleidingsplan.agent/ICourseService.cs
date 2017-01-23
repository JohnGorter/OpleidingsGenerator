using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.integration
{
    public interface ICourseService
    {
        Coursesummarycollection FindAllCourses();
        Course FindCourse(string courseCode);
        IEnumerable<Course> FindCourses(IEnumerable<string> courseCodes);
    }
}
