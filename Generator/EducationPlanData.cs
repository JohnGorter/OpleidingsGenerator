using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSupport.KC.OpleidingsplanGenerator.Generator
{
    public class EducationPlanData
    {
        public DateTime Created { get; set; }
        public DateTime InPaymentFrom { get; set; }
        public DateTime? EmployableFrom { get; set; }
        public string NameEmployee { get; set; }
        public string NameTeacher { get; set; }
        public string KnowledgeOf { get; set; }
        public string Profile { get; set; }
        public int ProfileId { get; set; }
        public long EducationPlanId { get; set; }
        public List<DateTime> BlockedDates { get; set; }
    }
}
