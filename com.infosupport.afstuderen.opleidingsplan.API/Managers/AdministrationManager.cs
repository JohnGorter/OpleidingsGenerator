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
        private readonly IDataMapper<CourseProfile> _profileDataMapper;

        public AdministrationManager(string pathToProfiles)
        {
            _profileDataMapper = new ProfileJsonDataMapper(pathToProfiles);
        }

        public AdministrationManager(IDataMapper<CourseProfile> profileDataMapper)
        {
            _profileDataMapper = profileDataMapper;
        }

        public CourseProfile FindProfile(string profileName)
        {
            return _profileDataMapper.Find(profile => profile.Name == profileName).First();
        }

        public CourseProfile FindProfileById(int id)
        {
            return _profileDataMapper.FindById(id);
        }

        public IEnumerable<CourseProfile> FindProfiles()
        {
            return _profileDataMapper.FindAll();
        }
    }
}