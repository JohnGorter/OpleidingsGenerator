using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InfoSupport.KC.OpleidingsplanGenerator.Models;
using InfoSupport.KC.OpleidingsplanGenerator.Dal.Mappers;

namespace InfoSupport.KC.OpleidingsplanGenerator.Api.Managers
{
    public class ManagementPropertiesManager : IManagementPropertiesManager
    {
        private readonly IManagementPropertiesDataMapper _managementPropertiesDataMapper;

        public ManagementPropertiesManager(string managementPropertiesPath)
        {
            _managementPropertiesDataMapper = new ManagementPropertiesJsonDataMapper(managementPropertiesPath);
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