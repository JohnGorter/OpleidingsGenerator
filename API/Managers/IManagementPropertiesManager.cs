using InfoSupport.KC.OpleidingsplanGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSupport.KC.OpleidingsplanGenerator.Api.Managers
{
    public interface IManagementPropertiesManager
    {
        ManagementProperties FindManagementProperties();
        void Update(ManagementProperties properties);
    }
}
