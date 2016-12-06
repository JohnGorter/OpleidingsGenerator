using com.infosupport.afstuderen.opleidingsplan.api.models;
using com.infosupport.afstuderen.opleidingsplan.models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace com.infosupport.afstuderen.opleidingsplan.api.tests.helpers
{
    public class AdministrationTestHelper
    {
        protected List<CourseProfile> GetDummyDataProfiles()
        {
            var profiles = JsonConvert.DeserializeObject<List<CourseProfile>>(File.ReadAllText("../../Helpers/Profiles.json"));
            return profiles;
        }
    }
}