using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSupport.KC.OpleidingsplanGenerator.Models
{
    public class EducationPlanCompareSummary
    {
        public long Id { get; set; }
        public string NameEmployee { get; set; }
        public DateTime Created { get; set; }
        public DateTime InPaymentFrom { get; set; }
        public string NameTeacher { get; set; }
        public string KnowledgeOf { get; set; }
        public string Profile { get; set; }
    }
}
