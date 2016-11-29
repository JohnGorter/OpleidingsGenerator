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

        public EducationPlanManager()
        {
            _courseService = new CourseService();
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
            Planner planner = new Planner();

            IEnumerable<integration.Course> courses = _courseService.FindCourses(educationPlan.Courses);
            List<model.Course> coursesToPlan = ConvertCourses(courses);

            planner.PlanCourses(coursesToPlan);

            EducationPlanOutputter outputter = new EducationPlanOutputter(planner);
            return outputter.GenerateEducationPlan(Mapper.Map<EducationPlanData>(educationPlan));
        }



    }
}