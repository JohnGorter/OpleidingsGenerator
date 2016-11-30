using com.infosupport.afstuderen.opleidingsplan.api.Models;
using com.infosupport.afstuderen.opleidingsplan.DAL.mapper;
using com.infosupport.afstuderen.opleidingsplan.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.infosupport.afstuderen.opleidingsplan.api.Managers
{
    public class AdministrationManager : IAdministrationManager
    {
        private IDataMapper<Profile> _profileDataMapper;

        public AdministrationManager(string pathToProfiles)
        {
            _profileDataMapper = new ProfileDataMapper(pathToProfiles);
        }

        public AdministrationManager(IDataMapper<Profile> profileDataMapper)
        {
            _profileDataMapper = profileDataMapper;
        }

        public Profile FindProfile(string profileName)
        {
            return _profileDataMapper.Find(profile => profile.Name == profileName).First();
        }

        public IEnumerable<Profile> FindProfiles()
        {
            return _profileDataMapper.FindAll();
        }
    }
}