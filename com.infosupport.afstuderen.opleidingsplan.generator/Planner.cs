using com.infosupport.afstuderen.opleidingsplan.dal.mappers;
using com.infosupport.afstuderen.opleidingsplan.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public class Planner : IPlanner
    {
        private IManagementPropertiesDataMapper _managementPropertiesDataMapper;
        private CoursePlanning _coursePlanning = new CoursePlanning();
        private DateTime _startDate = DateTime.Now;
        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                if (value != null)
                {
                    _startDate = value.Date;
                }
            }
        }
        private List<DateTime> _blockedDates = new List<DateTime>();

        public List<DateTime> BlockedDates
        {
            get
            {
                return _blockedDates;
            }
            set
            {
                if (value != null)
                {
                    _blockedDates = value.Select(date => date.Date).ToList();
                }
            }
        }

        public Planner(IManagementPropertiesDataMapper managementPropertiesDataMapper)
        {
            _managementPropertiesDataMapper = managementPropertiesDataMapper;
        }

        public IEnumerable<Course> GetPlannedCourses()
        {
            return _coursePlanning.GetPlannedCourses();
        }
        public IEnumerable<Course> GetNotPlannedCourses()
        {
            return _coursePlanning.GetNotPlannedCourses();
        }

        public IEnumerable<Course> GetAllCourses()
        {
            return _coursePlanning.GetCourses();
        }

        public void PlanCoursesWithOLC(IEnumerable<models.Course> coursesToPlan)
        {
            PlanCourses(coursesToPlan);
            ApplyOLC();
        }

        public void PlanCourses(IEnumerable<models.Course> coursesToPlan)
        {
            coursesToPlan = coursesToPlan.OrderBy(course => course.Priority);

            foreach (var courseToPlan in coursesToPlan)
            {
                var course = (generator.Course)courseToPlan;
                var knownCourses = _coursePlanning.GetCourses();

                MarkCourseImplementations(course, knownCourses);
                _coursePlanning.AddCourse(course);
            }

            var availableCourses = _coursePlanning.GetAvailableCourses();
            if (availableCourses.Any())
            {
                MarkAvailableCourseImplementations();
            }

            var plannedCourses = _coursePlanning.GetPlannedCourses();
            var notPlannedCourses = _coursePlanning.GetNotPlannedCourses();
            foreach (var notPlannedCourse in notPlannedCourses)
            {
                notPlannedCourse.AddIntersectedCourses(plannedCourses);
            }
        }

        private generator.Course RemoveBlockedAndOutsitePeriodImplementations(generator.Course course)
        {
            if (BlockedDates != null)
            {
                int periodEducationPlanDays = _managementPropertiesDataMapper.FindManagementProperties().PeriodEducationPlanInDays;

                DateTime endDate = StartDate.GetEndDay(periodEducationPlanDays);

                var blockedImplementations = course.CourseImplementations.Where(ci => ci.StartDay < StartDate || ci.StartDay > endDate || ci.Days.Any(day => BlockedDates.Contains(day)));

                foreach (var implementation in blockedImplementations)
                {
                    implementation.Status = Status.UNPLANNABLE;
                }
            }

            return course;
        }

        private void MarkCourseImplementations(generator.Course course, IEnumerable<generator.Course> knownCourses)
        {
            course = RemoveBlockedAndOutsitePeriodImplementations(course);

            if (course.HasOnlyImplementationsWithStatus(Status.UNPLANNABLE))
            {
                return;
            }

            if (course.HasOneImplementation())
            {
                if (course.IsPlannable(knownCourses))
                {
                    course.MarkAllImplementations(Status.NOTPLANNED);
                    course.MarkOnlyAvailableImplementationPlanned(knownCourses);
                    course.MarkAllIntersectedOfPlannedImplementations(Status.UNPLANNABLE, knownCourses);
                }
                else
                {
                    course.MarkAllImplementations(Status.UNPLANNABLE);
                }
            }
            else if (course.HasMultipleImplementationsWithStatus(Status.UNKNOWN))
            {
                course.MarkAllImplementations(Status.AVAILABLE);
            }
            else
            {
                course.MarkAllImplementations(Status.UNPLANNABLE);
            }
        }

        private void MarkAvailableCourseImplementations()
        {
            var availableCourses = _coursePlanning.GetAvailableCourses();
            var plannedCourses = _coursePlanning.GetCourses();

            while (availableCourses.Any())
            {
                var course = availableCourses.First();

                if (course.HasAvailableImplementations(plannedCourses))
                {
                    course.MarkAllImplementations(Status.NOTPLANNED);
                    course.MarkMinimumIntersectedFirstAvailableImplementationPlanned(plannedCourses);
                    course.MarkAllIntersectedOfPlannedImplementations(Status.UNPLANNABLE, plannedCourses);
                }
                else
                {
                    course.MarkAllImplementations(Status.UNPLANNABLE);
                }
            }

        }

        private void ApplyOLC()
        {
            List<Course> plannedCourses = _coursePlanning.GetPlannedCourses().OrderBy(course => course.GetPlannedImplementation().StartDay).ToList();

            int daysAfterLastCourseEmployable = _managementPropertiesDataMapper.FindManagementProperties().PeriodAfterLastCourseEmployableInDays;

            DateTime dateEmployableFrom = StartDate.AddDays(daysAfterLastCourseEmployable);

            DateTime? lastDayOfPlanning = plannedCourses.LastOrDefault()?.GetPlannedImplementation().Days.Last();

            if (lastDayOfPlanning != null)
            {
                dateEmployableFrom = lastDayOfPlanning.Value
                    .AddDays(daysAfterLastCourseEmployable);
            }

            var datesOfPeriod = Enumerable.Range(0, 1 + dateEmployableFrom.Subtract(StartDate).Days)
               .Select(offset => StartDate.AddDays(offset))
               .ToList();

            List<EducationPlanCourse> olcCourses = new List<EducationPlanCourse>();

            List<DateTime> olcDates = new List<DateTime>();
            Course previousCourse = new Course();
            Course previousDayCourse = new Course();

            foreach (var date in datesOfPeriod)
            {

                if (IsWeekend(date))
                {
                    if(olcDates.Any())
                    {
                        AddOLC(olcDates);
                        olcDates = new List<DateTime>();
                    }
                    continue;
                }

                Course selectedCourse = plannedCourses.FirstOrDefault(c => c.CourseImplementations.Any(ci => ci.Days.Contains(date) && ci.Status == Status.PLANNED));

                if (selectedCourse == null && !BlockedDates.Contains(date))
                {
                    olcDates.Add(date);
                }

                if ((previousCourse != selectedCourse && previousDayCourse == null) && selectedCourse != null || BlockedDates.Contains(date))
                {
                    if (olcDates.Any())
                    {
                        AddOLC(olcDates);
                        olcDates = new List<DateTime>();
                    }
                }

                if (selectedCourse != null)
                {
                    previousCourse = selectedCourse;
                }
                previousDayCourse = selectedCourse;
            }

            if (olcDates.Any())
            {
                AddOLC(olcDates);
            }
        }

        private void AddOLC(List<DateTime> dates)
        {
            decimal olcPrice = _managementPropertiesDataMapper.FindManagementProperties().OLCPrice;

            Course olc = new Course
            {
                Code = "OLC",
                Name = "OLC",
                CourseImplementations = new List<CourseImplementation>()
                {
                    new CourseImplementation
                    {
                        Days = dates,
                        Status = Status.PLANNED,
                    }
                },
                Price = olcPrice * dates.Count,
                Duration = dates.Count,
            };

            _coursePlanning.AddCourse(olc);
        }

        private bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }
    }
}
