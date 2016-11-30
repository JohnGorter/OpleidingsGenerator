using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.infosupport.afstuderen.opleidingsplan.api.Models
{
    public class Profile
    {
        public string name { get; set; }
        public List<CoursePriority> courses { get; set; }
    }
    public class CoursePriority
    {
        public string code { get; set; }
        public string priority { get; set; }
    }
}