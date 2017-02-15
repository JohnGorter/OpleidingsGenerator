using InfoSupport.KC.OpleidingsplanGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSupport.KC.OpleidingsplanGenerator.Api.Managers
{
    public interface IModuleManager
    {
        IEnumerable<Module> FindModules();
        Module FindModuleById(long id);
        void Insert(Module module);
        void Update(Module module);
        void Delete(long id);
    }
}
