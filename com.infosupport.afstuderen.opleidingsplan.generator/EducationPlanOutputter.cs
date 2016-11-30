using com.infosupport.afstuderen.opleidingsplan.integration;
using com.infosupport.afstuderen.opleidingsplan.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public class EducationPlanOutputter : IEducationPlanOutputter
    {
        private IPlanner _planner;

        public EducationPlanOutputter(IPlanner planner)
        {
            _planner = planner;
        }

        public EducationPlan GenerateEducationPlan(EducationPlanData educationPlanData)
        {
            List<EducationPlanCourse> educationPlannedCourses = GetPlannedEducationPlanCourses(_planner.GetPlannedCourses().ToList()).OrderBy(course => course.Date).ToList();
            List<EducationPlanCourse> educationNotPlannedCourses = GetNotPlannedEducationPlanCourses(_planner.GetNotPlannedCourses().ToList(), educationPlannedCourses).OrderBy(course => course.Date).ToList();

            return new EducationPlan
            {
                Created = educationPlanData.Created,
                PlannedCourses = educationPlannedCourses,
                NotPlannedCourses = educationNotPlannedCourses,
                InPaymentFrom = educationPlanData.InPaymentFrom,
                EmployableFrom = educationPlanData.EmployableFrom,
                Profile = educationPlanData.Profile,
                NameEmployee = educationPlanData.NameEmployee,
                NameTeacher = educationPlanData.NameTeacher,
                KnowledgeOf = educationPlanData.KnowledgeOf,
            };
        }

        private List<EducationPlanCourse> GetPlannedEducationPlanCourses(List<generator.Course> coursesFromPlanner)
        {
            List<EducationPlanCourse> educationPlanCourses = new List<EducationPlanCourse>();

            foreach (var course in coursesFromPlanner)
            {
                DateTime? startDay = course.GetPlannedImplementation()?.StartDay;

                educationPlanCourses.Add(new EducationPlanCourse
                {
                    Code = course.Code,
                    Date = startDay,
                    Days = course.Duration.Value,
                    Name = course.Name,
                    Price = course.Price,
                });
            }

            return educationPlanCourses;
        }

        private List<EducationPlanCourse> GetNotPlannedEducationPlanCourses(List<generator.Course> coursesFromPlanner, List<EducationPlanCourse> plannedCourses)
        {
            List<EducationPlanCourse> educationPlanCourses = new List<EducationPlanCourse>();

            foreach (var course in coursesFromPlanner)
            {
                DateTime? startDay = course.GetPlannedImplementation()?.StartDay;

                educationPlanCourses.Add(new EducationPlanCourse
                {
                    Code = course.Code,
                    Date = startDay,
                    Days = course.Duration.Value,
                    Name = course.Name,
                    Price = course.Price,
                    IntersectedCourses = plannedCourses.Where(plannedCourse => course.IntersectedCourseIds.Contains(plannedCourse.Code)).ToList(),
                });
            }

            return educationPlanCourses;
        }
    }
}
