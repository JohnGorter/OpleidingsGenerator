using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.infosupport.afstuderen.opleidingsplan.api.models
{
    public class RestEducationPlan
    {
        public string[] Courses { get; set; }
        public DateTime InPaymentFrom { get; set; }
        public DateTime EmployableFrom { get; set; }
        public string NameEmployee { get; set; }
        public string NameTeacher { get; set; }
        public string KnowledgeOf { get; set; }
        public int ProfileId { get; set; }
        public List<DateTime> BlockedDates { get; set; }
    }
}