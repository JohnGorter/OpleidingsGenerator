using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.DAL
{
    public class Configuration : ConfigurationSection
    {
        public static Configuration GetConfiguration()
        {
            Configuration configuration =
                ConfigurationManager
                .GetSection("profileJsonConnection")
                as Configuration;

            if (configuration != null)
                return configuration;

            return new Configuration();
        }

        [ConfigurationProperty("profile-path", IsRequired = false)]
        public string ProfilePath
        {
            get
            {
                return this["profile-path"] as string;
            }
        }

        [ConfigurationProperty("educationplan-path", IsRequired = false)]
        public string EducationPlanPath
        {
            get
            {
                return this["educationplan-path"] as string;
            }
        }

        [ConfigurationProperty("educationplan-updated-path", IsRequired = false)]
        public string EducationPlanUpdatedPath
        {
            get
            {
                return this["educationplan-updated-path"] as string;
            }
        }
    }
}
