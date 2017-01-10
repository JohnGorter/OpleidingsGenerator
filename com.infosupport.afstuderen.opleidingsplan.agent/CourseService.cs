﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private readonly Uri _baseUrl;

        public CourseService()
        {
            string infoSupportTrainingURL = IntegrationConfiguration.GetConfiguration().InfoSupportTrainingURL;
            _baseUrl = new Uri(infoSupportTrainingURL);
        }

        public Coursesummarycollection FindAllCourses()
        {
            Uri uri = new Uri(_baseUrl, "courses");

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
            Uri uri = new Uri(_baseUrl, "courses/" + courseCode);

            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            using (var stream = response.GetResponseStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Course));
                Course course = (Course)serializer.Deserialize(stream);

                return course;
            }
        }

        public IEnumerable<integration.Course> FindCourses(Collection<string> courseCodes)
        {
            if (courseCodes == null)
            {
                throw new ArgumentNullException("courseCodes");
            }

            List<integration.Course> courses = new List<integration.Course>();

            foreach (var courseCode in courseCodes)
            {
                courses.Add(FindCourse(courseCode));
            }

            return courses;
        }

    }
}
