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
    public class CourseJSONDataMapper : ICourseDataMapper
    {
        private string _path;
        private readonly CultureInfo _culture = new CultureInfo("nl-NL");

        public CourseJSONDataMapper(string path)
        {
            _path = path;
        }

        public void Delete(CoursePriority data)
        {
            if (data == null) { throw new ArgumentNullException("data"); }

            var courses = GetAllCoursesFromProfile(data.ProfileId);

            if(courses == null)
            {
                throw new ArgumentException(string.Format(_culture, "No profile found with id {0}", data.ProfileId));
            }

            CoursePriority courseToDelete = courses.FirstOrDefault(c => c.Id == data.Id);

            if (courseToDelete == null)
            {
                throw new ArgumentException(string.Format(_culture, "No course found with code {0} in profile {1}", data.Code, data.ProfileId));
            }

            courses.Remove(courseToDelete);

            var profiles = GetAllProfiles();
            profiles.FirstOrDefault(profile => profile.Id == data.ProfileId).Courses = courses;

            WriteAllProfilesToFile(profiles);
        }


        public List<CoursePriority> FindCoursesByProfile(int profileId)
        {
            return GetAllCoursesFromProfile(profileId);
        }

        public void Update(CoursePriority data)
        {
            if (data == null) { throw new ArgumentNullException("data"); }

            var courses = GetAllCoursesFromProfile(data.ProfileId);
            CoursePriority courseToUpdate = courses.FirstOrDefault(p => p.Id == data.Id);

            if (courseToUpdate == null)
            {
                throw new ArgumentException(string.Format(_culture, "No course found with id {0}", data.Id));
            }

            int index = courses.IndexOf(courseToUpdate);
            courses[index] = data;

            var profiles = GetAllProfiles();
            profiles.FirstOrDefault(profile => profile.Id == data.ProfileId).Courses = courses;

            WriteAllProfilesToFile(profiles);
        }

        public void Insert(CoursePriority data)
        {
            if (data == null) { throw new ArgumentNullException("data"); }

            var courses = GetAllCoursesFromProfile(data.ProfileId);
            var profiles = GetAllProfiles();
            data.Id = GenerateId(profiles);

            if (courses.Any(c => c.Code == data.Code))
            {
                throw new ArgumentException(string.Format(_culture, "Course with the code {0} already exists", data.Code));
            }

            courses.Add(data);
            profiles.FirstOrDefault(profile => profile.Id == data.ProfileId).Courses = courses;

            WriteAllProfilesToFile(profiles);
        }

        

        private int GenerateId(List<CourseProfile> allProfiles)
        {
            int newId = allProfiles.SelectMany(profile => profile.Courses).Max(profile => profile.Id) + 1;
            return newId;
        }

        private void WriteAllProfilesToFile(List<CourseProfile> profiles)
        {
            var convertedJson = JsonConvert.SerializeObject(profiles, Formatting.Indented);
            File.WriteAllText(_path, convertedJson);
        }


        private List<CoursePriority> GetAllCoursesFromProfile(int profileID)
        {
            var profiles = GetAllProfiles();
            return profiles.FirstOrDefault(profile => profile.Id == profileID)?.Courses?.ToList();
        }

        private List<CourseProfile> GetAllProfiles()
        {
            string profiles = File.ReadAllText(_path);
            return JsonConvert.DeserializeObject<List<CourseProfile>>(profiles);
        }
    }
}
