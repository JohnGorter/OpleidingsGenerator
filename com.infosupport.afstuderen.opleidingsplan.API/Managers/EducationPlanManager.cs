using AutoMapper;
using com.infosupport.afstuderen.opleidingsplan.api.Models;
using com.infosupport.afstuderen.opleidingsplan.DAL.mapper;
using com.infosupport.afstuderen.opleidingsplan.generator;
using com.infosupport.afstuderen.opleidingsplan.integration;
using com.infosupport.afstuderen.opleidingsplan.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.infosupport.afstuderen.opleidingsplan.api.Managers
{
    public class EducationPlanManager : IEducationPlanManager
    {
        private ICourseService _courseService;
        private IPlanner _planner;
        private IEducationPlanOutputter _educationPlanOutputter;
        private IDataMapper<model.Profile> _profileDataMapper;

        public EducationPlanManager(string profilePath)
        {
            _courseService = new CourseService();
            _planner = new Planner();
            _educationPlanOutputter = new EducationPlanOutputter(_planner);
            _profileDataMapper = new ProfileDataMapper(profilePath);
        }

        public EducationPlanManager(ICourseService courseService, IPlanner planner, IEducationPlanOutputter educationPlanOutputter, IDataMapper<model.Profile> profileDataMapper)
        {
            _courseService = courseService;
            _planner = planner;
            _educationPlanOutputter = educationPlanOutputter;
            _profileDataMapper = profileDataMapper;
        }

        private List<model.Course> ConvertCourses(IEnumerable<integration.Course> courses, model.Profile profile)
        {
            List<model.Course> coursesToPlan = new List<model.Course>();

            foreach (var course in courses)
            {
                model.Course courseToPlan = Mapper.Map<model.Course>(course);

                CoursePriority coursePriority = profile.Courses.FirstOrDefault(profileCourse => profileCourse.Code == course.Code);
                if (coursePriority != null)
                {
                    courseToPlan.Priority = coursePriority.Priority;
                }
                coursesToPlan.Add(courseToPlan);
            }

            return coursesToPlan;
        }

        public EducationPlan GenerateEducationPlan(RestEducationPlan educationPlan)
        {

            model.Profile profile = _profileDataMapper.Find(dataMapper => dataMapper.Name == educationPlan.Profile).First();

            IEnumerable<integration.Course> courses = _courseService.FindCourses(educationPlan.Courses);
            List<model.Course> coursesToPlan = ConvertCourses(courses, profile);

            _planner.PlanCourses(coursesToPlan);

            return _educationPlanOutputter.GenerateEducationPlan(Mapper.Map<EducationPlanData>(educationPlan));
        }



    }
}