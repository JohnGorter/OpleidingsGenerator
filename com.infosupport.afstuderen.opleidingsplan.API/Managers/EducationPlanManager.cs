using AutoMapper;
using com.infosupport.afstuderen.opleidingsplan.api.Models;
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

        public EducationPlanManager()
        {
            _courseService = new CourseService();
            _planner = new Planner();
            _educationPlanOutputter = new EducationPlanOutputter(_planner);
        }

        public EducationPlanManager(ICourseService courseService, IPlanner planner, IEducationPlanOutputter educationPlanOutputter)
        {
            _courseService = courseService;
            _planner = planner;
            _educationPlanOutputter = educationPlanOutputter;
        }

        private List<model.Course> ConvertCourses(IEnumerable<integration.Course> courses)
        {
            List<model.Course> coursesToPlan = new List<model.Course>();

            foreach (var course in courses)
            {
                model.Course courseToPlan = Mapper.Map<model.Course>(course);
                coursesToPlan.Add(courseToPlan);
            }

            return coursesToPlan;
        }

        public EducationPlan GenerateEducationPlan(RestEducationPlan educationPlan)
        {
            IEnumerable<integration.Course> courses = _courseService.FindCourses(educationPlan.Courses);
            List<model.Course> coursesToPlan = ConvertCourses(courses);

            _planner.PlanCourses(coursesToPlan);

            return _educationPlanOutputter.GenerateEducationPlan(Mapper.Map<EducationPlanData>(educationPlan));
        }



    }
}