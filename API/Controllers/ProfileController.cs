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
    public class ProfileController : ApiController
    {
        private readonly IProfileManager _administrationManager;
        private static ILog _logger = LogManager.GetLogger(typeof(CourseController));

        public ProfileController()
        {
            string profilepath = Dal.DalConfiguration.Configuration.ProfilePath;
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
        public CourseProfile Get(int? id)
        {
            if (!id.HasValue)
            {
                return new CourseProfile();
            }

            return _administrationManager.FindProfileById(id.Value);
        }

        // POST: api/Profile
        public void Post(CourseProfile profile)
        {
            if (ModelState.IsValid)
            {
                _administrationManager.Update(profile);
            }
            else
            {
                _logger.Warn("Post course profile modelstate is not valid");
            }
        }

        // PUT: api/Profile/profile
        public void Put(CourseProfile profile)
        {
            if (ModelState.IsValid)
            {
                _administrationManager.Insert(profile);
            }
            else
            {
                _logger.Warn("Put course profile modelstate is not valid");
            }
        }

        // DELETE: api/Profile/profile
        public void Delete(long id)
        {
            _administrationManager.Delete(id);
        }
    }
}
