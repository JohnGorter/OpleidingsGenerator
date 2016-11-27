using AutoMapper;
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

        public EducationPlan GenerateEducationPlan(string[] courseCodes)
        {
            Planner planner = new Planner();

            IEnumerable<integration.Course> courses = _courseService.FindCourses(courseCodes);
            List<model.Course> coursesToPlan = ConvertCourses(courses);

            planner.PlanCourses(coursesToPlan);

            return CreateEducationPlan(planner, coursesToPlan);
        }


        private EducationPlan CreateEducationPlan(Planner planner, List<model.Course> coursesToPlan)
        {
            List<EducationPlanCourse> educationPlannedCourses = GetEducationPlanCourses(planner, planner.GetPlannedCourses().ToList(), coursesToPlan);
            List<EducationPlanCourse> educationNotPlannedCourses = GetEducationPlanCourses(planner, planner.GetNotPlannedCourses().ToList(), coursesToPlan);

            return new EducationPlan
            {
                Created = DateTime.Now,
                PlannedCourses = educationPlannedCourses,
                NotPlannedCourses = educationNotPlannedCourses,
            };
        }

        private List<EducationPlanCourse> GetEducationPlanCourses(Planner planner, List<generator.Course> coursesFromPlanner, List<model.Course> coursesDetails)
        {
            List<EducationPlanCourse> educationPlanCourses = new List<EducationPlanCourse>();

            foreach (var course in coursesFromPlanner)
            {
                var matchedCourse = coursesDetails.First(c => c.Code == course.Code);

                DateTime? startDay = null;

                try
                {
                    startDay = course.GetPlannedImplementation().StartDay;
                }
                catch(AmountImplementationException)
                {
                }
    

                educationPlanCourses.Add(new EducationPlanCourse
                {
                    Code = course.Code,
                    Date = startDay,
                    Days = matchedCourse.Duration.ToInt(),
                    Name = matchedCourse.Name,
                    Price = matchedCourse.Price,
                });
            }

            return educationPlanCourses;
        }
    }
}