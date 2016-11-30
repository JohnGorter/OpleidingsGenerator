using com.infosupport.afstuderen.opleidingsplan.api.Managers;
using com.infosupport.afstuderen.opleidingsplan.api.Models;
using com.infosupport.afstuderen.opleidingsplan.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace com.infosupport.afstuderen.opleidingsplan.api.Controllers
{
    public class ProfileController : ApiController
    {
        private IAdministrationManager _administrationManager;

        public ProfileController()
        {
            string pathToProfiles = HttpContext.Current.Server.MapPath("~/App_Data/Profiles.json");
            _administrationManager = new AdministrationManager(pathToProfiles);
        }

        public ProfileController(IAdministrationManager administrationManager)
        {
            _administrationManager = administrationManager;
        }

        // GET: api/Administration
        public IEnumerable<Profile> Get()
        {
            return _administrationManager.FindProfiles();
        }

        // GET: api/Administration/5
        public Profile Get(string profileName)
        {
            return _administrationManager.FindProfile(profileName);
        }

        // POST: api/Administration
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Administration/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Administration/5
        public void Delete(int id)
        {
        }
    }
}
