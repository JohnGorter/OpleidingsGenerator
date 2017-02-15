using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSupport.KC.OpleidingsplanGenerator.Dal
{
    public class DalConfiguration : ConfigurationSection
    {
        public static DalConfiguration Configuration
        {
            get
            {
                DalConfiguration configuration =
                    ConfigurationManager
                    .GetSection("DalJsonConnection")
                    as DalConfiguration;

                if (configuration != null)
                {
                    return configuration;
                }

                return new DalConfiguration();
            }
        }

        [ConfigurationProperty("profile-path", IsRequired = true)]
        public string ProfilePath
        {
            get
            {
                return this["profile-path"] as string;
            }
        }

        [ConfigurationProperty("educationplan-path", IsRequired = true)]
        public string EducationPlanPath
        {
            get
            {
                return this["educationplan-path"] as string;
            }
        }

        [ConfigurationProperty("management-properties-path", IsRequired = true)]
        public string ManagementPropertiesPath
        {
            get
            {
                return this["management-properties-path"] as string;
            }
        }

        [ConfigurationProperty("educationplan-updated-path", IsRequired = true)]
        public string EducationPlanUpdatedPath
        {
            get
            {
                return this["educationplan-updated-path"] as string;
            }
        }

        [ConfigurationProperty("module-path", IsRequired = true)]
        public string ModulePath
        {
            get
            {
                return this["module-path"] as string;
            }
        }
    }
}
