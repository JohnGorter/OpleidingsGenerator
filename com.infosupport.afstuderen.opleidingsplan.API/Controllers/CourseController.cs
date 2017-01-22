using AutoMapper;
using com.infosupport.afstuderen.opleidingsplan.api.managers;
using com.infosupport.afstuderen.opleidingsplan.models;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private static ILog _logger = LogManager.GetLogger(typeof(CourseController));
        private readonly CultureInfo _culture = new CultureInfo("nl-NL");

        private readonly ICourseManager _courseManager;

        public CourseController()
        {
            string profilepath = dal.DalConfiguration.Configuration.ProfilePath;
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
            _logger.Info("Get all courses");
            var courses = _courseManager.FindCourses().Coursesummary;
            return Mapper.Map<IEnumerable<opleidingsplan.models.CourseSummary>>(courses);
        }

        // GET: api/Course/POLDEVEL
        public opleidingsplan.models.Course Get(string id)
        {
            _logger.Info(string.Format(_culture, "Get course with id {0}", id));
            var course = _courseManager.FindCourse(id);
            return Mapper.Map<opleidingsplan.models.Course>(course);
        }

        // POST: api/Course
        public void Post(CoursePriority course)
        {
            if(ModelState.IsValid)
            {
                _logger.Info(string.Format(_culture, "Post course {0}", course.Code));
                _courseManager.Update(course);
            }
            else
            {
                _logger.Warn(string.Format(_culture, "Post course {0} modelstate not valid", course.Code));
            }
        }

        // PUT: api/Course/course
        public void Put(CoursePriority course)
        {
            if (ModelState.IsValid)
            {
                _logger.Info(string.Format(_culture, "Put course {0}", course.Code));
                _courseManager.Insert(course);
            }
            else
            {
                _logger.Warn(string.Format(_culture, "Put course {0} modelstate not valid", course.Code));
            }
        }

        // DELETE: api/Course/course
        public void Delete(CoursePriority course)
        {
            if (ModelState.IsValid)
            {
                _logger.Info(string.Format(_culture, "Delete course {0}", course.Code));
                _courseManager.Delete(course);
            }
            else
            {
                _logger.Warn(string.Format(_culture, "Delete course {0} modelstate not valid", course.Code));
            }
        }
    }
}
