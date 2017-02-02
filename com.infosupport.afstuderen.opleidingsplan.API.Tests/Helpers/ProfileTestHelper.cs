using InfoSupport.KC.OpleidingsplanGenerator.Api.Models;
using InfoSupport.KC.OpleidingsplanGenerator.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InfoSupport.KC.OpleidingsplanGenerator.Api.Tests.Helpers
{
    public class ProfileTestHelper
    {
        protected List<CourseProfile> GetDummyDataProfiles()
        {
            var profiles = JsonConvert.DeserializeObject<List<CourseProfile>>(File.ReadAllText("../../Helpers/Profiles.json"));
            return profiles;
        }
    }
}