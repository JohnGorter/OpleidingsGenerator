using com.infosupport.afstuderen.opleidingsplan.api.managers;
using com.infosupport.afstuderen.opleidingsplan.api.models;
using com.infosupport.afstuderen.opleidingsplan.dal;
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
    public class EducationPlanSearchController : ApiController
    {
        private readonly IEducationPlanManager _educationPlanManager;

        public EducationPlanSearchController()
        {
            string profilepath = HttpContext.Current.Server.MapPath(DALConfiguration.Configuration.ProfilePath);
            string managementPropertiesPath = HttpContext.Current.Server.MapPath(DALConfiguration.Configuration.ManagementPropertiesPath);
            string educationPlanPath = HttpContext.Current.Server.MapPath(DALConfiguration.Configuration.EducationPlanPath);
            string educationPlanUpdatedPath = HttpContext.Current.Server.MapPath(DALConfiguration.Configuration.EducationPlanUpdatedPath);

            _educationPlanManager = new EducationPlanManager(profilepath, managementPropertiesPath, educationPlanPath, educationPlanUpdatedPath);
        }

        public EducationPlanSearchController(IEducationPlanManager educationPlanManager)
        {
            _educationPlanManager = educationPlanManager;
        }

        // GET: api/EducationPlanSearch
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    
        // GET: api/EducationPlanSearch/5
        public List<EducationPlan> Get(string name, DateTime? date)
        {
            return _educationPlanManager.FindEducationPlans(new EducationPlanSearch
            {
                Name = name,
                Date = date,
            });
        }

        // POST: api/EducationPlanSearch
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/EducationPlanSearch/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/EducationPlanSearch/5
        public void Delete(int id)
        {
        }
    }
}
