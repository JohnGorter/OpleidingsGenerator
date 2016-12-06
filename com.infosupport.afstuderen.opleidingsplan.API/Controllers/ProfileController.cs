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
    public class ProfileController : ApiController
    {
        private IAdministrationManager _administrationManager;

        public ProfileController()
        {
            string profilepath = dal.Configuration.GetConfiguration().ProfilePath;
            string pathToProfiles = HttpContext.Current.Server.MapPath(profilepath);

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
        public Profile Get(int id)
        {
            return _administrationManager.FindProfileById(id);
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
