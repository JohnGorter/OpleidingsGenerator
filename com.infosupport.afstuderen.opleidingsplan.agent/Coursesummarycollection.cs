using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace com.infosupport.afstuderen.opleidingsplan.integration
{
    [XmlRoot(ElementName = "CourseSummaryCollection", Namespace = "http://schemas.datacontract.org/2004/07/InfoSupport.Trainingen")]
    public class Coursesummarycollection
    {
        [XmlElement(ElementName = "CourseSummary", Namespace = "http://schemas.datacontract.org/2004/07/InfoSupport.Trainingen")]
        public List<Coursesummary> Coursesummary { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "i", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string I { get; set; }
    }

    [XmlRoot(ElementName = "CourseSummary", Namespace = "http://schemas.datacontract.org/2004/07/InfoSupport.Trainingen")]
    public class Coursesummary
    {
        [XmlElement(ElementName = "Code", Namespace = "http://schemas.datacontract.org/2004/07/InfoSupport.Trainingen")]
        public string Code { get; set; }
        [XmlElement(ElementName = "Name", Namespace = "http://schemas.datacontract.org/2004/07/InfoSupport.Trainingen")]
        public string Name { get; set; }
        [XmlElement(ElementName = "SupplierCode", Namespace = "http://schemas.datacontract.org/2004/07/InfoSupport.Trainingen")]
        public Suppliercode Suppliercode { get; set; }
        [XmlElement(ElementName = "SupplierName", Namespace = "http://schemas.datacontract.org/2004/07/InfoSupport.Trainingen")]
        public string Suppliername { get; set; }
    }

    [XmlRoot(ElementName = "SupplierCode", Namespace = "http://schemas.datacontract.org/2004/07/InfoSupport.Trainingen")]
    public class Suppliercode
    {
        [XmlElement(ElementName = "SupplierName", Namespace = "http://schemas.datacontract.org/2004/07/InfoSupport.Trainingen")]
        public string Suppliername { get; set; }
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }
}
