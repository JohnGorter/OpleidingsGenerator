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
        private readonly IProfileManager _administrationManager;

        public ProfileController()
        {
            string profilepath = dal.DALConfiguration.GetConfiguration().ProfilePath;
            string pathToProfiles = HttpContext.Current.Server.MapPath(profilepath);

            _administrationManager = new ProfileManager(pathToProfiles);
        }

        public ProfileController(IProfileManager administrationManager)
        {
            _administrationManager = administrationManager;
        }

        // GET: api/Profile
        public IEnumerable<CourseProfile> Get()
        {
            return _administrationManager.FindProfiles();
        }

        // GET: api/Profile/5
        public CourseProfile Get(int id)
        {
            return _administrationManager.FindProfileById(id);
        }

        // POST: api/Profile
        public void Post(CourseProfile profile)
        {
            _administrationManager.Update(profile);
        }

        // PUT: api/Profile/profile
        public void Put(CourseProfile profile)
        {
            _administrationManager.Insert(profile);
        }

        // DELETE: api/Profile/profile
        public void Delete(CourseProfile profile)
        {
            _administrationManager.Delete(profile);
        }
    }
}
