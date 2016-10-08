using com.infosupport.afstuderen.opleidingsplan.api.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace com.infosupport.afstuderen.opleidingsplan.api.Controllers
{
    public class CourseController : ApiController
    {
        private CourseManager _courseManager;

        public CourseController()
        {
            _courseManager = new CourseManager();
        }
        // GET: api/Training
        public IEnumerable<agent.Coursesummary> Get()
        {
            return _courseManager.FindCourses().Coursesummary;
        }

        // GET: api/Training/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Training
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Training/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Training/5
        public void Delete(int id)
        {
        }
    }
}
