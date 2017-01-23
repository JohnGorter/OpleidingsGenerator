using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace com.infosupport.afstuderen.opleidingsplan.api.models
{
    public class RestEducationPlan
    {
        public long EducationPlanId { get; set; }
        public Collection<RestEducationPlanCourse> Courses { get; set; }
        public DateTime InPaymentFrom { get; set; }
        public DateTime EmployableFrom { get; set; }
        public string NameEmployee { get; set; }
        public string NameTeacher { get; set; }
        public string KnowledgeOf { get; set; }
        public int ProfileId { get; set; }
        public Collection<DateTime> BlockedDates { get; set; }
    }

    public class RestEducationPlanCourse
    {
        public string Code { get; set; }
        public string Commentary { get; set; }
        public int Priority { get; set; }
    }
}