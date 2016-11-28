using com.infosupport.afstuderen.opleidingsplan.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public class EducationPlanOutputter
    {
        private Planner _planner;

        public EducationPlanOutputter(Planner planner)
        {
            _planner = planner;
        }

        public EducationPlan GenerateEducationPlan()
        {
            List<EducationPlanCourse> educationPlannedCourses = GetEducationPlanCourses(_planner.GetPlannedCourses().ToList());
            List<EducationPlanCourse> educationNotPlannedCourses = GetEducationPlanCourses(_planner.GetNotPlannedCourses().ToList());

            return new EducationPlan
            {
                Created = DateTime.Now,
                PlannedCourses = educationPlannedCourses,
                NotPlannedCourses = educationNotPlannedCourses,
            };
        }

        private List<EducationPlanCourse> GetEducationPlanCourses(List<generator.Course> coursesFromPlanner)
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
    }
}
