using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace com.infosupport.afstuderen.opleidingsplan.agent
{
    public class EducationPlanAgent
    {

        public CoursesEducationPlan FindAllCourses(string educationPlanName)
        {
            using (FileStream stream = new FileStream(@"..\..\EducationPlan.xml", FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Courses));
                Courses courses = (Courses)serializer.Deserialize(stream);


                return courses.EducationPlan.FirstOrDefault(educationPlan => educationPlan.Name == educationPlanName);

            }
        }
    }
}
