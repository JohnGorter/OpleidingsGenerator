using com.infosupport.afstuderen.opleidingsplan.api.managers;
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
    public class ManagementPropertiesController : ApiController
    {
        private readonly IManagementPropertiesManager _managementPropertiesManager;
        public ManagementPropertiesController(IManagementPropertiesManager managementPropertiesManager)
        {
            _managementPropertiesManager = managementPropertiesManager;
        }

        public ManagementPropertiesController()
        {
            string managementPropertiesPath = dal.DALConfiguration.GetConfiguration().ManagementPropertiesPath;
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
            _managementPropertiesManager.Update(properties);
        }
    }
}
