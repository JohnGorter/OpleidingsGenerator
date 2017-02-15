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
        private readonly CultureInfo _culture = new CultureInfo("nl-NL");

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
            _logger.Info("Get all course profiles");
            return _administrationManager.FindProfiles();
        }

        // GET: api/Profile/5
        public CourseProfile Get(int? id)
        {
            _logger.Info(string.Format(_culture, "Get all course profile with id {0}", id));
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
                _logger.Info("Post course profile");
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
                _logger.Info("Put course profile");
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
            _logger.Info(string.Format(_culture, "Delete course profile with id {0}", id));
            _administrationManager.Delete(id);
        }
    }
}
