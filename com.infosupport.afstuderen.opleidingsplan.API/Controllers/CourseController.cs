using AutoMapper;
using com.infosupport.afstuderen.opleidingsplan.api.managers;
using com.infosupport.afstuderen.opleidingsplan.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace com.infosupport.afstuderen.opleidingsplan.api.controllers
{
    [EnableCors("*","*","*")]
    public class CourseController : ApiController
    {
        private readonly ICourseManager _courseManager;

        public CourseController()
        {
            string profilepath = dal.DALConfiguration.GetConfiguration().ProfilePath;
            string pathToProfiles = HttpContext.Current.Server.MapPath(profilepath);

            _courseManager = new CourseManager(pathToProfiles);
        }

        public CourseController(ICourseManager courseManager)
        {
            _courseManager = courseManager;
        }

        // GET: api/Course
        public IEnumerable<opleidingsplan.models.CourseSummary> Get()
        {
            var courses = _courseManager.FindCourses().Coursesummary;
            return Mapper.Map<IEnumerable<opleidingsplan.models.CourseSummary>>(courses);
        }

        // GET: api/Course/POLDEVEL
        public opleidingsplan.models.Course Get(string id)
        {
            var course = _courseManager.FindCourse(id);
            return Mapper.Map<opleidingsplan.models.Course>(course);
        }

        // POST: api/Course
        public void Post(CoursePriority course)
        {
            _courseManager.Update(course);
        }

        // PUT: api/Course/course
        public void Put(CoursePriority course)
        {
            _courseManager.Insert(course);
        }

        // DELETE: api/Course/course
        public void Delete(CoursePriority course)
        {
            _courseManager.Delete(course);
        }
    }
}
