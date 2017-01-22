using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace com.infosupport.afstuderen.opleidingsplan.integration
{
    public class CourseService : ICourseService
    {
        private readonly CultureInfo _culture = new CultureInfo("nl-NL");
        private static ILog _logger = LogManager.GetLogger(typeof(CourseService));
        private readonly Uri _baseUrl;

        public CourseService()
        {
            string infoSupportTrainingURL = IntegrationConfiguration.GetConfiguration().InfoSupportTrainingURL;
            _baseUrl = new Uri(infoSupportTrainingURL);
        }

        public Coursesummarycollection FindAllCourses()
        {
            Uri uri = new Uri(_baseUrl, "courses");
            _logger.Debug(string.Format(_culture, "FindAllCourses from url {0}", uri));

            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            using (var stream = response.GetResponseStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Coursesummarycollection));
                Coursesummarycollection coursesummarycollection = (Coursesummarycollection)serializer.Deserialize(stream);

                _logger.Debug(string.Format(_culture, "return coursesummarycollection in FindAllCourses from url {0}", uri));
                return coursesummarycollection;
            }
        }

        public Course FindCourse(string courseCode)
        {
            Uri uri = new Uri(_baseUrl, "courses/" + courseCode);
            _logger.Debug(string.Format(_culture, "FindCourse with code {0} from url {1}", courseCode, uri));

            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            using (var stream = response.GetResponseStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Course));
                Course course = (Course)serializer.Deserialize(stream);

                _logger.Debug(string.Format(_culture, "return course in FindCourse with code {0} from url {1}", courseCode, uri));
                return course;
            }
        }

        public IEnumerable<integration.Course> FindCourses(Collection<string> courseCodes)
        {
            _logger.Debug("FindCourses");

            if (courseCodes == null)
            {
                _logger.Error("ArgumentNullException: courseCodes");
                throw new ArgumentNullException("courseCodes");
            }

            List<integration.Course> courses = new List<integration.Course>();

            foreach (var courseCode in courseCodes)
            {
                _logger.Debug(string.Format(_culture, "FindCourse {0} in FindCourses", courseCode));
                courses.Add(FindCourse(courseCode));
            }

            return courses;
        }

    }
}
