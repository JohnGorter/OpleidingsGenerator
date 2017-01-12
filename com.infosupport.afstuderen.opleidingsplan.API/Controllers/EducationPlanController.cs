using AutoMapper;
using com.infosupport.afstuderen.opleidingsplan.api.managers;
using com.infosupport.afstuderen.opleidingsplan.api.models;
using com.infosupport.afstuderen.opleidingsplan.dal;
using com.infosupport.afstuderen.opleidingsplan.dal.mappers;
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

            _educationPlanManager = new EducationPlanManager(profilepath, managementPropertiesPath, educationPlanPath, educationPlanUpdatedPath);
        }

        public EducationPlanController(IEducationPlanManager educationPlanManager)
        {
            _educationPlanManager = educationPlanManager;
        }

        // POST: api/EducationPlan
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

        public List<EducationPlan> Search(EducationPlanSearch search)
        {
            return _educationPlanManager.FindEducationPlans(search);
        }
    }
}
