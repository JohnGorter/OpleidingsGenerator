using AutoMapper;
using com.infosupport.afstuderen.opleidingsplan.api.managers;
using com.infosupport.afstuderen.opleidingsplan.api.models;
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
    [EnableCors("*", "*", "*")]
    public class EducationPlanController : ApiController
    {
        private readonly IEducationPlanManager _educationPlanManager;

        public EducationPlanController()
        {
            string profilepath = dal.DALConfiguration.GetConfiguration().ProfilePath;
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
            throw new NotSupportedException();
        }

        // GET: api/EducationPlan/5
        public EducationPlan Get(RestEducationPlan educationPlan)
        {
            throw new NotSupportedException();
        }
        

        // POST: api/EducationPlan
        public EducationPlan Post(RestEducationPlan educationPlan)
        {
            return _educationPlanManager.GenerateEducationPlan(educationPlan);
        }

        // PUT: api/EducationPlan/5
        public void Put(int id, [FromBody]string value)
        {
            throw new NotSupportedException();
        }

        // DELETE: api/EducationPlan/5
        public void Delete(int id)
        {
            throw new NotSupportedException();
        }
    }
}
