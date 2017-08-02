using InfoSupport.KC.OpleidingsplanGenerator.Dal.Mappers;
using InfoSupport.KC.OpleidingsplanGenerator.Integration;
using InfoSupport.KC.OpleidingsplanGenerator.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSupport.KC.OpleidingsplanGenerator.Generator
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

            var plannedCourses = _planner.PlannedCourses.ToList();
           // var otherCourses = _planner.AllCourses.Where(c => plannedCourses.FirstOrDefault(pc => pc.Code.Equals(c.Code)) == null).ToList();
            var otherCourses = _planner.AllCourses.ToList();
            List <EducationPlanCourse> educationPlannedCourses = GetPlannedEducationPlanCourses(plannedCourses, otherCourses).ToList()
                .OrderBy(course => course.Date).ToList();
            List<EducationPlanCourse> educationNotPlannedCourses = GetNotPlannedEducationPlanCourses(_planner.NotPlannedCourses.ToList())
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

            if (lastDayOfPlanning.HasValue)
            {
                dateEmployableFrom = lastDayOfPlanning.Value
                    .AddDays(educationPlannedCourses.Last().Days)
                    .AddDays(daysAfterLastCourseEmployable);
            }

            if (educationPlanData.EmployableFrom.HasValue)
            {
                dateEmployableFrom = educationPlanData.EmployableFrom.Value;
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

        private List<EducationPlanCourse> GetJustBeforeStartDateNotPlannedEducationPlanCourses(List<Generator.Course> coursesFromPlanner, DateTime justBeforeStart)
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

        private List<EducationPlanCourse> GetPlannedEducationPlanCourses(List<Generator.Course> coursesFromPlanner, List<Generator.Course> unplannableCoursesFromPlanner)
        {
            _logger.Debug("GetPlannedEducationPlanCourses");
            List<EducationPlanCourse> educationPlanCourses = new List<EducationPlanCourse>();
            var discount = _managementPropertiesDataMapper.FindManagementProperties().StaffDiscount;

            foreach (var course in coursesFromPlanner)
            {
                var discountPerCourse = discount;

                DateTime? startDay = course.PlannedImplementation?.StartDay;
                Boolean? PinnedCourse = course.PlannedImplementation?.Pinned;
                if (PinnedCourse.HasValue && PinnedCourse.Value == true)
                    Debug.WriteLine("Pin");

                List<Course> unplannableIntersectedCourses = unplannableCoursesFromPlanner.FindAll(x => x.IntersectsWithPlanned(course) && x.Code != course.Code).ToList();
                List<EducationPlanCourse> intersectedEducationPlanCourses = GetNotPlannedEducationPlanCourses(unplannableIntersectedCourses);

                educationPlanCourses.Add(new EducationPlanCourse
                {
                    Code = course.Code,
                    Date = startDay,
                    Days = course.Duration.Value,
                    Name = course.Name,
                    Price = course.Price,
                    Pinned = PinnedCourse.HasValue ? PinnedCourse.Value : false,
                    StaffDiscountInPercentage = discountPerCourse,
                    Commentary = course.Commentary,
                    IntersectedCourses = intersectedEducationPlanCourses
                });
            }

            return educationPlanCourses;
        }

        private List<EducationPlanCourse> GetNotPlannedEducationPlanCourses(List<Generator.Course> coursesFromPlanner)
        {
            _logger.Debug("GetNotPlannedEducationPlanCourses");
            List<EducationPlanCourse> educationPlanCourses = new List<EducationPlanCourse>();
            var discount = _managementPropertiesDataMapper.FindManagementProperties().StaffDiscount;

            foreach (var course in coursesFromPlanner)
            {
                var discountPerCourse = discount;

                DateTime? startDay = course.PlannedImplementation?.StartDay;

                educationPlanCourses.Add(new EducationPlanCourse
                {
                    Code = course.Code,
                    Date = startDay,
                    Days = course.Duration.Value,
                    Name = course.Name,
                    Price = course.Price,
                    StaffDiscountInPercentage = discountPerCourse,
                    Commentary = course.Commentary,
                });
            }

            return educationPlanCourses;
        }
    }
}
