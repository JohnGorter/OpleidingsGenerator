﻿using AutoMapper;
using InfoSupport.KC.OpleidingsplanGenerator.Api.Filters;
using InfoSupport.KC.OpleidingsplanGenerator.Api.Managers;
using InfoSupport.KC.OpleidingsplanGenerator.Models;
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

namespace InfoSupport.KC.OpleidingsplanGenerator.Api.Controllers
{
    [LogActionFilter]
    [LogExceptionFilter]
    [EnableCors("*","*","*")]
    public class CourseController : ApiController
    {
        private static ILog _logger = LogManager.GetLogger(typeof(CourseController));
        private readonly CultureInfo _culture = new CultureInfo("nl-NL");

        private readonly ICourseManager _courseManager;

        public CourseController()
        {
            string profilepath = Dal.DalConfiguration.Configuration.ProfilePath;
            string pathToProfiles = HttpContext.Current.Server.MapPath(profilepath);

            _courseManager = new CourseManager(pathToProfiles);
        }

        public CourseController(ICourseManager courseManager)
        {
            _courseManager = courseManager;
        }

        // GET: api/Course
        public IEnumerable<OpleidingsplanGenerator.Models.CourseSummary> Get()
        {
            var courses = _courseManager.FindCourses().Coursesummary;
            return Mapper.Map<IEnumerable<OpleidingsplanGenerator.Models.CourseSummary>>(courses);
        }

        // GET: api/Course/POLDEVEL
        public OpleidingsplanGenerator.Models.Course Get(string id)
        {
            var course = _courseManager.FindCourse(id);
            return Mapper.Map<OpleidingsplanGenerator.Models.Course>(course);
        }

        // POST: api/Course
        public void Post(CoursePriority course)
        {
            if(ModelState.IsValid)
            {
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
                _courseManager.Delete(course);
            }
            else
            {
                _logger.Warn(string.Format(_culture, "Delete course {0} modelstate not valid", course.Code));
            }
        }
    }
}
