using com.infosupport.afstuderen.opleidingsplan.integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.api.Managers
{
    public interface ICourseManager
    {
        Coursesummarycollection FindCourses();
        Course FindCourse(string courseCode);
    }
}
