using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.dal
{
    public class DALConfiguration : ConfigurationSection
    {
        public static DALConfiguration Configuration
        {
            get
            {
                DALConfiguration configuration =
                    ConfigurationManager
                    .GetSection("DalJsonConnection")
                    as DALConfiguration;

                if (configuration != null)
                    return configuration;

                return new DALConfiguration();
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
    }
}
