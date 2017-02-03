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
    public class CourseJsonDataMapper : ICourseDataMapper
    {
        private readonly string _path;
        private readonly CultureInfo _culture = new CultureInfo("nl-NL");
        private static ILog _logger = LogManager.GetLogger(typeof(CourseJsonDataMapper));

        public CourseJsonDataMapper(string path)
        {
            _path = path;
        }

        public void Delete(CoursePriority coursePriority)
        {

            if (coursePriority == null)
            {
                _logger.Error("ArgumentNullException coursePriority");
                throw new ArgumentNullException("coursePriority");
            }
            _logger.Debug(string.Format(_culture, "Delete course with id {0}, code {1} and profile id {2}", coursePriority.Id, coursePriority.Code, coursePriority.ProfileId));

            var courses = GetAllCoursesFromProfile(coursePriority.ProfileId);

            if(courses == null)
            {
                string errorMessage = string.Format(_culture, "No profile found with id {0}", coursePriority.ProfileId);
                _logger.Error("ArgumentException: " + errorMessage);
                throw new ArgumentException(errorMessage);
            }

            CoursePriority courseToDelete = courses.FirstOrDefault(c => c.Id == coursePriority.Id);

            if (courseToDelete == null)
            {
                string errorMessage = string.Format(_culture, "No course found with code {0} in profile {1}", coursePriority.Code, coursePriority.ProfileId);
                _logger.Error("ArgumentException: " + errorMessage);
                throw new ArgumentException(errorMessage);
            }

            courses.Remove(courseToDelete);

            var profiles = GetAllProfiles();
            profiles.FirstOrDefault(profile => profile.Id == coursePriority.ProfileId).Courses = courses;

            WriteAllProfilesToFile(profiles);
            _logger.Debug(string.Format(_culture, "Course deleted with id {0}, code {1} and profile id {2}", courseToDelete.Id, courseToDelete.Code, courseToDelete.ProfileId));
        }


        public void Update(CoursePriority coursePriority)
        {
            if (coursePriority == null)
            {
                _logger.Error("ArgumentNullException coursePriority");
                throw new ArgumentNullException("coursePriority");
            }

            _logger.Debug(string.Format(_culture, "Update course with id {0}, code {1} and profile id {2}", coursePriority.Id, coursePriority.Code, coursePriority.ProfileId));

            var courses = GetAllCoursesFromProfile(coursePriority.ProfileId);
            CoursePriority courseToUpdate = courses.FirstOrDefault(p => p.Id == coursePriority.Id);

            if (courseToUpdate == null)
            {
                string errorMessage = string.Format(_culture, "No course found with id {0}", coursePriority.Id);
                _logger.Error("ArgumentException: " + errorMessage);
                throw new ArgumentException(errorMessage);
            }

            int index = courses.IndexOf(courseToUpdate);
            courses[index] = coursePriority;

            var profiles = GetAllProfiles();
            profiles.FirstOrDefault(profile => profile.Id == coursePriority.ProfileId).Courses = courses;

            WriteAllProfilesToFile(profiles);
            _logger.Debug(string.Format(_culture, "Course updated with id {0}, code {1} and profile id {2}", courseToUpdate.Id, courseToUpdate.Code, courseToUpdate.ProfileId));
        }

        public void Insert(CoursePriority coursePriority)
        {
            if (coursePriority == null)
            {
                _logger.Error("ArgumentNullException coursePriority");
                throw new ArgumentNullException("coursePriority");
            }
            _logger.Debug(string.Format(_culture, "Insert course with code {0} and profile id {1}", coursePriority.Code, coursePriority.ProfileId));

            var courses = GetAllCoursesFromProfile(coursePriority.ProfileId);
            var profiles = GetAllProfiles();
            coursePriority.Id = GenerateId(profiles);

            if (courses.Any(c => c.Code == coursePriority.Code))
            {
                string errorMessage = string.Format(_culture, "Course with the code {0} already exists", coursePriority.Code);
                _logger.Error("ArgumentException: " + errorMessage);
                throw new ArgumentException(errorMessage);
            }

            courses.Add(coursePriority);
            profiles.FirstOrDefault(profile => profile.Id == coursePriority.ProfileId).Courses = courses;

            WriteAllProfilesToFile(profiles);
            _logger.Debug(string.Format(_culture, "Course inserted with code {0} and profile id {1} and generated id {2}", coursePriority.Code, coursePriority.ProfileId, coursePriority.Id));
        }

        private long GenerateId(List<CourseProfile> allProfiles)
        {
            _logger.Debug("Generate id for course");

            long newId = 1;

            if (allProfiles.Any())
            {
                newId = allProfiles.SelectMany(profile => profile.Courses).Max(profile => profile.Id) + 1;
            }

            _logger.Debug(string.Format(_culture, "Generated id for course {0}", newId));
            return newId;
        }

        private void WriteAllProfilesToFile(List<CourseProfile> profiles)
        {
            var convertedJson = JsonConvert.SerializeObject(profiles, Formatting.Indented);
            try
            {
                File.WriteAllText(_path, convertedJson);
                _logger.Debug(string.Format(_culture, "Saved all profiles to path {0}", _path));
            }
            catch (FileNotFoundException ex)
            {
                _logger.Error(string.Format(_culture, "File {0} to write all profiles not found", _path), ex);
                throw;
            }
        }

        private List<CoursePriority> GetAllCoursesFromProfile(int profileID)
        {
            _logger.Debug(string.Format(_culture, "Get all courses from profile with id {0}", profileID));
            var profiles = GetAllProfiles();
            _logger.Debug(string.Format(_culture, "Get all courses from profile with id {0}, {1} profile(s) found", profileID, profiles.Count));
            return profiles.FirstOrDefault(profile => profile.Id == profileID)?.Courses?.ToList();
        }

        private List<CourseProfile> GetAllProfiles()
        {
            try
            {
                string profiles = File.ReadAllText(_path);
                _logger.Debug(string.Format(_culture, "Get all profiles with path {0}", _path));
                return JsonConvert.DeserializeObject<List<CourseProfile>>(profiles);
            }
            catch (FileNotFoundException ex)
            {
                _logger.Error(string.Format(_culture, "File {0} to get all profiles not found", _path), ex);
                throw;
            }
            catch (JsonReaderException ex)
            {
                _logger.Error(string.Format(_culture, "Couldn't deserialize profiles from path {0}", _path), ex);
                throw;
            }
        }
    }
}
