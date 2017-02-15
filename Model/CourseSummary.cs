using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSupport.KC.OpleidingsplanGenerator.Models
{
    public class CourseSummary
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Suppliername { get; set; }

        public override string ToString()
        {
            return string.Format("Code: {0}, Name: {1}, Suppliername: {2}", Code, Name, Suppliername);
        }
    }
}
