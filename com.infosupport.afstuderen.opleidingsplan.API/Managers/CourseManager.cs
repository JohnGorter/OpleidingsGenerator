using com.infosupport.afstuderen.opleidingsplan.integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.infosupport.afstuderen.opleidingsplan.api.managers
{
    public class CourseManager : ICourseManager
    {
        private readonly ICourseService _courseService;

        public CourseManager()
        {
            _courseService = new CourseService();
        }

        public CourseManager(ICourseService courseManager)
        {
            _courseService = courseManager;
        }

        public Coursesummarycollection FindCourses()
        {
            return _courseService.FindAllCourses();
        }

        public Course FindCourse(string courseCode)
        {
            return _courseService.FindCourse(courseCode);
        }
    }
}