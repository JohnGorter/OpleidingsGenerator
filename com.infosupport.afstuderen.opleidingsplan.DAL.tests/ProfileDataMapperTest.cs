using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.infosupport.afstuderen.opleidingsplan.dal.mappers;
using com.infosupport.afstuderen.opleidingsplan.models;
using System.Linq;
using System.IO;

namespace com.infosupport.afstuderen.opleidingsplan.dal.tests
{
    [TestClass]
    public class ProfileDataMapperTest
    {
        private string _profilePath;
        public ProfileDataMapperTest()
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
        public void FindAll_ThreeProfilesFound()
        {
            // Arrange
            IDataMapper<CourseProfile> dataMapper = new ProfileJsonDataMapper(_profilePath);

            // Act
            var result = dataMapper.FindAll();

            // Assert
            Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        public void Find_ProfileFound()
        {
            // Arrange
            IDataMapper<CourseProfile> dataMapper = new ProfileJsonDataMapper(_profilePath);

            // Act
            var result = dataMapper.Find(profile => profile.Name == "NET_Developer").First();

            // Assert
            Assert.AreEqual(31, result.Courses.Count());
        }

        [TestMethod]
        public void FindById_ProfileFound()
        {
            // Arrange
            IDataMapper<CourseProfile> dataMapper = new ProfileJsonDataMapper(_profilePath);

            // Act
            var result = dataMapper.FindById(1);

            // Assert
            Assert.AreEqual("NET_Developer", result.Name);
            Assert.AreEqual(31, result.Courses.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FindById_WitNotExistingId_ExceptionThrowed()
        {
            // Arrange
            IDataMapper<CourseProfile> dataMapper = new ProfileJsonDataMapper(_profilePath);

            // Act
            var result = dataMapper.FindById(100);

            // Assert ArgumentException
        }

        [TestMethod]
        public void Insert_NewProfileAdded()
        {
            // Arrange
            IDataMapper<CourseProfile> dataMapper = new ProfileJsonDataMapper(_profilePath);
            CourseProfile profile = new CourseProfile
            {
                Name = "FrondEnd_Developer"
            };

            // Act
            dataMapper.Insert(profile);

            // Assert
            var result = dataMapper.FindAll();
            Assert.AreEqual(4, result.Count());
            Assert.AreEqual("NET_Developer", result.ElementAt(0).Name);
            Assert.AreEqual(31, result.ElementAt(0).Courses.Count());
            Assert.AreEqual("Developer_Mobile", result.ElementAt(1).Name);
            Assert.AreEqual(30, result.ElementAt(1).Courses.Count());
            Assert.AreEqual("JAVA_Developer", result.ElementAt(2).Name);
            Assert.AreEqual(27, result.ElementAt(2).Courses.Count());
            Assert.AreEqual("FrondEnd_Developer", result.ElementAt(3).Name);
            Assert.AreEqual(0, result.ElementAt(3).Courses.Count());
            Assert.AreEqual(4, result.ElementAt(3).Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Insert_ExistingProfile_ExceptionThrowed()
        {
            // Arrange
            IDataMapper<CourseProfile> dataMapper = new ProfileJsonDataMapper(_profilePath);
            CourseProfile profile = new CourseProfile
            {
                Name = "NET_Developer"
            };

            // Act
            dataMapper.Insert(profile);

            // Assert ArgumentException
        }

        [TestMethod]
        public void Update_ProfileUpdated()
        {
            // Arrange
            IDataMapper<CourseProfile> dataMapper = new ProfileJsonDataMapper(_profilePath);
            CourseProfile profile = new CourseProfile
            {
                Id = 1,
                Name = "DOT_NET_Developer"
            };

            // Act
            dataMapper.Update(profile);

            // Assert
            var result = dataMapper.FindAll();
            Assert.AreEqual(3, result.Count());
            Assert.AreEqual("DOT_NET_Developer", result.ElementAt(0).Name);
            Assert.AreEqual(31, result.ElementAt(0).Courses.Count());
            Assert.AreEqual("Developer_Mobile", result.ElementAt(1).Name);
            Assert.AreEqual(30, result.ElementAt(1).Courses.Count());
            Assert.AreEqual("JAVA_Developer", result.ElementAt(2).Name);
            Assert.AreEqual(27, result.ElementAt(2).Courses.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Update_WithNotExistingProfile_ExceptionThrowed()
        {
            // Arrange
            IDataMapper<CourseProfile> dataMapper = new ProfileJsonDataMapper(_profilePath);
            CourseProfile profile = new CourseProfile
            {
                Id = 100,
                Name = "DOT_NET_Developer"
            };

            // Act
            dataMapper.Update(profile);

            // Assert ArgumentException
        }


        [TestMethod]
        public void Delete_ProfileDeleted()
        {
            // Arrange
            IDataMapper<CourseProfile> dataMapper = new ProfileJsonDataMapper(_profilePath);
            CourseProfile profile = new CourseProfile
            {
                Id = 1,
                Name = "NET_Developer"
            };

            // Act
            dataMapper.Delete(profile);

            // Assert
            var result = dataMapper.FindAll();
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Developer_Mobile", result.ElementAt(0).Name);
            Assert.AreEqual(30, result.ElementAt(0).Courses.Count());
            Assert.AreEqual("JAVA_Developer", result.ElementAt(1).Name);
            Assert.AreEqual(27, result.ElementAt(1).Courses.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Delete_WithNotExistingProfile_ExceptionThrowed()
        {
            // Arrange
            IDataMapper<CourseProfile> dataMapper = new ProfileJsonDataMapper(_profilePath);
            CourseProfile profile = new CourseProfile
            {
                Id = 100,
                Name = "NET_Developer"
            };

            // Act
            dataMapper.Delete(profile);

            // Assert ArgumentException
        }
    }
}
