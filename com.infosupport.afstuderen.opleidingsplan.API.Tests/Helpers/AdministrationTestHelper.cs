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
        protected List<Profile> GetDummyDataProfiles()
        {
            var profiles = JsonConvert.DeserializeObject<List<Profile>>(File.ReadAllText("../../Helpers/Profiles.json"));
            return profiles;
        }
    }
}