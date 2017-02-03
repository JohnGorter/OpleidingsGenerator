using InfoSupport.KC.OpleidingsplanGenerator.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InfoSupport.KC.OpleidingsplanGenerator.Models;
using InfoSupport.KC.OpleidingsplanGenerator.Dal.Mappers;

namespace InfoSupport.KC.OpleidingsplanGenerator.Api.Managers
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

        public Integration.Course FindCourse(string courseCode)
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