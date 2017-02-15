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

        public EducationPlanController()
        {
            string profilepath = HttpContext.Current.Server.MapPath(DalConfiguration.Configuration.ProfilePath);
            string managementPropertiesPath = HttpContext.Current.Server.MapPath(DalConfiguration.Configuration.ManagementPropertiesPath);
            string educationPlanPath = HttpContext.Current.Server.MapPath(DalConfiguration.Configuration.EducationPlanPath);
            string educationPlanUpdatedPath = HttpContext.Current.Server.MapPath(DalConfiguration.Configuration.EducationPlanUpdatedPath);
            string educationPlanFilesDir = HttpContext.Current.Server.MapPath(GeneratorConfiguration.Configuration.EducationPlanFileDirPath);
            string modulePath = HttpContext.Current.Server.MapPath(DalConfiguration.Configuration.ModulePath);

            _educationPlanManager = new EducationPlanManager(profilepath, managementPropertiesPath, educationPlanPath, educationPlanUpdatedPath, educationPlanFilesDir, modulePath);
        }

        public EducationPlanController(IEducationPlanManager educationPlanManager)
        {
            _educationPlanManager = educationPlanManager;
        }

        [HttpPost]
        [Route("api/EducationPlan/generate")]
        public EducationPlan GenerateEducationPlan(RestEducationPlan educationPlan)
        {
            return _educationPlanManager.PreviewEducationPlan(educationPlan);
        }

        public long Put(RestEducationPlan educationPlan)
        {
            return _educationPlanManager.SaveEducationPlan(educationPlan);
        }

        public long Post(RestEducationPlan educationPlan)
        {
            return _educationPlanManager.UpdateEducationPlan(educationPlan);
        }

        public EducationPlan Get(long id)
        {
            return _educationPlanManager.FindEducationPlan(id);
        }

        public void Delete(long id)
        {
            _educationPlanManager.DeleteEducationPlan(id);
        }

        [HttpGet]
        [Route("api/EducationPlan/search")]
        public List<EducationPlan> Get(string name, long? date)
        {
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
            var educationPlan = _educationPlanManager.FindEducationPlan(id);
            return _educationPlanManager.GenerateWordFile(educationPlan);
        }

        [HttpGet]
        [Route("api/FindAllUpdated")]
        public List<EducationPlanCompareSummary> FindAllUpdated()
        {
            return _educationPlanManager.FindAllUpdated();
        }

        [HttpGet]
        [Route("api/FindUpdated/{id}")]
        public EducationPlanCompare FindUpdated(long id)
        {
            return _educationPlanManager.FindUpdatedById(id);
        }

        [HttpPost]
        [Route("api/ApproveUpdatedEducationPlan/{id}")]
        public void ApproveUpdatedEducationPlan(long id)
        {
            _educationPlanManager.ApproveUpdatedEducationPlan(id);
        }

        [HttpPost]
        [Route("api/RejectUpdatedEducationPlan/{id}")]
        public void RejectUpdatedEducationPlan(long id)
        {
            _educationPlanManager.RejectUpdatedEducationPlan(id);
        }

        [HttpPost]
        [Route("api/ApproveEducationPlan/{id}")]
        public void ApproveEducationPlan(long id)
        {
            _educationPlanManager.ChangeStatusEducationPlan(id, EducationplanStatus.Approved);
        }

        [HttpPost]
        [Route("api/CompletedEducationPlan/{id}")]
        public void CompletedEducationPlan(long id)
        {
            _educationPlanManager.ChangeStatusEducationPlan(id, EducationplanStatus.Completed);
        }

        [HttpGet]
        [Route("api/FindAllApproved")]
        public List<EducationPlan> FindAllApproved()
        {
            return _educationPlanManager.FindApprovedEducationPlans();
        }
    }
}
