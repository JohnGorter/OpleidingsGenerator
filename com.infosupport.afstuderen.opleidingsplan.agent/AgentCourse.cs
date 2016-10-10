using InfoSupport.Trainingen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace com.infosupport.afstuderen.opleidingsplan.agent
{
    public class AgentCourse
    {

        private Uri baseUrl = new Uri("http://services.infosupport.com/ISTraining.External/v2/nl/");

        public AgentCourse(Stream stream)
        {

        }

        public AgentCourse()
        {
        }

        public Coursesummarycollection FindAllCourses()
        {
            Uri uri = new Uri(baseUrl, "courses");

            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            using (var stream = response.GetResponseStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Coursesummarycollection));
                Coursesummarycollection coursesummarycollection = (Coursesummarycollection)serializer.Deserialize(stream);

                return coursesummarycollection;
            }
        }

        public Course FindCourse(string courseCode)
        {
            Uri uri = new Uri(baseUrl, "courses/" + courseCode);

            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            using (var stream = response.GetResponseStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Course));
                Course course = (Course)serializer.Deserialize(stream);

                return course;
            }
        }

    }
}
