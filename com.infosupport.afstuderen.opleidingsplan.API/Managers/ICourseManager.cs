using com.infosupport.afstuderen.opleidingsplan.integration;
using com.infosupport.afstuderen.opleidingsplan.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.api.managers
{
    public interface ICourseManager
    {
        Coursesummarycollection FindCourses();
        integration.Course FindCourse(string courseCode);
        void Insert(CoursePriority course);
        void Update(CoursePriority course);
        void Delete(CoursePriority course);
    }
}
