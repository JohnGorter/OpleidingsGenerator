using com.infosupport.afstuderen.opleidingsplan.integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.infosupport.afstuderen.opleidingsplan.models;
using com.infosupport.afstuderen.opleidingsplan.dal.mappers;

namespace com.infosupport.afstuderen.opleidingsplan.api.managers
{
    public class CourseManager : ICourseManager
    {
        private readonly ICourseService _courseService;
        private readonly ICourseDataMapper _courseDataMapper;

        public CourseManager(string pathToProfiles)
        {
            _courseService = new CourseService();
            _courseDataMapper = new CourseJsonDataMapper(pathToProfiles);
        }

        public CourseManager(ICourseService courseManager)
        {
            _courseService = courseManager;
        }

        public CourseManager(ICourseDataMapper courseDataMapper)
        {
            _courseDataMapper = courseDataMapper;
        }

        public Coursesummarycollection FindCourses()
        {
            return _courseService.FindAllCourses();
        }

        public integration.Course FindCourse(string courseCode)
        {
            return _courseService.FindCourse(courseCode);
        }

        public void Insert(CoursePriority course)
        {
            _courseDataMapper.Insert(course);
        }

        public void Update(CoursePriority course)
        {
            _courseDataMapper.Update(course);
        }

        public void Delete(CoursePriority course)
        {
            _courseDataMapper.Delete(course);
        }
    }
}