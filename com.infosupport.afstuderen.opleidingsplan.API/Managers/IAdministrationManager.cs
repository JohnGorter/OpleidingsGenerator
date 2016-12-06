using com.infosupport.afstuderen.opleidingsplan.api.models;
using com.infosupport.afstuderen.opleidingsplan.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.api.managers
{
    public interface IAdministrationManager
    {
        IEnumerable<CourseProfile> FindProfiles();
        CourseProfile FindProfile(string profileName);
        CourseProfile FindProfileById(int id);
    }
}
