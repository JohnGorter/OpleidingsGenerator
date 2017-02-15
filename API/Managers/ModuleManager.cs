using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InfoSupport.KC.OpleidingsplanGenerator.Models;
using InfoSupport.KC.OpleidingsplanGenerator.Dal.Mappers;

namespace InfoSupport.KC.OpleidingsplanGenerator.Api.Managers
{
    public class ModuleManager : IModuleManager
    {
        private readonly IDataMapper<Module> _moduleDataMapper;

        public ModuleManager(string pathToModules)
        {
            _moduleDataMapper = new ModuleJSONDataMapper(pathToModules);
        }

        public void Delete(long id)
        {
            _moduleDataMapper.Delete(id);
        }

        public Module FindModuleById(long id)
        {
            return _moduleDataMapper.FindById(id);
        }

        public IEnumerable<Module> FindModules()
        {
            return _moduleDataMapper.FindAll();
        }

        public void Insert(Module module)
        {
            _moduleDataMapper.Insert(module);
        }

        public void Update(Module module)
        {
            _moduleDataMapper.Update(module);
        }
    }
}