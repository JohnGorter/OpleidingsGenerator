using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.infosupport.afstuderen.opleidingsplan.api
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(mapper =>
            {
                mapper.CreateMap<integration.Course, model.Course>();
                mapper.CreateMap<integration.Coursesummary, model.CourseSummary>();
                mapper.CreateMap<integration.CourseImplementation, model.CourseImplementation>();
                mapper.CreateMap<Models.RestEducationPlan, generator.EducationPlanData>();
            });
        }
    }
}