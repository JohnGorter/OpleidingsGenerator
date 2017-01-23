using com.infosupport.afstuderen.opleidingsplan.dal.mappers;
using com.infosupport.afstuderen.opleidingsplan.integration;
using com.infosupport.afstuderen.opleidingsplan.models;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public class EducationPlanOutputter : IEducationPlanOutputter
    {
        private static ILog _logger = LogManager.GetLogger(typeof(CourseImplementation));
        private static readonly CultureInfo _culture = new CultureInfo("nl-NL");

        private readonly IPlanner _planner;
        private readonly IManagementPropertiesDataMapper _managementPropertiesDataMapper;

        public EducationPlanOutputter(IPlanner planner, IManagementPropertiesDataMapper managementPropertiesDataMapper)
        {
            _planner = planner;
            _managementPropertiesDataMapper = managementPropertiesDataMapper;
        }

        public EducationPlan GenerateEducationPlan(EducationPlanData educationPlanData)
        {
            _logger.Debug("GenerateEducationPlan");
            if (educationPlanData == null)
            {
                _logger.Error("ArgumentNullException educationPlanData");
                throw new ArgumentNullException("educationPlanData");
            }

            List<EducationPlanCourse> educationPlannedCourses = GetPlannedEducationPlanCourses(_planner.PlannedCourses.ToList()).OrderBy(course => course.Date).ToList();
            List<EducationPlanCourse> educationNotPlannedCourses = GetNotPlannedEducationPlanCourses(_planner.NotPlannedCourses.ToList(), educationPlannedCourses)
                .OrderBy(course => course.Date).ToList();

            int daysBeforeStart = _managementPropertiesDataMapper.FindManagementProperties().PeriodBeforeStartNotifiable;
            DateTime justBeforeStart = educationPlanData.InPaymentFrom.AddDays(-daysBeforeStart);
            var coursesJustBeforeStart = _planner.NotPlannedCourses
                .Where(course => course.CourseImplementations
                .Any(ci => ci.StartDay >= justBeforeStart && ci.StartDay < _planner.StartDate))
                .ToList();
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

            _logger.Debug("return EducationPlan");
            return new EducationPlan
            {
                Id = educationPlanData.EducationPlanId,
                Created = DateTime.Now,
                PlannedCourses = educationPlannedCourses,
                NotPlannedCourses = educationNotPlannedCourses,
                CoursesJustBeforeStart = educationCoursesJustBeforeStart,
                InPaymentFrom = educationPlanData.InPaymentFrom,
                EmployableFrom = dateEmployableFrom,
                Profile = educationPlanData.Profile,
                ProfileId = educationPlanData.ProfileId,
                NameEmployee = educationPlanData.NameEmployee,
                NameTeacher = educationPlanData.NameTeacher,
                KnowledgeOf = educationPlanData.KnowledgeOf,
                BlockedDates = educationPlanData.BlockedDates,
            };
        }

        private List<EducationPlanCourse> GetJustBeforeStartDateNotPlannedEducationPlanCourses(List<generator.Course> coursesFromPlanner, DateTime justBeforeStart)
        {
            _logger.Debug(string.Format(_culture, "GetJustBeforeStartDateNotPlannedEducationPlanCourses with date justBeforeStart {0}", justBeforeStart.ToString("dd-MM-yyyy")));
            List<EducationPlanCourse> educationPlanCourses = new List<EducationPlanCourse>();
            var discount = _managementPropertiesDataMapper.FindManagementProperties().StaffDiscount;

            foreach (var course in coursesFromPlanner)
            {
                _logger.Debug(string.Format(_culture, "Course {0} is just before startdate", course.Code));

                DateTime? startDay = course.CourseImplementations.FirstOrDefault(ci => ci.StartDay > justBeforeStart)?.StartDay;
                educationPlanCourses.Add(new EducationPlanCourse
                {
                    Code = course.Code,
                    Date = startDay,
                    Days = course.Duration.Value,
                    Name = course.Name,
                    Price = course.Price,
                    StaffDiscountInPercentage = discount,
                });
            }

            return educationPlanCourses;
        }

        private List<EducationPlanCourse> GetPlannedEducationPlanCourses(List<generator.Course> coursesFromPlanner)
        {
            _logger.Debug("GetPlannedEducationPlanCourses");
            List<EducationPlanCourse> educationPlanCourses = new List<EducationPlanCourse>();
            var discount = _managementPropertiesDataMapper.FindManagementProperties().StaffDiscount;

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
                    StaffDiscountInPercentage = discount,
                });
            }

            return educationPlanCourses;
        }

        private List<EducationPlanCourse> GetNotPlannedEducationPlanCourses(List<generator.Course> coursesFromPlanner, List<EducationPlanCourse> plannedCourses)
        {
            _logger.Debug("GetNotPlannedEducationPlanCourses");
            List<EducationPlanCourse> educationPlanCourses = new List<EducationPlanCourse>();
            var discount = _managementPropertiesDataMapper.FindManagementProperties().StaffDiscount;

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
                    StaffDiscountInPercentage = discount,
                });
            }

            return educationPlanCourses;
        }
    }
}
