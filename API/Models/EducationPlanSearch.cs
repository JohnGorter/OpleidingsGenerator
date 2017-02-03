using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InfoSupport.KC.OpleidingsplanGenerator.Api.Models
{
    public class EducationPlanSearch
    {
        public string Name { get; set; }
        public DateTime? Date { get; set; }
    }
}