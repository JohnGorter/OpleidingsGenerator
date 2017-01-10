﻿using AutoMapper;
using com.infosupport.afstuderen.opleidingsplan.api.managers;
using com.infosupport.afstuderen.opleidingsplan.api.models;
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
            string profilepath = dal.DALConfiguration.Configuration.ProfilePath;
            string profilepathMapped = HttpContext.Current.Server.MapPath(profilepath);
            string managementPropertiesPath = dal.DALConfiguration.Configuration.ManagementPropertiesPath;
            string managementPropertiesPathMapped = HttpContext.Current.Server.MapPath(managementPropertiesPath);

            _educationPlanManager = new EducationPlanManager(profilepathMapped, managementPropertiesPathMapped);
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
    }
}
