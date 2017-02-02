using InfoSupport.KC.OpleidingsplanGenerator.Models;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSupport.KC.OpleidingsplanGenerator.Dal.Mappers
{
    public class ProfileJsonDataMapper : IDataMapper<CourseProfile>
    {
        private readonly string _path;
        private readonly CultureInfo _culture = new CultureInfo("nl-NL");
        private static ILog _logger = LogManager.GetLogger(typeof(ProfileJsonDataMapper));

        public ProfileJsonDataMapper(string path)
        {
            _path = path;
        }
        public CourseProfile FindById(long id)
        {
            _logger.Debug(string.Format(_culture, "Find profile by id {0} ", id));
            CourseProfile foundProfile = GetAllProfiles().FirstOrDefault(profile => profile.Id == id);

            if(foundProfile == null)
            {
                string errorMessage = string.Format(_culture, "No profile found with id {0}", id);
                _logger.Error("ArgumentException: " + errorMessage);
                throw new ArgumentException(errorMessage);
            }

            _logger.Debug(string.Format(_culture, "Profile found by id {0} with name {1}", id, foundProfile.Name));
            return foundProfile;
        }

        public void Delete(long id)
        {
            _logger.Debug(string.Format(_culture, "Delete profile with id {0}", id));
  
            var profiles = GetAllProfiles();
            CourseProfile profileToDelete = profiles.FirstOrDefault(p => p.Id == id);

            if (profileToDelete == null)
            {
                string errorMessage = string.Format(_culture, "No profile found with id {0}", id);
                _logger.Error("ArgumentException: " + errorMessage);
                throw new ArgumentException(errorMessage);
            }

            profiles.Remove(profileToDelete);

            WriteAllProfilesToFile(profiles);

            _logger.Debug(string.Format(_culture, "Profile deleted with id {0} with name {1}", profileToDelete.Id, profileToDelete.Name));

        }

        public IEnumerable<CourseProfile> Find(Func<CourseProfile, bool> predicate)
        {
            _logger.Debug("Find profile");
            return GetAllProfiles().Where(predicate);
        }

        public IEnumerable<CourseProfile> FindAll()
        {
            _logger.Debug("Find all profiles");
            return GetAllProfiles();
        }

        public void Insert(CourseProfile courseProfile)
        {
            if (courseProfile == null)
            {
                _logger.Error("ArgumentNullException courseProfile");
                throw new ArgumentNullException("courseProfile");
            }

            _logger.Debug(string.Format(_culture, "Insert profile with name {0}", courseProfile.Name));
            var profiles = GetAllProfiles();
            courseProfile.Id = GenerateId(profiles);

            if (profiles.Any(p => p.Name == courseProfile.Name))
            {
                string errorMessage = string.Format(_culture, "Profile with the name {0} already exists", courseProfile.Name);
                _logger.Error("ArgumentException: " + errorMessage);
                throw new ArgumentException(errorMessage);
            }

            profiles.Add(courseProfile);
            WriteAllProfilesToFile(profiles);
            _logger.Debug(string.Format(_culture, "Profile with name {0} inserted with generated id {1}", courseProfile.Name, courseProfile.Id));
        }

        public void Update(CourseProfile courseProfile)
        {
            if (courseProfile == null)
            {
                _logger.Error("ArgumentNullException courseProfile");
                throw new ArgumentNullException("courseProfile");
            }

            var profiles = GetAllProfiles();
            CourseProfile profileToUpdate = profiles.FirstOrDefault(p => p.Id == courseProfile.Id);

            if(profileToUpdate == null)
            {
                string errorMessage = string.Format(_culture, "No profile found with id {0}", courseProfile.Id);
                _logger.Error("ArgumentException: " + errorMessage);
                throw new ArgumentException(errorMessage);
            }

            _logger.Debug(string.Format(_culture, "Update profile with id {0} from name {1} to name {2}", courseProfile.Id, profileToUpdate.Name, courseProfile.Name));

            courseProfile.Courses = profileToUpdate.Courses;

            int index = profiles.IndexOf(profileToUpdate);
            profiles[index] = courseProfile;

            WriteAllProfilesToFile(profiles);
            _logger.Debug(string.Format(_culture, "Profile updated with id {0} and name {1}", courseProfile.Id, courseProfile.Name));

        }

        private long GenerateId(List<CourseProfile> allProfiles)
        {
            _logger.Debug("Generate id for profile");

            long newId = 1;
            if (allProfiles.Any())
            {
                newId = allProfiles.Max(profile => profile.Id) + 1;
            }

            _logger.Debug(string.Format(_culture, "Generated id for profile {0}", newId));

            return newId;
        }

        private void WriteAllProfilesToFile(List<CourseProfile> profiles)
        {
            var convertedJson = JsonConvert.SerializeObject(profiles, Formatting.Indented);
            try
            {
                File.WriteAllText(_path, convertedJson);
                _logger.Debug(string.Format(_culture, "Saved all profiles to file with path {0}", _path));
            }
            catch (FileNotFoundException ex)
            {
                _logger.Error(string.Format(_culture, "File {0} to write all profiles not found", _path), ex);
                throw;
            }
        }

        private List<CourseProfile> GetAllProfiles()
        {
            try
            {
                string profiles = File.ReadAllText(_path);
                _logger.Debug(string.Format(_culture, "Get all profiles from file with path {0}", _path));
                return JsonConvert.DeserializeObject<List<CourseProfile>>(profiles);
            }
            catch (FileNotFoundException ex)
            {
                _logger.Error(string.Format(_culture, "File {0} to get all profiles not found", _path), ex);
                throw;
            }
            catch(JsonReaderException ex)
            {
                _logger.Error(string.Format(_culture, "Couldn't deserialize profiles from path {0}", _path), ex);
                throw;
            }
        }
    }
}
