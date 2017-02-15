using InfoSupport.KC.OpleidingsplanGenerator.Api.Filters;
using InfoSupport.KC.OpleidingsplanGenerator.Api.Managers;
using InfoSupport.KC.OpleidingsplanGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace InfoSupport.KC.OpleidingsplanGenerator.Api.Controllers
{
    [LogActionFilter]
    [LogExceptionFilter]
    public class ModuleController : ApiController
    {
        private readonly IModuleManager _moduleManager;

        public ModuleController()
        {
            string profilepath = Dal.DalConfiguration.Configuration.ModulePath;
            string pathToProfiles = HttpContext.Current.Server.MapPath(profilepath);

            _moduleManager = new ModuleManager(pathToProfiles);
        }

        // GET: api/Module
        public IEnumerable<Module> Get()
        {
            return _moduleManager.FindModules();
        }

        // GET: api/Module/5
        public Module Get(int id)
        {
            return _moduleManager.FindModuleById(id);
        }

        // POST: api/Module
        public void Post(Module module)
        {
            _moduleManager.Update(module);
        }

        // PUT: api/Module
        public void Put(Module module)
        {
            _moduleManager.Insert(module);
        }

        // DELETE: api/Module/5
        public void Delete(int id)
        {
            _moduleManager.Delete(id);
        }
    }
}
