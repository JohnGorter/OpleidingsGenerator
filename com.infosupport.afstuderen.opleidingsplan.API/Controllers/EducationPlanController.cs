using AutoMapper;
using com.infosupport.afstuderen.opleidingsplan.api.managers;
using com.infosupport.afstuderen.opleidingsplan.api.models;
using com.infosupport.afstuderen.opleidingsplan.dal;
using com.infosupport.afstuderen.opleidingsplan.dal.mappers;
using com.infosupport.afstuderen.opleidingsplan.generator;
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
            string profilepath = HttpContext.Current.Server.MapPath(DALConfiguration.Configuration.ProfilePath);
            string managementPropertiesPath = HttpContext.Current.Server.MapPath(DALConfiguration.Configuration.ManagementPropertiesPath);
            string educationPlanPath = HttpContext.Current.Server.MapPath(DALConfiguration.Configuration.EducationPlanPath);
            string educationPlanUpdatedPath = HttpContext.Current.Server.MapPath(DALConfiguration.Configuration.EducationPlanUpdatedPath);
            string educationPlanFilesDir = HttpContext.Current.Server.MapPath(GeneratorConfiguration.Configuration.EducationPlanFileDirPath);

            _educationPlanManager = new EducationPlanManager(profilepath, managementPropertiesPath, educationPlanPath, educationPlanUpdatedPath, educationPlanFilesDir);
        }

        public EducationPlanController(IEducationPlanManager educationPlanManager)
        {
            _educationPlanManager = educationPlanManager;
        }

        public EducationPlan Post(RestEducationPlan educationPlan)
        {
            return _educationPlanManager.GenerateEducationPlan(educationPlan);
        }

        public long Put(RestEducationPlan educationPlan)
        {
            return _educationPlanManager.SaveEducationPlan(educationPlan);
        }

        public EducationPlan Get(long id)
        {
             return _educationPlanManager.FindEducationPlan(id);
        }

        [HttpGet]
        [Route("api/EducationPlan/search")]
        public List<EducationPlan> Get(string name, DateTime? date)
        {
            return _educationPlanManager.FindEducationPlans(new EducationPlanSearch
            {
                Name = name,
                Date = date,
            });
        }

        [HttpGet]
        [Route("api/GenerateWordFile/{id}")]
        public string GenerateWordFile(long id)
        {
            var educationPlan = _educationPlanManager.FindEducationPlan(id);
            return _educationPlanManager.GenerateWordFile(educationPlan);
        }
    }
}
