using com.infosupport.afstuderen.opleidingsplan.dal.mappers;
using com.infosupport.afstuderen.opleidingsplan.integration;
using com.infosupport.afstuderen.opleidingsplan.models;
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
        private IManagementPropertiesDataMapper _managementPropertiesDataMapper;

        public EducationPlanOutputter(IPlanner planner, IManagementPropertiesDataMapper managementPropertiesDataMapper)
        {
            _planner = planner;
            _managementPropertiesDataMapper = managementPropertiesDataMapper;
        }

        public EducationPlan GenerateEducationPlan(EducationPlanData educationPlanData)
        {
            if (educationPlanData == null)
            {
                throw new ArgumentNullException("educationPlanData");
            }

            List<EducationPlanCourse> educationPlannedCourses = GetPlannedEducationPlanCourses(_planner.PlannedCourses.ToList()).OrderBy(course => course.Date).ToList();
            List<EducationPlanCourse> educationNotPlannedCourses = GetNotPlannedEducationPlanCourses(_planner.NotPlannedCourses.ToList(), educationPlannedCourses).OrderBy(course => course.Date).ToList();

            int daysBeforeStart = _managementPropertiesDataMapper.FindManagementProperties().PeriodBeforeStartNotifiable;
            DateTime justBeforeStart = educationPlanData.InPaymentFrom.AddDays(-daysBeforeStart);
            var coursesJustBeforeStart = _planner.NotPlannedCourses.Where(course => course.CourseImplementations.Any(ci => ci.StartDay >= justBeforeStart && ci.StartDay < _planner.StartDate)).ToList();
            List<EducationPlanCourse> educationCoursesJustBeforeStart = GetJustBeforeStartDateNotPlannedEducationPlanCourses(coursesJustBeforeStart, justBeforeStart).ToList();

            int daysAfterLastCourseEmployable = _managementPropertiesDataMapper.FindManagementProperties().PeriodAfterLastCourseEmployableInDays;

            DateTime dateEmployableFrom = educationPlanData.InPaymentFrom.AddDays(daysAfterLastCourseEmployable);
            DateTime? lastDayOfPlanning = educationPlannedCourses.LastOrDefault()?.Date;

            if (lastDayOfPlanning != null)
            {
                dateEmployableFrom = lastDayOfPlanning.Value
                    .AddDays(educationPlannedCourses.Last().Days)
                    .AddDays(daysAfterLastCourseEmployable);
            }

            return new EducationPlan
            {
                Created = DateTime.Now,
                PlannedCourses = educationPlannedCourses,
                NotPlannedCourses = educationNotPlannedCourses,
                CoursesJustBeforeStart = educationCoursesJustBeforeStart,
                InPaymentFrom = educationPlanData.InPaymentFrom,
                EmployableFrom = dateEmployableFrom,
                Profile = educationPlanData.Profile,
                NameEmployee = educationPlanData.NameEmployee,
                NameTeacher = educationPlanData.NameTeacher,
                KnowledgeOf = educationPlanData.KnowledgeOf,
                BlockedDates = educationPlanData.BlockedDates,
            };
        }

        private static List<EducationPlanCourse> GetJustBeforeStartDateNotPlannedEducationPlanCourses(List<generator.Course> coursesFromPlanner, DateTime justBeforeStart)
        {
            List<EducationPlanCourse> educationPlanCourses = new List<EducationPlanCourse>();

            foreach (var course in coursesFromPlanner)
            {
                DateTime? startDay = course.CourseImplementations.FirstOrDefault(ci => ci.StartDay > justBeforeStart)?.StartDay;
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

        private static List<EducationPlanCourse> GetPlannedEducationPlanCourses(List<generator.Course> coursesFromPlanner)
        {
            List<EducationPlanCourse> educationPlanCourses = new List<EducationPlanCourse>();

            foreach (var course in coursesFromPlanner)
            {
                DateTime? startDay = course.PlannedImplementation?.StartDay;

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

        private static List<EducationPlanCourse> GetNotPlannedEducationPlanCourses(List<generator.Course> coursesFromPlanner, List<EducationPlanCourse> plannedCourses)
        {
            List<EducationPlanCourse> educationPlanCourses = new List<EducationPlanCourse>();

            foreach (var course in coursesFromPlanner)
            {
                DateTime? startDay = course.PlannedImplementation?.StartDay;

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
