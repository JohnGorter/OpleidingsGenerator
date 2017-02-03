using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InfoSupport.KC.OpleidingsplanGenerator.Api
{
    public sealed class AutoMapperConfiguration
    {
        private AutoMapperConfiguration() { }
        public static void Configure()
        {
            Mapper.Initialize(mapper =>
            {
                mapper.CreateMap<Integration.Course, OpleidingsplanGenerator.Models.Course>();
                mapper.CreateMap<Integration.Coursesummary, OpleidingsplanGenerator.Models.CourseSummary>();
                mapper.CreateMap<Integration.CourseImplementation, OpleidingsplanGenerator.Models.CourseImplementation>();
                mapper.CreateMap<Models.RestEducationPlan, Generator.EducationPlanData>();
            });
        }
    }
}