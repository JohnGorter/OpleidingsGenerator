using com.infosupport.afstuderen.opleidingsplan.dal.mappers;
using com.infosupport.afstuderen.opleidingsplan.models;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public class Planner : IPlanner
    {
        private readonly IManagementPropertiesDataMapper _managementPropertiesDataMapper;
        private readonly CoursePlanning _coursePlanning = new CoursePlanning();
        private DateTime _startDate = DateTime.Now;
        private static ILog _logger = LogManager.GetLogger(typeof(Planner));
        private readonly CultureInfo _culture = new CultureInfo("nl-NL");

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
        private Collection<DateTime> _blockedDates = new Collection<DateTime>();

        public Collection<DateTime> BlockedDates
        {
            get
            {
                return _blockedDates;
            }
            set
            {
                if (value != null)
                {
                    Collection<DateTime> blockedDates = new Collection<DateTime>();
                    foreach (var blockedDate in value)
                    {
                        blockedDates.Add(blockedDate.Date);
                    }
                    _blockedDates = blockedDates;
                }
            }
        }

        public IEnumerable<Course> PlannedCourses
        {
            get
            {
                return _coursePlanning.PlannedCourses;
            }
        }

        public IEnumerable<Course> NotPlannedCourses
        {
            get
            {
                return _coursePlanning.NotPlannedCourses;
            }
        }

        public IEnumerable<Course> AllCourses
        {
            get
            {
                return _coursePlanning.Courses;
            }
        }


        public Planner(IManagementPropertiesDataMapper managementPropertiesDataMapper)
        {
            _managementPropertiesDataMapper = managementPropertiesDataMapper;
        }

        public void PlanCoursesWithOLC(IEnumerable<models.Course> coursesToPlan)
        {
            _logger.Debug("Plan courses with OLC");
            PlanCourses(coursesToPlan);
            ApplyOLC();
        }

        public void PlanCourses(IEnumerable<models.Course> coursesToPlan)
        {
            _logger.Debug("Plan courses");
            var coursesToPlanOrdered = coursesToPlan.OrderBy(course => course.Priority);

            foreach (var courseToPlan in coursesToPlanOrdered)
            {
                var course = (generator.Course)courseToPlan;
                var knownCourses = _coursePlanning.Courses;

                MarkCourseImplementations(course, knownCourses);
                _coursePlanning.Courses.Add(course);
                _logger.Debug(string.Format(_culture, "Course {0} added to course planning", course.Code));
            }

            var availableCourses = _coursePlanning.AvailableCourses;
            if (availableCourses.Any())
            {
                _logger.Debug(string.Format(_culture, "{0} available courses in courseplanning", availableCourses.Count()));
                MarkAvailableCourseImplementations();
            }

            var plannedCourses = _coursePlanning.PlannedCourses;
            var notPlannedCourses = _coursePlanning.NotPlannedCourses;
            _logger.Debug(string.Format(_culture, "{0} not planned courses", notPlannedCourses.Count()));
            foreach (var notPlannedCourse in notPlannedCourses)
            {
                notPlannedCourse.AddIntersectedCourses(plannedCourses);
            }
        }

        private generator.Course RemoveBlockedAndOutsidePeriodImplementations(generator.Course course)
        {
            _logger.Debug(string.Format(_culture, "Remove blocked implementations and implementations outside the period from course {0}", course.Code));

            if (BlockedDates != null)
            {
                _logger.Debug(string.Format(_culture, "Remove {0} blocked implementations from course {1}", BlockedDates.Count, course.Code));

                int periodEducationPlanDays = _managementPropertiesDataMapper.FindManagementProperties().PeriodEducationPlanInDays;
                DateTime endDate = StartDate.GetEndDay(periodEducationPlanDays);

                _logger.Debug(string.Format(_culture, "Period for education plan is {0} days", periodEducationPlanDays));

                var blockedImplementations = course.CourseImplementations.Where(ci => ci.StartDay < StartDate || ci.StartDay > endDate || ci.Days.Any(day => BlockedDates.Contains(day)));

                _logger.Debug("Set status of blocked implementations and implementations outside the period on unplannable");
                foreach (var implementation in blockedImplementations)
                {
                    implementation.Status = Status.UNPLANNABLE;
                }
            }

            return course;
        }

        private void MarkCourseImplementations(generator.Course course, IEnumerable<generator.Course> knownCourses)
        {
            _logger.Debug(string.Format(_culture, "Mark course implementations from course with code {0}", course.Code));
            var courseWithoutBlocked = RemoveBlockedAndOutsidePeriodImplementations(course);

            if (courseWithoutBlocked.HasOnlyImplementationsWithStatus(Status.UNPLANNABLE))
            {
                _logger.Debug(string.Format(_culture, "Course {0} has only implementations with status unplannable", courseWithoutBlocked.Code));
                return;
            }

            if (courseWithoutBlocked.HasOneImplementation())
            {
                _logger.Debug(string.Format(_culture, "Course {0} has only one implementations with status available", courseWithoutBlocked.Code));

                if (courseWithoutBlocked.IsPlannable(knownCourses))
                {
                    _logger.Debug(string.Format(_culture, "Course {0} is plannable", courseWithoutBlocked.Code));
                    courseWithoutBlocked.MarkAllImplementations(Status.NOTPLANNED);
                    courseWithoutBlocked.MarkOnlyAvailableImplementationPlanned(knownCourses);
                    courseWithoutBlocked.MarkAllIntersectedOfPlannedImplementations(Status.UNPLANNABLE, knownCourses);
                }
                else
                {
                    _logger.Debug(string.Format(_culture, "Course {0} is not plannable", courseWithoutBlocked.Code));
                    courseWithoutBlocked.MarkAllImplementations(Status.UNPLANNABLE);
                }
            }
            else if (courseWithoutBlocked.HasMultipleImplementationsWithStatus(Status.UNKNOWN))
            {
                _logger.Debug(string.Format(_culture, "Course {0} has multiple implementations with status unknown", courseWithoutBlocked.Code));
                courseWithoutBlocked.MarkAllImplementations(Status.AVAILABLE);
            }
            else
            {
                _logger.Debug(string.Format(_culture, "Course {0} has only implementations with status unplannable", courseWithoutBlocked.Code));
                courseWithoutBlocked.MarkAllImplementations(Status.UNPLANNABLE);
            }
        }

        private void MarkAvailableCourseImplementations()
        {
            _logger.Debug("Mark available course implementations");
            var availableCourses = _coursePlanning.AvailableCourses;
            var plannedCourses = _coursePlanning.Courses;

            while (availableCourses.Any())
            {
                var course = availableCourses.First();
                _logger.Debug(string.Format(_culture, "Mark available course {0}", course.Code));

                if (course.HasAvailableImplementations(plannedCourses))
                {
                    _logger.Debug(string.Format(_culture, "Course {0} has available course implementations", course.Code));
                    course.MarkAllImplementations(Status.NOTPLANNED);
                    course.MarkMinimumIntersectedFirstAvailableImplementationPlanned(plannedCourses);
                    course.MarkAllIntersectedOfPlannedImplementations(Status.UNPLANNABLE, plannedCourses);
                }
                else
                {
                    _logger.Debug(string.Format(_culture, "Course {0} has no available course implementations", course.Code));
                    course.MarkAllImplementations(Status.UNPLANNABLE);
                }
            }

        }

        private void ApplyOLC()
        {
            _logger.Debug("Apply OLC");
            List<Course> plannedCourses = _coursePlanning.PlannedCourses.OrderBy(course => course.PlannedImplementation.StartDay).ToList();

            int daysAfterLastCourseEmployable = _managementPropertiesDataMapper.FindManagementProperties().PeriodAfterLastCourseEmployableInDays;

            DateTime dateEmployableFrom = StartDate.AddDays(daysAfterLastCourseEmployable);

            DateTime? lastDayOfPlanning = plannedCourses.LastOrDefault()?.PlannedImplementation.Days.Last();

            if (lastDayOfPlanning != null)
            {
                dateEmployableFrom = lastDayOfPlanning.Value
                    .AddDays(daysAfterLastCourseEmployable);
            }

            var datesOfPeriod = Enumerable.Range(0, 1 + dateEmployableFrom.Subtract(StartDate).Days)
               .Select(offset => StartDate.AddDays(offset))
               .ToList();

            _logger.Debug(string.Format(_culture, "{0} dates in periode found", datesOfPeriod.Count));


            List<DateTime> olcDates = new List<DateTime>();
            Course previousCourse = new Course();
            Course previousDayCourse = new Course();

            foreach (var date in datesOfPeriod)
            {
                _logger.Debug(string.Format(_culture, "Check date {0} for applying OLC", date.ToString("dd-MM-yyyy")));
                if (IsWeekend(date))
                {
                    _logger.Debug(string.Format(_culture, "Date {0} is weekend", date.ToString("dd-MM-yyyy")));
                    if (olcDates.Any())
                    {
                        _logger.Debug(string.Format(_culture, "OLC dates is not empty at date {0}", date.ToString("dd-MM-yyyy")));
                        AddOLC(olcDates);
                        olcDates = new List<DateTime>();
                    }
                    continue;
                }

                Course selectedCourse = plannedCourses.FirstOrDefault(c => c.CourseImplementations.Any(ci => ci.Days.Contains(date) && ci.Status == Status.PLANNED));

                if (selectedCourse == null && !BlockedDates.Contains(date))
                {
                    _logger.Debug(string.Format(_culture, "Add date {0} to OLC dates", date.ToString("dd-MM-yyyy")));
                    olcDates.Add(date);
                }

                if (((previousCourse != selectedCourse && previousDayCourse == null) && selectedCourse != null || BlockedDates.Contains(date)) && olcDates.Any())
                {
                    _logger.Debug(string.Format(_culture, "OLC dates is not empty at date {0}", date.ToString("dd-MM-yyyy")));
                    AddOLC(olcDates);
                    olcDates = new List<DateTime>();                  
                }

                if (selectedCourse != null)
                {
                    _logger.Debug(string.Format(_culture, "Selected course {0} is not null", selectedCourse.Code));
                    previousCourse = selectedCourse;
                }
                previousDayCourse = selectedCourse;
            }

            if (olcDates.Any())
            {
                _logger.Debug("Add last OLC to planning");
                AddOLC(olcDates);
            }
        }

        private void AddOLC(List<DateTime> dates)
        {
            _logger.Debug("Add OLC");
            decimal olcPrice = _managementPropertiesDataMapper.FindManagementProperties().OLCPrice;

            Course olc = new Course
            {
                Code = "OLC",
                Name = "OLC",
                CourseImplementations = new List<CourseImplementation>
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

            _coursePlanning.Courses.Add(olc);
            _logger.Debug(string.Format(_culture, "OLC added to course planning. {0} days with start day {1}", dates.Count, dates.First().ToString("dd-MM-yyyy")));
        }

        private static bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }
    }
}
