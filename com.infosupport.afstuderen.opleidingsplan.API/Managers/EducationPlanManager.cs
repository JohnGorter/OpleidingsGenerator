using AutoMapper;
using com.infosupport.afstuderen.opleidingsplan.api.models;
using com.infosupport.afstuderen.opleidingsplan.dal.mappers;
using com.infosupport.afstuderen.opleidingsplan.generator;
using com.infosupport.afstuderen.opleidingsplan.integration;
using com.infosupport.afstuderen.opleidingsplan.models;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace com.infosupport.afstuderen.opleidingsplan.api.managers
{
    public class EducationPlanManager : IEducationPlanManager
    {
        private readonly ICourseService _courseService;
        private readonly IPlanner _planner;
        private readonly IEducationPlanOutputter _educationPlanOutputter;
        private readonly IDataMapper<opleidingsplan.models.CourseProfile> _profileDataMapper;
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
            IDataMapper<opleidingsplan.models.CourseProfile> profileDataMapper, IEducationPlanDataMapper educationPlanDataMapper, 
            IEducationPlanConverter educationPlanConverter)
        {
            _courseService = courseService;
            _planner = planner;
            _educationPlanOutputter = educationPlanOutputter;
            _profileDataMapper = profileDataMapper;
            _educationPlanDataMapper = educationPlanDataMapper;
            _educationPlanConverter = educationPlanConverter;
        }

        private static List<opleidingsplan.models.Course> ConvertCourses(IEnumerable<integration.Course> courses, opleidingsplan.models.CourseProfile profile)
        {
            _logger.Debug("ConvertCourses");
            List<opleidingsplan.models.Course> coursesToPlan = new List<opleidingsplan.models.Course>();

            foreach (var course in courses)
            {
                opleidingsplan.models.Course courseToPlan = Mapper.Map<opleidingsplan.models.Course>(course);
                if (profile != null)
                {
                    CoursePriority coursePriority = profile.Courses.FirstOrDefault(profileCourse => profileCourse.Code == course.Code);
                    if (coursePriority != null)
                    {
                        courseToPlan.Priority = coursePriority.Priority;
                    }
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
            opleidingsplan.models.CourseProfile profile = null;
            if (educationPlan.ProfileId != 0)
            {
                _logger.Debug(string.Format(_culture, "ProfileId exists: {0}", educationPlan.ProfileId));
                profile = _profileDataMapper.FindById(educationPlan.ProfileId);
                educationplanData.Profile = profile.Name;
                educationplanData.ProfileId = educationPlan.ProfileId;
            }

            educationPlan.Courses.Remove("OLC"); 

            _logger.Debug("Find courses from service");
            IEnumerable<integration.Course> courses = _courseService.FindCourses(educationPlan.Courses);
            List<opleidingsplan.models.Course> coursesToPlan = ConvertCourses(courses, profile);

            _planner.PlanCoursesWithOlc(coursesToPlan);

            return _educationPlanOutputter.GenerateEducationPlan(educationplanData);
        }

        public long SaveEducationPlan(RestEducationPlan restEducationPlan)
        {
            var educationPlan = GenerateEducationPlan(restEducationPlan);
            return _educationPlanDataMapper.Insert(educationPlan);
        }
        public long UpdateEducationPlan(RestEducationPlan restEducationPlan)
        {
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
    }
}