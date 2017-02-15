using InfoSupport.KC.OpleidingsplanGenerator.Models;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSupport.KC.OpleidingsplanGenerator.Dal.Mappers
{
    public class ModuleJSONDataMapper : IDataMapper<Module>
    {
        private readonly string _path;
        private readonly CultureInfo _culture = new CultureInfo("nl-NL");
        private static ILog _logger = LogManager.GetLogger(typeof(ModuleJSONDataMapper));

        public ModuleJSONDataMapper(string path)
        {
            _path = path;
        }

        public void Delete(long id)
        {
            _logger.Debug(string.Format(_culture, "Delete module with id {0}", id));

            var modules = GetAllModules();
            Module moduleToDelete = modules.FirstOrDefault(module => module.Id == id);

            if (moduleToDelete == null)
            {
                string errorMessage = string.Format(_culture, "No module found with id {0}", id);
                _logger.Error("ArgumentException: " + errorMessage);
                throw new ArgumentException(errorMessage);
            }

            modules.Remove(moduleToDelete);

            WriteAllModulesToFile(modules);

            _logger.Debug(string.Format(_culture, "Module deleted with id {0} with name {1}", moduleToDelete.Id, moduleToDelete.Name));
        }

        public IEnumerable<Module> Find(Func<Module, bool> predicate)
        {
            _logger.Debug("Find module");
            return GetAllModules().Where(predicate);
        }

        public IEnumerable<Module> FindAll()
        {
            _logger.Debug("Find all modules");
            return GetAllModules();
        }

        public Module FindById(long id)
        {
            _logger.Debug(string.Format(_culture, "Find module by id {0} ", id));
            Module foundModule = GetAllModules().FirstOrDefault(module => module.Id == id);

            if (foundModule == null)
            {
                string errorMessage = string.Format(_culture, "No module found with id {0}", id);
                _logger.Error("ArgumentException: " + errorMessage);
                throw new ArgumentException(errorMessage);
            }

            _logger.Debug(string.Format(_culture, "Module found by id {0} with name {1}", id, foundModule.Name));
            return foundModule;
        }

        public void Insert(Module module)
        {
            if (module == null)
            {
                _logger.Error("ArgumentNullException module");
                throw new ArgumentNullException("module");
            }

            _logger.Debug(string.Format(_culture, "Insert module with name {0}", module.Name));
            var modules = GetAllModules();
            module.Id = GenerateId(modules);

            if (modules.Any(p => p.Name == module.Name))
            {
                string errorMessage = string.Format(_culture, "Module with the name {0} already exists", module.Name);
                _logger.Error("ArgumentException: " + errorMessage);
                throw new ArgumentException(errorMessage);
            }

            modules.Add(module);
            WriteAllModulesToFile(modules);
            _logger.Debug(string.Format(_culture, "Module with name {0} inserted with generated id {1}", module.Name, module.Id));
        }

        private long GenerateId(List<Module> allModules)
        {
            _logger.Debug("Generate id for module");

            long newId = 1;
            if (allModules.Any())
            {
                newId = allModules.Max(module => module.Id) + 1;
            }

            _logger.Debug(string.Format(_culture, "Generated id for module {0}", newId));

            return newId;
        }

        public void Update(Module module)
        {
            if (module == null)
            {
                _logger.Error("ArgumentNullException module");
                throw new ArgumentNullException("module");
            }

            var modules = GetAllModules();
            Module moduleToUpdate = modules.FirstOrDefault(p => p.Id == module.Id);

            if (moduleToUpdate == null)
            {
                string errorMessage = string.Format(_culture, "No modules found with id {0}", module.Id);
                _logger.Error("ArgumentException: " + errorMessage);
                throw new ArgumentException(errorMessage);
            }

            _logger.Debug(string.Format(_culture, "Update module with id {0} from name {1} to name {2}", module.Id, moduleToUpdate.Name, module.Name));

            int index = modules.IndexOf(moduleToUpdate);
            modules[index] = module;

            WriteAllModulesToFile(modules);
            _logger.Debug(string.Format(_culture, "Modules updated with id {0} and name {1}", module.Id, module.Name));
        }


        private List<Module> GetAllModules()
        {
            try
            {
                string modules = File.ReadAllText(_path);
                _logger.Debug(string.Format(_culture, "Get all modules from file with path {0}", _path));
                return JsonConvert.DeserializeObject<List<Module>>(modules);
            }
            catch (FileNotFoundException ex)
            {
                _logger.Error(string.Format(_culture, "File {0} to get all modules not found", _path), ex);
                throw;
            }
            catch (JsonReaderException ex)
            {
                _logger.Error(string.Format(_culture, "Couldn't deserialize modules from path {0}", _path), ex);
                throw;
            }
        }

        private void WriteAllModulesToFile(List<Module> modules)
        {
            var convertedJson = JsonConvert.SerializeObject(modules, Formatting.Indented);
            try
            {
                File.WriteAllText(_path, convertedJson);
                _logger.Debug(string.Format(_culture, "Saved all modules to file with path {0}", _path));
            }
            catch (FileNotFoundException ex)
            {
                _logger.Error(string.Format(_culture, "File {0} to write all modules not found", _path), ex);
                throw;
            }
        }
    }
}
