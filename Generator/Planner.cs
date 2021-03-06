﻿using InfoSupport.KC.OpleidingsplanGenerator.Dal.Mappers;
using InfoSupport.KC.OpleidingsplanGenerator.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSupport.KC.OpleidingsplanGenerator.Generator
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
                _startDate = value.Date;
            }
        }

        public DateTime? EndDate { get; set; }

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

        public void PlanCoursesWithOlc(IEnumerable<Models.Course> coursesToPlan, IEnumerable<Models.PinnedCourseImplementation> ImplementationConstraints)
        {
            _logger.Debug("Plan courses with OLC");
            PlanCourses(coursesToPlan, ImplementationConstraints);
            ApplyOlc();
        }

        public void PlanCoursesWithOlcInOldEducationPlan(IEnumerable<Models.Course> coursesToPlan, EducationPlan oldEducationplan, IEnumerable<Models.PinnedCourseImplementation> ImplementationConstraints)
        {
            var passedCourses = oldEducationplan.PlannedCourses.Where(course => course.Date <= DateTime.Today.Date);
            var courses = coursesToPlan.Where(course => !passedCourses.Select(passedCourse => passedCourse.Code).Contains(course.Code));
            StartDate = oldEducationplan?.InPaymentFrom ?? DateTime.Now.Date;

            PlanCourses(courses, ImplementationConstraints);

            foreach (var passedCourse in passedCourses)
            {
                var days = new List<DateTime>();
                days.Add(passedCourse.Date.Value);

                for (int i = 0; i < passedCourse.Days; i++)
                {
                    days.Add(days.Last().AddDays(1));
                }

                _coursePlanning.Courses.Add(new Course
                {
                    Code = passedCourse.Code,
                    Commentary = passedCourse.Commentary,
                    CourseImplementations = new List<CourseImplementation>
                    {
                        new CourseImplementation
                        {
                            Status = Status.PLANNED,
                            Days = days,
                        }
                    },
                    Duration = passedCourse.Days,
                    Name = passedCourse.Name,
                    Price = passedCourse.Price,
                });
            }

            ApplyOlc();

        }

        public void PlanCourses(IEnumerable<Models.Course> coursesToPlan, IEnumerable<Models.PinnedCourseImplementation> implementationConstraints)
        {
            _logger.Debug("Plan courses");
            var coursesToPlanOrdered = coursesToPlan.OrderBy(course => course.Priority).ThenByDescending(course => course.Days).ToList();

            // set the constraints first
            foreach (var impcon in implementationConstraints) {
                var orig = coursesToPlan.FirstOrDefault(cc => cc.Code.Equals(impcon.CourseCode));
                var course = (Generator.Course)orig;
                course.MarkAllImplementations(Status.NOTPLANNED);
                var imp = course.getImplementationByWeekNumber(impcon.CourseImplementationWeek);
               // var imp = course.CourseImplementations.Where(ci => ci.StartDay..Equals(impcon.CourseImplementationWeek)).FirstOrDefault();
                if (imp != null)
                {
                    imp.Status = Status.PLANNED;
                    imp.Pinned = true;

                    _coursePlanning.Courses.Add(course);
                    // remove the pinned course from the planning... it has been planned!
                    coursesToPlanOrdered.Remove(orig);
                }
            }
            
            foreach (var courseToPlan in coursesToPlanOrdered)
            {
                var course = (Generator.Course)courseToPlan;
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

        private Generator.Course RemoveBlockedAndOutsidePeriodImplementations(Generator.Course course)
        {
            _logger.Debug(string.Format(_culture, "Remove blocked implementations and implementations outside the period from course {0}", course.Code));

            if (BlockedDates != null)
            {
                _logger.Debug(string.Format(_culture, "Remove {0} blocked implementations from course {1}", BlockedDates.Count, course.Code));

                int periodEducationPlanDays = _managementPropertiesDataMapper.FindManagementProperties().PeriodEducationPlanInDays;
                int daysAfterLastCourseEmployable = _managementPropertiesDataMapper.FindManagementProperties().PeriodAfterLastCourseEmployableInDays;

                DateTime endDate = StartDate.GetEndDay(periodEducationPlanDays);

                if (EndDate != null)
                {
                    endDate = EndDate.Value.AddDays(-daysAfterLastCourseEmployable);
                    _logger.Debug(string.Format(_culture, "Period for education plan is {0} days", endDate - StartDate));
                }
                else
                {
                    _logger.Debug(string.Format(_culture, "Period for education plan is {0} days", periodEducationPlanDays));
                }

                var blockedImplementations = course.CourseImplementations.Where(ci => ci.StartDay < StartDate || ci.StartDay.GetEndDay(ci.Days.Count()) > endDate || ci.Days.Any(day => BlockedDates.Contains(day)));

                _logger.Debug("Set status of blocked implementations and implementations outside the period on unplannable");
                foreach (var implementation in blockedImplementations)
                {
                    implementation.Status = Status.UNPLANNABLE;
                }
            }

            return course;
        }

        private void MarkCourseImplementations(Generator.Course course, IEnumerable<Generator.Course> knownCourses)
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

        private void ApplyOlc()
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
                        AddOlc(olcDates);
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

                bool newCourse = previousCourse != selectedCourse && previousDayCourse == null && selectedCourse != null;
                if ((newCourse || BlockedDates.Contains(date)) && olcDates.Any())
                {
                    _logger.Debug(string.Format(_culture, "OLC dates is not empty at date {0}", date.ToString("dd-MM-yyyy")));
                    AddOlc(olcDates);
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
                AddOlc(olcDates);
            }
        }

        private void AddOlc(List<DateTime> dates)
        {
            _logger.Debug("Add OLC");
            decimal olcPrice = _managementPropertiesDataMapper.FindManagementProperties().OlcPrice;

            Course olc = new Course
            {
                Code = "OLC" + (_coursePlanning.Courses.Count(course => course.Name == "OLC") +1),
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

        public void AddModules(IEnumerable<Module> modules)
        {
            foreach (var module in modules)
            {
                _coursePlanning.Courses.Add(new Course
                {
                    Code = "OLC",
                    Commentary = module.Commentary,
                    CourseImplementations = new List<CourseImplementation>(),
                    Duration = module.Days,
                    Name = module.Name,
                    Price = module.Price,      
                });
            }
        }
    }
}
