using com.infosupport.afstuderen.opleidingsplan.dal.mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.infosupport.afstuderen.opleidingsplan.models;
using Newtonsoft.Json;
using System.IO;

namespace com.infosupport.afstuderen.opleidingsplan.dal.mappers
{
    public class ManagementPropertiesJSONDataMapper : IManagementPropertiesDataMapper
    {
        private string _path;

        public ManagementPropertiesJSONDataMapper(string path)
        {
            _path = path;
        }

        public ManagementProperties FindManagementProperties()
        {
            return GetProperties();
        }

        public void Update(ManagementProperties properties)
        {
            WriteAllPropertiesToFile(properties);
        }

        private void WriteAllPropertiesToFile(ManagementProperties properties)
        {
            var convertedJson = JsonConvert.SerializeObject(properties, Formatting.Indented);
            File.WriteAllText(_path, convertedJson);
        }

        private ManagementProperties GetProperties()
        {
            string properties = File.ReadAllText(_path);
            return JsonConvert.DeserializeObject<ManagementProperties>(properties);
        }
    }
}
