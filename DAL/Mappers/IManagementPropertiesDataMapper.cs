using InfoSupport.KC.OpleidingsplanGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSupport.KC.OpleidingsplanGenerator.Dal.Mappers
{
    public interface IManagementPropertiesDataMapper
    {
        void Update(ManagementProperties properties);
        ManagementProperties FindManagementProperties();
    }
}
