﻿using InfoSupport.KC.OpleidingsplanGenerator.Dal.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfoSupport.KC.OpleidingsplanGenerator.Models;
using Newtonsoft.Json;
using System.IO;
using log4net;
using System.Globalization;

namespace InfoSupport.KC.OpleidingsplanGenerator.Dal.Mappers
{
    public class ManagementPropertiesJsonDataMapper : IManagementPropertiesDataMapper
    {
        private readonly string _path;
        private static ILog _logger = LogManager.GetLogger(typeof(ManagementPropertiesJsonDataMapper));

        public ManagementPropertiesJsonDataMapper(string path)
        {
            _path = path;
        }

        public ManagementProperties FindManagementProperties()
        {
            _logger.Debug("Find management properties");
            return GetProperties();
        }

        public void Update(ManagementProperties properties)
        {
            _logger.Debug("Update management properties");
            WriteAllPropertiesToFile(properties);
        }

        private void WriteAllPropertiesToFile(ManagementProperties properties)
        {
            var convertedJson = JsonConvert.SerializeObject(properties, Formatting.Indented);

            if(File.Exists(_path))
            {
                File.WriteAllText(_path, convertedJson);
            }
            else
            {
                string errorMessage = "File to write properties not found";
                _logger.Error(errorMessage);
                throw new FileNotFoundException(errorMessage);
            }
        }

        private ManagementProperties GetProperties()
        {
            try
            {
                string properties = File.ReadAllText(_path);
                return JsonConvert.DeserializeObject<ManagementProperties>(properties);
            }
            catch (FileNotFoundException ex)
            {
                _logger.Error("File to get properties not found", ex);
                throw;
            }
            catch (JsonSerializationException ex)
            {
                _logger.Error("Couldn't deserialize properties", ex);
                throw;
            }
        }
    }
}