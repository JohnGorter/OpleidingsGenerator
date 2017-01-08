using com.infosupport.afstuderen.opleidingsplan.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.dal.mappers
{
    public interface IManagementPropertiesDataMapper
    {
        void Update(ManagementProperties properties);
        ManagementProperties FindManagementProperties();
    }
}
