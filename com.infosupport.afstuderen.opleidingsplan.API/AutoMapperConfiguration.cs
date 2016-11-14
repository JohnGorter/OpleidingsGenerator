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
                mapper.CreateMap<agent.Course, model.Course>();
                mapper.CreateMap<agent.CourseImplementation, model.CourseImplementation>();

                //ONLY FOR DEMO
                mapper.CreateMap<agent.Course, model.EducationPlanCourse>()
                    .ForMember(dest => dest.Date, opt => opt.MapFrom(src => RandomDay()))
                    .ForMember(dest => dest.Days, opt => opt.MapFrom(src => null != src.CourseImplementations.FirstOrDefault() ? src.CourseImplementations.FirstOrDefault().Days.Count() : 4))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));
            });
        }

        private static Random gen = new Random();
        static DateTime RandomDay()
        {
            DateTime start = DateTime.Now;
            return start.AddDays(gen.Next(150));
        }
    }
}