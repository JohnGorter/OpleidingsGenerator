using InfoSupport.KC.OpleidingsplanGenerator.Api.Models;
using InfoSupport.KC.OpleidingsplanGenerator.Dal.Mappers;
using InfoSupport.KC.OpleidingsplanGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InfoSupport.KC.OpleidingsplanGenerator.Api.Managers
{
    public class ProfileManager : IProfileManager
    {
        private readonly IDataMapper<CourseProfile> _profileDataMapper;

        public ProfileManager(string pathToProfiles)
        {
            _profileDataMapper = new ProfileJsonDataMapper(pathToProfiles);
        }

        public ProfileManager(IDataMapper<CourseProfile> profileDataMapper)
        {
            _profileDataMapper = profileDataMapper;
        }

        public CourseProfile FindProfile(string profileName)
        {
            return _profileDataMapper.Find(profile => profile.Name == profileName).First();
        }

        public CourseProfile FindProfileById(long id)
        {
            return _profileDataMapper.FindById(id);
        }

        public IEnumerable<CourseProfile> FindProfiles()
        {
            return _profileDataMapper.FindAll();
        }

        public void Insert(CourseProfile profile)
        {
            _profileDataMapper.Insert(profile);
        }

        public void Update(CourseProfile profile)
        {
            _profileDataMapper.Update(profile);
        }

        public void Delete(long id)
        {
            _profileDataMapper.Delete(id);
        }
    }
}