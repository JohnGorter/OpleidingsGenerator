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
                return (int)this["period-educationplan-days"];
            }
        }

        [ConfigurationProperty("period-after-last-course-employable", IsRequired = true)]
        public int PeriodAfterLastCourseEmployable
        {
            get
            {
                return (int)this["period-after-last-course-employable"];
            }
        }

        [ConfigurationProperty("period-before-start-days", IsRequired = true)]
        public int PeriodeBeforeStartDays
        {
            get
            {
                return (int)this["period-before-start-days"];
            }
        }
    }
}
