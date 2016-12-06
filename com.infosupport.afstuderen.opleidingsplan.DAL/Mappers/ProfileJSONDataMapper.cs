using com.infosupport.afstuderen.opleidingsplan.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.dal.mappers
{
    public class ProfileJsonDataMapper : IDataMapper<CourseProfile>
    {
        private string _path;
        private readonly CultureInfo _culture = new CultureInfo("nl-NL");

        public ProfileJsonDataMapper(string path)
        {
            _path = path;
        }
        public CourseProfile FindById(long id)
        {
            CourseProfile foundProfile = GetAllProfiles().FirstOrDefault(profile => profile.Id == id);

            if(foundProfile == null)
            {
                throw new ArgumentException(string.Format(_culture, "No profile found with id {0}", id));
            }

            return foundProfile;
        }

        public void Delete(CourseProfile data)
        {
            var profiles = GetAllProfiles();
            CourseProfile profileToDelete = profiles.FirstOrDefault(p => p.Id == data.Id);

            if (profileToDelete == null)
            {
                throw new ArgumentException(string.Format(_culture, "No profile found with id {0}", data.Id));
            }

            profiles.Remove(profileToDelete);

            WriteAllProfilesToFile(profiles);
        }

        public IEnumerable<CourseProfile> Find(Func<CourseProfile, bool> predicate)
        {
            return GetAllProfiles().Where(predicate);
        }

        public IEnumerable<CourseProfile> FindAll()
        {
            return GetAllProfiles();
        }

        public void Insert(CourseProfile data)
        {
            var profiles = GetAllProfiles();
            data.Id = GenerateId(profiles);

            if (profiles.Any(p => p.Name == data.Name))
            {
                throw new ArgumentException(string.Format(_culture, "Profile with the name {0} already exists", data.Name));
            }

            profiles.Add(data);
            WriteAllProfilesToFile(profiles);
        }

        public void Update(CourseProfile data)
        {
            var profiles = GetAllProfiles();
            CourseProfile profileToUpdate = profiles.FirstOrDefault(p => p.Id == data.Id);

            if(profileToUpdate == null)
            {
                throw new ArgumentException(string.Format(_culture, "No profile found with id {0}", data.Id));
            }

            data.Courses = profileToUpdate.Courses;

            int index = profiles.IndexOf(profileToUpdate);
            profiles[index] = data;

            WriteAllProfilesToFile(profiles);
        }

        private int GenerateId(List<CourseProfile> allProfiles)
        {
            int newId = allProfiles.Max(profile => profile.Id) + 1;
            return newId;
        }

        private void WriteAllProfilesToFile(List<CourseProfile> profiles)
        {
            var convertedJson = JsonConvert.SerializeObject(profiles, Formatting.Indented);
            File.WriteAllText(_path, convertedJson);
        }

        private List<CourseProfile> GetAllProfiles()
        {
            string profiles = File.ReadAllText(_path);
            return JsonConvert.DeserializeObject<List<CourseProfile>>(profiles);
        }
    }
}
