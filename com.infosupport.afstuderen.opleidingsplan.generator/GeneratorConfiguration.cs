using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSupport.KC.OpleidingsplanGenerator.Generator
{
    public class GeneratorConfiguration : ConfigurationSection
    {
        public static GeneratorConfiguration Configuration
        {
            get
            {
                GeneratorConfiguration configuration =
                    ConfigurationManager
                    .GetSection("generatorConfigurations")
                    as GeneratorConfiguration;

                if (configuration != null)
                {
                    return configuration;
                }

                return new GeneratorConfiguration();
            }
        }

        [ConfigurationProperty("education-plan-file-dir-path", IsRequired = true)]
        public string EducationPlanFileDirPath
        {
            get
            {
                return this["education-plan-file-dir-path"] as string;
            }
        }
    }
    }
