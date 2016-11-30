using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.infosupport.afstuderen.opleidingsplan.DAL.mapper;
using com.infosupport.afstuderen.opleidingsplan.model;
using System.Linq;

namespace com.infosupport.afstuderen.opleidingsplan.DAL.tests
{
    [TestClass]
    public class ProfileDataMapperTest
    {
        private string _profilePath;
        public ProfileDataMapperTest()
        {
            _profilePath = Configuration.GetConfiguration().ProfilePath;
        }

        [TestMethod]
        public void FindAll()
        {
            // Arrange
            IDataMapper<Profile> dataMapper = new ProfileDataMapper(_profilePath);

            // Act
            var result = dataMapper.FindAll();

            // Assert
            Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        public void Find()
        {
            // Arrange
            IDataMapper<Profile> dataMapper = new ProfileDataMapper(_profilePath);

            // Act
            var result = dataMapper.Find(profile => profile.Name == "NET_Developer").First();

            // Assert
            Assert.AreEqual(31, result.Courses.Count());
        }
    }
}
