﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.integration
{
    public class IntegrationConfiguration : ConfigurationSection
    {
        public static IntegrationConfiguration Configuration
        {
            get
            {
                IntegrationConfiguration configuration =
                    ConfigurationManager
                    .GetSection("serviceConnection")
                    as IntegrationConfiguration;

                if (configuration != null)
                {
                    return configuration;
                }

                return new IntegrationConfiguration();
            }
        }

        [ConfigurationProperty("info-support-training-url", IsRequired = true)]
        public string InfoSupportTrainingURL
        {
            get
            {
                return this["info-support-training-url"] as string;
            }
        }
    }
}
