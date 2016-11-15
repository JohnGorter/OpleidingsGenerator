using AutoMapper;
using com.infosupport.afstuderen.opleidingsplan.api.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace com.infosupport.afstuderen.opleidingsplan.api.Controllers
{
    [EnableCors("*","*","*")]
    public class CourseController : ApiController
    {
        private ICourseManager _courseManager;

        public CourseController()
        {
            _courseManager = new CourseManager();
        }

        public CourseController(ICourseManager courseManager)
        {
            _courseManager = courseManager;
        }

        // GET: api/Training
        public IEnumerable<model.CourseSummary> Get()
        {
            var courses = _courseManager.FindCourses().Coursesummary;
            return Mapper.Map<IEnumerable<model.CourseSummary>>(courses);
        }

        // GET: api/Training/5
        public model.Course Get(string id)
        {
            var course = _courseManager.FindCourse(id);
            return Mapper.Map<model.Course>(course);
        }
    }
}
