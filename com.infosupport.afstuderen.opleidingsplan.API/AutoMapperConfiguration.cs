using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.infosupport.afstuderen.opleidingsplan.api
{
    public sealed class AutoMapperConfiguration
    {
        private AutoMapperConfiguration() { }
        public static void Configure()
        {
            Mapper.Initialize(mapper =>
            {
                mapper.CreateMap<integration.Course, opleidingsplan.models.Course>();
                mapper.CreateMap<integration.Coursesummary, opleidingsplan.models.CourseSummary>();
                mapper.CreateMap<integration.CourseImplementation, opleidingsplan.models.CourseImplementation>();
                mapper.CreateMap<models.RestEducationPlan, generator.EducationPlanData>();
            });
        }
    }
}