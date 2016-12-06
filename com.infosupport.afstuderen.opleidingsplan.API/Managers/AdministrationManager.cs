using com.infosupport.afstuderen.opleidingsplan.api.models;
using com.infosupport.afstuderen.opleidingsplan.dal.mappers;
using com.infosupport.afstuderen.opleidingsplan.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.infosupport.afstuderen.opleidingsplan.api.managers
{
    public class AdministrationManager : IAdministrationManager
    {
        private IDataMapper<Profile> _profileDataMapper;

        public AdministrationManager(string pathToProfiles)
        {
            _profileDataMapper = new ProfileJSONDataMapper(pathToProfiles);
        }

        public AdministrationManager(IDataMapper<Profile> profileDataMapper)
        {
            _profileDataMapper = profileDataMapper;
        }

        public Profile FindProfile(string profileName)
        {
            return _profileDataMapper.Find(profile => profile.Name == profileName).First();
        }

        public Profile FindProfileById(int id)
        {
            return _profileDataMapper.FindById(id);
        }

        public IEnumerable<Profile> FindProfiles()
        {
            return _profileDataMapper.FindAll();
        }
    }
}