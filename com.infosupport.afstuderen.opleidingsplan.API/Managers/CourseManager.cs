using com.infosupport.afstuderen.opleidingsplan.agent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.infosupport.afstuderen.opleidingsplan.api.Managers
{
    public class CourseManager
    {
        private AgentCourse _agentCourse;

        public CourseManager()
        {
            _agentCourse = new AgentCourse();
        }

        public Coursesummarycollection FindCourses()
        {
            return _agentCourse.FindAllCourses();
        }
    }
}