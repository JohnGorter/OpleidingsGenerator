using InfoSupport.KC.OpleidingsplanGenerator.Integration;
using InfoSupport.KC.OpleidingsplanGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSupport.KC.OpleidingsplanGenerator.Api.Managers
{
    public interface ICourseManager
    {
        Coursesummarycollection FindCourses();
        Integration.Course FindCourse(string courseCode);
        void Insert(CoursePriority course);
        void Update(CoursePriority course);
        void Delete(CoursePriority course);
    }
}
