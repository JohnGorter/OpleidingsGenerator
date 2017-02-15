using AutoMapper;
using InfoSupport.KC.OpleidingsplanGenerator.Api.Filters;
using InfoSupport.KC.OpleidingsplanGenerator.Api.Managers;
using InfoSupport.KC.OpleidingsplanGenerator.Api.Models;
using InfoSupport.KC.OpleidingsplanGenerator.Dal;
using InfoSupport.KC.OpleidingsplanGenerator.Dal.Mappers;
using InfoSupport.KC.OpleidingsplanGenerator.Generator;
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
            return _educationPlanManager.PreviewEducationPlan(educationPlan);
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

        [HttpGet]
        [Route("api/FindAllUpdated")]
        public List<EducationPlanCompareSummary> FindAllUpdated()
        {
            _logger.Info("FindAllUpdated");
            return _educationPlanManager.FindAllUpdated();
        }

        [HttpGet]
        [Route("api/FindUpdated/{id}")]
        public EducationPlanCompare FindUpdated(long id)
        {
            _logger.Info(string.Format(_culture, "Find updated educationplan with id {0}", id));
            return _educationPlanManager.FindUpdatedById(id);
        }

        [HttpPost]
        [Route("api/ApproveUpdatedEducationPlan/{id}")]
        public void ApproveUpdatedEducationPlan(long id)
        {
            _logger.Info(string.Format(_culture, "Approve updated educationplan with id {0}", id));
            _educationPlanManager.ApproveUpdatedEducationPlan(id);
        }

        [HttpPost]
        [Route("api/RejectUpdatedEducationPlan/{id}")]
        public void RejectUpdatedEducationPlan(long id)
        {
            _logger.Info(string.Format(_culture, "Reject updated educationplan with id {0}", id));
            _educationPlanManager.RejectUpdatedEducationPlan(id);
        }
    }
}
