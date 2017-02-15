using InfoSupport.KC.OpleidingsplanGenerator.Api.Filters;
using InfoSupport.KC.OpleidingsplanGenerator.Api.Managers;
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
    public class ManagementPropertiesController : ApiController
    {
        private readonly IManagementPropertiesManager _managementPropertiesManager;
        public ManagementPropertiesController(IManagementPropertiesManager managementPropertiesManager)
        {
            _managementPropertiesManager = managementPropertiesManager;
        }

        public ManagementPropertiesController()
        {
            string managementPropertiesPath = Dal.DalConfiguration.Configuration.ManagementPropertiesPath;
            string pathToManagementProperties = HttpContext.Current.Server.MapPath(managementPropertiesPath);

            _managementPropertiesManager = new ManagementPropertiesManager(pathToManagementProperties);
        }

        // GET: api/ManagementProperties
        public ManagementProperties Get()
        {
            return _managementPropertiesManager.FindManagementProperties();
        }

        // POST: api/ManagementProperties
        public void Post(ManagementProperties properties)
        {
            if (ModelState.IsValid)
            {
                _managementPropertiesManager.Update(properties);
            }
        }
    }
}
