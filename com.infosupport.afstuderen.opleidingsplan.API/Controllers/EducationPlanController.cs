using AutoMapper;
using com.infosupport.afstuderen.opleidingsplan.api.managers;
using com.infosupport.afstuderen.opleidingsplan.api.models;
using com.infosupport.afstuderen.opleidingsplan.dal;
using com.infosupport.afstuderen.opleidingsplan.dal.mappers;
using com.infosupport.afstuderen.opleidingsplan.generator;
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
    [EnableCors("*", "*", "*")]
    public class EducationPlanController : ApiController
    {
        private readonly IEducationPlanManager _educationPlanManager;
        private static ILog _logger = LogManager.GetLogger(typeof(EducationPlanController));
        private readonly CultureInfo _culture = new CultureInfo("nl-NL");

        public EducationPlanController()
        {
            string profilepath = HttpContext.Current.Server.MapPath(DalConfiguration.Configuration.ProfilePath);
            string managementPropertiesPath = HttpContext.Current.Server.MapPath(DalConfiguration.Configuration.ManagementPropertiesPath);
            string educationPlanPath = HttpContext.Current.Server.MapPath(DalConfiguration.Configuration.EducationPlanPath);
            string educationPlanUpdatedPath = HttpContext.Current.Server.MapPath(DalConfiguration.Configuration.EducationPlanUpdatedPath);
            string educationPlanFilesDir = HttpContext.Current.Server.MapPath(GeneratorConfiguration.Configuration.EducationPlanFileDirPath);

            _educationPlanManager = new EducationPlanManager(profilepath, managementPropertiesPath, educationPlanPath, educationPlanUpdatedPath, educationPlanFilesDir);
        }

        public EducationPlanController(IEducationPlanManager educationPlanManager)
        {
            _educationPlanManager = educationPlanManager;
        }

        [HttpPost]
        [Route("api/EducationPlan/generate")]
        public EducationPlan GenerateEducationPlan(RestEducationPlan educationPlan)
        {
            _logger.Info("GenerateEducationPlan");
            return _educationPlanManager.GenerateEducationPlan(educationPlan);
        }

        public long Put(RestEducationPlan educationPlan)
        {
            _logger.Info("Put educationplan");
            return _educationPlanManager.SaveEducationPlan(educationPlan);
        }

        public long Post(RestEducationPlan educationPlan)
        {
            _logger.Info("Post educationplan");
            return _educationPlanManager.UpdateEducationPlan(educationPlan);
        }

        public EducationPlan Get(long id)
        {
            _logger.Info(string.Format(_culture, "Get educationplan with id {0}", id));
            return _educationPlanManager.FindEducationPlan(id);
        }

        public void Delete(long id)
        {
            _logger.Info(string.Format(_culture, "Delete educationplan with id {0}", id));
            _educationPlanManager.DeleteEducationPlan(id);
        }

        [HttpGet]
        [Route("api/EducationPlan/search")]
        public List<EducationPlan> Get(string name, long? date)
        {
            _logger.Info(string.Format(_culture, "Get educationplan with name {0} and date {1}", name, date));
            DateTime? dateCreated = null;

            if (date.HasValue)
            {
                dateCreated = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(date.Value).AddHours(1).Date;
            }

            return _educationPlanManager.FindEducationPlans(new EducationPlanSearch
            {
                Name = name,
                Date = dateCreated,
            });
        }

        [HttpGet]
        [Route("api/GenerateWordFile/{id}")]
        public string GenerateWordFile(long id)
        {
            _logger.Info(string.Format(_culture, "GenerateWordFile with id {0}", id));
            var educationPlan = _educationPlanManager.FindEducationPlan(id);
            return _educationPlanManager.GenerateWordFile(educationPlan);
        }
    }
}
