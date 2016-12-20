using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public class GeneratorConfiguration : ConfigurationSection
    {
        public static GeneratorConfiguration GetConfiguration()
        {
            GeneratorConfiguration configuration =
                ConfigurationManager
                .GetSection("generatorSettings")
                as GeneratorConfiguration;

            if (configuration != null)
                return configuration;

            return new GeneratorConfiguration();
        }

        [ConfigurationProperty("period-educationplan-days", IsRequired = true)]
        public int PeriodEducationPlanInDays
        {
            get
            {
                return (int) this["period-educationplan-days"];
            }
        }
    }
}
