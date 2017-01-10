using AutoMapper;
using com.infosupport.afstuderen.opleidingsplan.api.models;
using com.infosupport.afstuderen.opleidingsplan.dal.mappers;
using com.infosupport.afstuderen.opleidingsplan.generator;
using com.infosupport.afstuderen.opleidingsplan.integration;
using com.infosupport.afstuderen.opleidingsplan.models;
using System;
using System.Collections.Generic;
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

        public EducationPlanManager(string profilePath, ICourseService courseService, string managementPropertiesPath)
        {
            _courseService = courseService;
            IManagementPropertiesDataMapper managementPropertiesDataMapper = new ManagementPropertiesJSONDataMapper(managementPropertiesPath);
            _planner = new Planner(managementPropertiesDataMapper);
            _educationPlanOutputter = new EducationPlanOutputter(_planner, managementPropertiesDataMapper);
            _profileDataMapper = new ProfileJsonDataMapper(profilePath);
        }

        public EducationPlanManager(string profilePath, string managementPropertiesPath)
        {
            _courseService = new CourseService();
            IManagementPropertiesDataMapper managementPropertiesDataMapper = new ManagementPropertiesJSONDataMapper(managementPropertiesPath);
            _planner = new Planner(managementPropertiesDataMapper);
            _educationPlanOutputter = new EducationPlanOutputter(_planner, managementPropertiesDataMapper);
            _profileDataMapper = new ProfileJsonDataMapper(profilePath);
        }

        public EducationPlanManager(ICourseService courseService, IPlanner planner, IEducationPlanOutputter educationPlanOutputter, IDataMapper<opleidingsplan.models.CourseProfile> profileDataMapper)
        {
            _courseService = courseService;
            _planner = planner;
            _educationPlanOutputter = educationPlanOutputter;
            _profileDataMapper = profileDataMapper;
        }

        private static List<opleidingsplan.models.Course> ConvertCourses(IEnumerable<integration.Course> courses, opleidingsplan.models.CourseProfile profile)
        {
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
                throw new ArgumentNullException("educationPlan");
            }

            _planner.StartDate = educationPlan.InPaymentFrom;
            _planner.BlockedDates = educationPlan.BlockedDates;

            opleidingsplan.models.CourseProfile profile = null;
            if (educationPlan.ProfileId != 0)
            {
                profile = _profileDataMapper.FindById(educationPlan.ProfileId);
            }

            IEnumerable<integration.Course> courses = _courseService.FindCourses(educationPlan.Courses);
            List<opleidingsplan.models.Course> coursesToPlan = ConvertCourses(courses, profile);

            _planner.PlanCoursesWithOLC(coursesToPlan);

            return _educationPlanOutputter.GenerateEducationPlan(Mapper.Map<EducationPlanData>(educationPlan));
        }



    }
}