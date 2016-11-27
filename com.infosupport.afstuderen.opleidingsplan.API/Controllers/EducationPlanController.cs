using AutoMapper;
using com.infosupport.afstuderen.opleidingsplan.api.Managers;
using com.infosupport.afstuderen.opleidingsplan.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace com.infosupport.afstuderen.opleidingsplan.api.Controllers
{
    [EnableCors("*", "*", "*")]
    public class EducationPlanController : ApiController
    {
        private IEducationPlanManager _educationPlanManager;

        public EducationPlanController()
        {
            _educationPlanManager = new EducationPlanManager();
        }

        // GET: api/EducationPlan
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/EducationPlan/5
        public EducationPlan Get(int id)
        {
            return new EducationPlan
            {
                Created = DateTime.Now,
                InPaymentFrom = DateTime.Now.AddDays(5),
                EmployableFrom = DateTime.Now.AddDays(90),
                KnowledgeOf = "MVC, DPAT, OOUML, SCRUMES",
                NameEmployee = "Pim Verheij",
                NameTeacher = "Felix Sedney",
                Profile = "NET_Developer",
                PlannedCourses = new List<EducationPlanCourse>()
                {
                    Mapper.Map<EducationPlanCourse>(new CourseManager().FindCourse("2NETARCH")),
                    Mapper.Map<EducationPlanCourse>(new CourseManager().FindCourse("ADCSB")),
                    Mapper.Map<EducationPlanCourse>(new CourseManager().FindCourse("CNET")),
                    Mapper.Map<EducationPlanCourse>(new CourseManager().FindCourse("HOCS")),
                    Mapper.Map<EducationPlanCourse>(new CourseManager().FindCourse("BLCNETIN")),
                    Mapper.Map<EducationPlanCourse>(new CourseManager().FindCourse("GIT")),
                },
                NotPlannedCourses = new List<EducationPlanCourse>()
                {
                    Mapper.Map<EducationPlanCourse>(new CourseManager().FindCourse("WPFDEV")),
                    Mapper.Map<EducationPlanCourse>(new CourseManager().FindCourse("WIN8SNEAK")),
                    Mapper.Map<EducationPlanCourse>(new CourseManager().FindCourse("BLNETFOUNB")),
                }
            };
        }

        // POST: api/EducationPlan
        public EducationPlan Post(string[] courses)
        {
            if (courses == null) courses = new string[0];

            var educationPlan = _educationPlanManager.GenerateEducationPlan(courses);
            return educationPlan;
        }

        // PUT: api/EducationPlan/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/EducationPlan/5
        public void Delete(int id)
        {
        }
    }
}
