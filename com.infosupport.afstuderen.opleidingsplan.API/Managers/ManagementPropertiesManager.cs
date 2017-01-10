using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.infosupport.afstuderen.opleidingsplan.models;
using com.infosupport.afstuderen.opleidingsplan.dal.mappers;

namespace com.infosupport.afstuderen.opleidingsplan.api.managers
{
    public class ManagementPropertiesManager : IManagementPropertiesManager
    {
        private readonly IManagementPropertiesDataMapper _managementPropertiesDataMapper;

        public ManagementPropertiesManager(string managementPropertiesPath)
        {
            _managementPropertiesDataMapper = new ManagementPropertiesJSONDataMapper(managementPropertiesPath);
        }

        public ManagementPropertiesManager(IManagementPropertiesDataMapper managementPropertiesDataMapper)
        {
            _managementPropertiesDataMapper = managementPropertiesDataMapper;
        }

        public ManagementProperties FindManagementProperties()
        {
            return _managementPropertiesDataMapper.FindManagementProperties();
        }

        public void Update(ManagementProperties properties)
        {
            _managementPropertiesDataMapper.Update(properties);
        }
    }
}