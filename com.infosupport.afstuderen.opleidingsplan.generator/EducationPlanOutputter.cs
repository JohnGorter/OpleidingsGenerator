﻿using com.infosupport.afstuderen.opleidingsplan.dal.mappers;
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
            if (educationPlanData == null) throw new ArgumentNullException("educationPlanData");

            List<EducationPlanCourse> educationPlannedCourses = GetPlannedEducationPlanCourses(_planner.GetPlannedCourses().ToList()).OrderBy(course => course.Date).ToList();
            List<EducationPlanCourse> educationNotPlannedCourses = GetNotPlannedEducationPlanCourses(_planner.GetNotPlannedCourses().ToList(), educationPlannedCourses).OrderBy(course => course.Date).ToList();

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
                Created = educationPlanData.Created,
                PlannedCourses = educationPlannedCourses,
                NotPlannedCourses = educationNotPlannedCourses,
                InPaymentFrom = educationPlanData.InPaymentFrom,
                EmployableFrom = dateEmployableFrom,
                Profile = educationPlanData.Profile,
                NameEmployee = educationPlanData.NameEmployee,
                NameTeacher = educationPlanData.NameTeacher,
                KnowledgeOf = educationPlanData.KnowledgeOf,
                BlockedDates = educationPlanData.BlockedDates,
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
