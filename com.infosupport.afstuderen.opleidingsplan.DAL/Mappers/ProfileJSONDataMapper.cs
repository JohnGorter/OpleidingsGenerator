using com.infosupport.afstuderen.opleidingsplan.model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.DAL.mapper
{
    public class ProfileJSONDataMapper : IDataMapper<Profile>
    {
        private string _path;

        public ProfileJSONDataMapper(string path)
        {
            _path = path;
        }
        public Profile FindById(long id)
        {
            Profile foundProfile = GetAllProfiles().FirstOrDefault(profile => profile.Id == id);

            if(foundProfile == null)
            {
                throw new ArgumentException(string.Format("No profile found with id {0}", id));
            }

            return foundProfile;
        }

        public void Delete(Profile profile)
        {
            var profiles = GetAllProfiles();
            Profile profileToDelete = profiles.FirstOrDefault(p => p.Id == profile.Id);

            if (profileToDelete == null)
            {
                throw new ArgumentException(string.Format("No profile found with id {0}", profile.Id));
            }

            profiles.Remove(profileToDelete);

            WriteAllProfilesToFile(profiles);
        }

        public IEnumerable<Profile> Find(Func<Profile, bool> predicate)
        {
            return GetAllProfiles().Where(predicate);
        }

        public IEnumerable<Profile> FindAll()
        {
            return GetAllProfiles();
        }

        public void Insert(Profile profile)
        {
            var profiles = GetAllProfiles();
            profile.Id = GenerateId(profiles);

            if (profiles.Any(p => p.Name == profile.Name))
            {
                throw new ArgumentException(string.Format("Profile with the name {0} already exists", profile.Name));
            }

            profiles.Add(profile);
            WriteAllProfilesToFile(profiles);
        }

        public void Update(Profile profile)
        {
            var profiles = GetAllProfiles();
            Profile profileToUpdate = profiles.FirstOrDefault(p => p.Id == profile.Id);

            if(profileToUpdate == null)
            {
                throw new ArgumentException(string.Format("No profile found with id {0}", profile.Id));
            }

            profile.Courses = profileToUpdate.Courses;

            int index = profiles.IndexOf(profileToUpdate);
            profiles[index] = profile;

            WriteAllProfilesToFile(profiles);
        }

        private int GenerateId(List<Profile> allProfiles)
        {
            int newId = allProfiles.Max(profile => profile.Id) + 1;
            return newId;
        }

        private void WriteAllProfilesToFile(List<Profile> profiles)
        {
            var convertedJson = JsonConvert.SerializeObject(profiles, Formatting.Indented);
            File.WriteAllText(_path, convertedJson);
        }

        private List<Profile> GetAllProfiles()
        {
            string profiles = File.ReadAllText(_path);
            return JsonConvert.DeserializeObject<List<Profile>>(profiles);
        }
    }
}
