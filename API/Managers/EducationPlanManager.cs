using AutoMapper;
using InfoSupport.KC.OpleidingsplanGenerator.Api.Models;
using InfoSupport.KC.OpleidingsplanGenerator.Dal.Mappers;
using InfoSupport.KC.OpleidingsplanGenerator.Generator;
using InfoSupport.KC.OpleidingsplanGenerator.Integration;
using InfoSupport.KC.OpleidingsplanGenerator.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;

namespace InfoSupport.KC.OpleidingsplanGenerator.Api.Managers
{
    public class EducationPlanManager : IEducationPlanManager
    {
        private readonly ICourseService _courseService;
        private readonly IPlanner _planner;
        private readonly IEducationPlanOutputter _educationPlanOutputter;
        private readonly IDataMapper<OpleidingsplanGenerator.Models.CourseProfile> _profileDataMapper;
        private readonly IEducationPlanDataMapper _educationPlanDataMapper;
        private readonly IEducationPlanConverter _educationPlanConverter;

        private static ILog _logger = LogManager.GetLogger(typeof(EducationPlanManager));
        private static CultureInfo _culture = new CultureInfo("nl-NL");

        public EducationPlanManager(string profilePath, ICourseService courseService, string managementPropertiesPath, string educationPlanPath, string educationPlanUpdatedPath)
        {
            _courseService = courseService;
            IManagementPropertiesDataMapper managementPropertiesDataMapper = new ManagementPropertiesJsonDataMapper(managementPropertiesPath);
            _planner = new Planner(managementPropertiesDataMapper);
            _educationPlanOutputter = new EducationPlanOutputter(_planner, managementPropertiesDataMapper);
            _profileDataMapper = new ProfileJsonDataMapper(profilePath);
            _educationPlanDataMapper = new EducationPlanJsonDataMapper(educationPlanPath, educationPlanUpdatedPath);
        }

        public EducationPlanManager(string profilePath, string managementPropertiesPath, string educationPlanPath, string educationPlanUpdatedPath, string educationPlanFilePath)
        {
            _courseService = new CourseService();
            IManagementPropertiesDataMapper managementPropertiesDataMapper = new ManagementPropertiesJsonDataMapper(managementPropertiesPath);
            _planner = new Planner(managementPropertiesDataMapper);
            _educationPlanOutputter = new EducationPlanOutputter(_planner, managementPropertiesDataMapper);
            _profileDataMapper = new ProfileJsonDataMapper(profilePath);
            _educationPlanDataMapper = new EducationPlanJsonDataMapper(educationPlanPath, educationPlanUpdatedPath);
            _educationPlanConverter = new EducationPlanConverter(managementPropertiesPath, educationPlanFilePath);
        }

        public EducationPlanManager(ICourseService courseService, IPlanner planner, IEducationPlanOutputter educationPlanOutputter, 
            IDataMapper<OpleidingsplanGenerator.Models.CourseProfile> profileDataMapper, IEducationPlanDataMapper educationPlanDataMapper, 
            IEducationPlanConverter educationPlanConverter)
        {
            _courseService = courseService;
            _planner = planner;
            _educationPlanOutputter = educationPlanOutputter;
            _profileDataMapper = profileDataMapper;
            _educationPlanDataMapper = educationPlanDataMapper;
            _educationPlanConverter = educationPlanConverter;
        }

        private static List<OpleidingsplanGenerator.Models.Course> ConvertCourses(IEnumerable<Integration.Course> courses, CourseProfile profile, Collection<RestEducationPlanCourse> restCourses)
        {
            _logger.Debug("ConvertCourses");
            List<OpleidingsplanGenerator.Models.Course> coursesToPlan = new List<OpleidingsplanGenerator.Models.Course>();

            foreach (var course in courses)
            {
                OpleidingsplanGenerator.Models.Course courseToPlan = Mapper.Map<OpleidingsplanGenerator.Models.Course>(course);

                if (profile != null)
                {
                    CoursePriority coursePriority = profile.Courses.FirstOrDefault(profileCourse => profileCourse.Code == course.Code);
                    if (coursePriority != null)
                    {
                        courseToPlan.Priority = coursePriority.Priority;
                    }
                }

                RestEducationPlanCourse restCourse = restCourses.FirstOrDefault(profileCourse => profileCourse.Code == course.Code);
                if (restCourse != null && restCourse.Priority != 0)
                {
                    courseToPlan.Priority = restCourse.Priority;
                }

                coursesToPlan.Add(courseToPlan);
            }

            return coursesToPlan;
        }

        public EducationPlan GenerateEducationPlan(RestEducationPlan educationPlan)
        {
            if (educationPlan == null)
            {
                _logger.Error("ArgumentNullException: educationPlan");
                throw new ArgumentNullException("educationPlan");
            }

            _logger.Debug(string.Format(_culture, "GenerateEducationPlan for employee {0}", educationPlan.NameEmployee));

            _planner.StartDate = educationPlan.InPaymentFrom;
            _planner.BlockedDates = educationPlan.BlockedDates;

            var educationplanData = Mapper.Map<EducationPlanData>(educationPlan);
            CourseProfile profile = null;
            if (educationPlan.ProfileId != 0)
            {
                _logger.Debug(string.Format(_culture, "ProfileId exists: {0}", educationPlan.ProfileId));
                profile = _profileDataMapper.FindById(educationPlan.ProfileId);
                educationplanData.Profile = profile.Name;
                educationplanData.ProfileId = educationPlan.ProfileId;
            }

            var educationPlanCourses = educationPlan.Courses.Where(course => !course.Code.StartsWith("OLC"));

            _logger.Debug("Find courses from service");
            IEnumerable<Integration.Course> courses = _courseService.FindCourses(educationPlanCourses.Select(course => course.Code));
            List<OpleidingsplanGenerator.Models.Course> coursesToPlan = ConvertCourses(courses, profile, educationPlan.Courses);
            _planner.PlanCoursesWithOlc(coursesToPlan);
            OverrideRestCourse(_planner, educationPlan.Courses);

            return _educationPlanOutputter.GenerateEducationPlan(educationplanData);
        }

        private void OverrideRestCourse(IPlanner planner, Collection<RestEducationPlanCourse> restCourses)
        {
            foreach (var restCourse in restCourses)
            {
                var course = planner.AllCourses.FirstOrDefault(c => c.Code == restCourse.Code);

                if (course != null)
                {
                    if (restCourse.Priority != 0)
                    {
                        course.Priority = restCourse.Priority;
                    }
                    course.Commentary = restCourse.Commentary;
                }
            }
        }

        public long SaveEducationPlan(RestEducationPlan restEducationPlan)
        {
            var educationPlan = GenerateEducationPlan(restEducationPlan);
            return _educationPlanDataMapper.Insert(educationPlan);
        }
        public long UpdateEducationPlan(RestEducationPlan restEducationPlan)
        {

            //Get old educationplan
            //Get courses before today

            var educationPlan = GenerateEducationPlan(restEducationPlan);
            


            return _educationPlanDataMapper.Update(educationPlan);
        }

        public EducationPlan FindEducationPlan(long id)
        {
            return _educationPlanDataMapper.FindById(id);
        }

        public List<EducationPlan> FindEducationPlans(EducationPlanSearch search)
        {
            return _educationPlanDataMapper
                .Find(educationPlan => 
                educationPlan.NameEmployee != null && search.Name != null &&
                educationPlan.NameEmployee.ToLowerInvariant().Contains(search.Name.ToLowerInvariant()) || 
                search.Date.HasValue && educationPlan.Created.Date == search.Date).ToList();
        }

        public string GenerateWordFile(EducationPlan educationPlan)
        {
            return _educationPlanConverter.GenerateWord(educationPlan);
        }

        public void DeleteEducationPlan(long id)
        {
            _educationPlanDataMapper.Delete(id);
        }

        public List<EducationPlanCompare> FindAllUpdated()
        {
            return _educationPlanDataMapper.FindAllUpdated().ToList();
        }
    }
}