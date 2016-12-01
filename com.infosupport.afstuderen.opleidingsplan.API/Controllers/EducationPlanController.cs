using AutoMapper;
using com.infosupport.afstuderen.opleidingsplan.api.Managers;
using com.infosupport.afstuderen.opleidingsplan.api.Models;
using com.infosupport.afstuderen.opleidingsplan.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace com.infosupport.afstuderen.opleidingsplan.api.Controllers
{
    [EnableCors("*", "*", "*")]
    public class EducationPlanController : ApiController
    {
        private IEducationPlanManager _educationPlanManager;

        public EducationPlanController()
        {
            string profilepath = DAL.Configuration.GetConfiguration().ProfilePath;
            string pathToProfiles = HttpContext.Current.Server.MapPath(profilepath);

            _educationPlanManager = new EducationPlanManager(pathToProfiles);
        }

        public EducationPlanController(IEducationPlanManager educationPlanManager)
        {
            _educationPlanManager = educationPlanManager;
        }

        // GET: api/EducationPlan
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/EducationPlan/5
        public EducationPlan Get(RestEducationPlan educationPlan)
        {
            throw new NotImplementedException();
        }
        

        // POST: api/EducationPlan
        public EducationPlan Post(RestEducationPlan educationPlan)
        {
            return _educationPlanManager.GenerateEducationPlan(educationPlan);
        }

        // PUT: api/EducationPlan/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/EducationPlan/5
        public void Delete(int id)
        {
        }
    }
}
