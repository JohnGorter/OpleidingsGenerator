using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using com.infosupport.afstuderen.opleidingsplan.dal.mappers;
using com.infosupport.afstuderen.opleidingsplan.models;
using System.Linq;

namespace com.infosupport.afstuderen.opleidingsplan.dal.tests
{
    [TestClass]
    public class CourseDataMapperTest
    {

        private string _profilePath;
        public CourseDataMapperTest()
        {
            _profilePath = DALConfiguration.GetConfiguration().ProfilePath;
        }

        [TestInitialize]
        public void Initialize()
        {
            var originalProfiles = File.ReadAllText("../../Data/ProfilesOriginal.json");
            File.WriteAllText(_profilePath, originalProfiles);
        }

        [TestMethod]
        public void Insert_NewCoursePriorityAdded()
        {
            // Arrange
            ICourseDataMapper dataMapper = new CourseJSONDataMapper(_profilePath);
            CoursePriority course = new CoursePriority
            {
                ProfileId = 1,
                Code = "WINDOWDEV",
                Priority = 5,
            };

            // Act
            dataMapper.Insert(course);

            // Assert
            IDataMapper<CourseProfile> dataMapperProfile = new ProfileJsonDataMapper(_profilePath);
            var profiles = dataMapperProfile.FindAll();

            Assert.AreEqual(3, profiles.Count());
            Assert.AreEqual(32, profiles.ElementAt(0).Courses.Count());
            Assert.AreEqual(30, profiles.ElementAt(1).Courses.Count());
            Assert.AreEqual(27, profiles.ElementAt(2).Courses.Count());
            Assert.AreEqual(5, profiles.ElementAt(0).Courses.First(c => c.Id == 90).Priority);
            Assert.AreEqual("WINDOWDEV", profiles.ElementAt(0).Courses.First(c => c.Id == 90).Code);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Insert_ExistingProfile_ExceptionThrowed()
        {
            // Arrange
            ICourseDataMapper dataMapper = new CourseJSONDataMapper(_profilePath);
            CoursePriority course = new CoursePriority
            {
                ProfileId = 1,
                Code = "NETFOUNB",
                Priority = 5,
            };

            // Act
            dataMapper.Insert(course);

            // Assert ArgumentException
        }

        [TestMethod]
        public void Update_CoursePriority()
        {
            // Arrange
            ICourseDataMapper dataMapper = new CourseJSONDataMapper(_profilePath);
            CoursePriority course = new CoursePriority
            {
                Id = 1,
                ProfileId = 1,
                Code = "WINDOWDEV",
                Priority = 10,
            };

            // Act
            dataMapper.Update(course);

            // Assert
            IDataMapper<CourseProfile> dataMapperProfile = new ProfileJsonDataMapper(_profilePath);
            var profiles = dataMapperProfile.FindAll();

            Assert.AreEqual(3, profiles.Count());
            Assert.AreEqual(31, profiles.ElementAt(0).Courses.Count());
            Assert.AreEqual(30, profiles.ElementAt(1).Courses.Count());
            Assert.AreEqual(27, profiles.ElementAt(2).Courses.Count());
            Assert.AreEqual(10, profiles.ElementAt(0).Courses.First(c => c.Id == 1).Priority);
            Assert.AreEqual("WINDOWDEV", profiles.ElementAt(0).Courses.First(c => c.Id == 1).Code);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Update_WithNotExistingCourse_ExceptionThrowed()
        {
            // Arrange
            ICourseDataMapper dataMapper = new CourseJSONDataMapper(_profilePath);
            CoursePriority course = new CoursePriority
            {
                Id = 99,
                ProfileId = 1,
                Code = "WINDOWDEV",
                Priority = 10,
            };

            // Act
            dataMapper.Update(course);

            // Assert ArgumentException
        }

        [TestMethod]
        public void Delete_CoursePriority()
        {
            // Arrange
            ICourseDataMapper dataMapper = new CourseJSONDataMapper(_profilePath);
            CoursePriority course = new CoursePriority
            {
                Id = 1,
                ProfileId = 1,
                Code = "WINDEV"
            };

            // Act
            dataMapper.Delete(course);

            // Assert
            IDataMapper<CourseProfile> dataMapperProfile = new ProfileJsonDataMapper(_profilePath);
            var profiles = dataMapperProfile.FindAll();

            Assert.AreEqual(3, profiles.Count());
            Assert.AreEqual(30, profiles.ElementAt(0).Courses.Count());
            Assert.AreEqual(30, profiles.ElementAt(1).Courses.Count());
            Assert.AreEqual(27, profiles.ElementAt(2).Courses.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Delete_WithNotExistingProfile_ExceptionThrowed()
        {
            // Arrange
            ICourseDataMapper dataMapper = new CourseJSONDataMapper(_profilePath);
            CoursePriority course = new CoursePriority
            {
                ProfileId = 4,
                Code = "WINDEV"
            };

            // Act
            dataMapper.Delete(course);

            // Assert ArgumentException
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Delete_WithNotExistingCourseInProfile_ExceptionThrowed()
        {
            // Arrange
            ICourseDataMapper dataMapper = new CourseJSONDataMapper(_profilePath);
            CoursePriority course = new CoursePriority
            {
                ProfileId = 1,
                Code = "NODOTNET"
            };

            // Act
            dataMapper.Delete(course);

            // Assert ArgumentException
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Delete_DataIsNull_ExceptionThrowed()
        {
            // Arrange
            ICourseDataMapper dataMapper = new CourseJSONDataMapper(_profilePath);

            // Act
            dataMapper.Delete(null);

            // Assert ArgumentNullException
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Insert_DataIsNull_ExceptionThrowed()
        {
            // Arrange
            ICourseDataMapper dataMapper = new CourseJSONDataMapper(_profilePath);

            // Act
            dataMapper.Insert(null);

            // Assert ArgumentNullException
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_DataIsNull_ExceptionThrowed()
        {
            // Arrange
            ICourseDataMapper dataMapper = new CourseJSONDataMapper(_profilePath);

            // Act
            dataMapper.Update(null);

            // Assert ArgumentNullException
        }
    }
}
