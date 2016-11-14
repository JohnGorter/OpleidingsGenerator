﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.agent
{
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/InfoSupport.Trainingen")]
    [System.Xml.Serialization.XmlRoot(Namespace = "http://schemas.datacontract.org/2004/07/InfoSupport.Trainingen", IsNullable = false)]
    public class Course
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string SupplierName { get; set; }
        public string Duration { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string Prerequisites { get; set; }
        public CourseImplementation[] CourseImplementations { get; set; }
        public decimal Price { get; set; }
    }

    public class CourseImplementation
    {
        public string Location { get; set; }
        [System.Xml.Serialization.XmlArrayItem("DateTime", IsNullable = false)]
        public DateTime[] Days { get; set; }
    }
}
