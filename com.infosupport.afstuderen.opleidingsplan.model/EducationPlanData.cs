using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.model
{
    public class EducationPlanData
    {
        public DateTime Created { get; set; }
        public DateTime InPaymentFrom { get; set; }
        public DateTime EmployableFrom { get; set; }
        public string NameEmployee { get; set; }
        public string NameTeacher { get; set; }
        public string KnowledgeOf { get; set; }
        public string Profile { get; set; }
    }
}
