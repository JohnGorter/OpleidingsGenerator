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
        private Stream _courseStream;

        public AgentCourse(Stream stream)
        {
            _courseStream = stream;
        }

        public AgentCourse()
        {
            string requestUrl = "http://services.infosupport.com/ISTraining.External/v2/nl/courses";
            HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            _courseStream = response.GetResponseStream();
        }

        public Coursesummarycollection FindAllCourses()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Coursesummarycollection));
            Coursesummarycollection coursesummarycollection = (Coursesummarycollection)serializer.Deserialize(_courseStream);

            return coursesummarycollection;
        }

    }
}
