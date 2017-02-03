using InfoSupport.KC.OpleidingsplanGenerator.Api.Models;
using InfoSupport.KC.OpleidingsplanGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSupport.KC.OpleidingsplanGenerator.Api.Managers
{
    public interface IProfileManager
    {
        IEnumerable<CourseProfile> FindProfiles();
        CourseProfile FindProfile(string profileName);
        CourseProfile FindProfileById(long id);
        void Insert(CourseProfile profile);
        void Update(CourseProfile profile);
        void Delete(long id);
    }
}
