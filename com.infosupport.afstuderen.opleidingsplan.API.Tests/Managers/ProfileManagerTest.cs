using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using com.infosupport.afstuderen.opleidingsplan.dal.mappers;
using com.infosupport.afstuderen.opleidingsplan.models;
using com.infosupport.afstuderen.opleidingsplan.api.tests.helpers;
using com.infosupport.afstuderen.opleidingsplan.api.managers;
using System.Linq;

namespace com.infosupport.afstuderen.opleidingsplan.api.tests.managers
{
    [TestClass]
    public class ProfileManagerTest : ProfileTestHelper
    {
        [TestMethod]
        public void FindProfilesTest()
        {
            // Arrange
            var profileDataMapperMock = new Mock<IDataMapper<CourseProfile>>(MockBehavior.Strict);
            profileDataMapperMock.Setup(dataMapper => dataMapper.FindAll()).Returns(GetDummyDataProfiles());

            ProfileManager manager = new ProfileManager(profileDataMapperMock.Object);

            // Act
            var result = manager.FindProfiles();

            // Assert
            profileDataMapperMock.Verify(dataMapper => dataMapper.FindAll());
            Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        public void FindProfile_NET_Developer_31_CoursesTest()
        {
            // Arrange
            var profileDataMapperMock = new Mock<IDataMapper<CourseProfile>>(MockBehavior.Strict);
            profileDataMapperMock.Setup(dataMapper => dataMapper.Find(It.IsAny<Func<CourseProfile, bool>>())).Returns(GetDummyDataProfiles());

            ProfileManager manager = new ProfileManager(profileDataMapperMock.Object);

            // Act
            var result = manager.FindProfile("NET_Developer");

            // Assert
            profileDataMapperMock.Verify(dataMapper => dataMapper.Find(It.IsAny<Func<CourseProfile, bool>>()));
            Assert.AreEqual(31, result.Courses.Count());
        }

        [TestMethod]
        public void FindProfileById1_NET_Developer_31_CoursesTest()
        {
            // Arrange
            var profileDataMapperMock = new Mock<IDataMapper<CourseProfile>>(MockBehavior.Strict);
            profileDataMapperMock.Setup(dataMapper => dataMapper.FindById(1)).Returns(GetDummyDataProfiles().First());

            ProfileManager manager = new ProfileManager(profileDataMapperMock.Object);

            // Act
            var result = manager.FindProfileById(1);

            // Assert
            profileDataMapperMock.Verify(dataMapper => dataMapper.FindById(1));
            Assert.AreEqual(31, result.Courses.Count());
            Assert.AreEqual("NET_Developer", result.Name);
        }

        [TestMethod]
        public void Insert_NewCourseProfile()
        {
            // Arrange
            var profileDataMapperMock = new Mock<IDataMapper<CourseProfile>>(MockBehavior.Strict);
            profileDataMapperMock.Setup(dataMapper => dataMapper.Insert(It.IsAny<CourseProfile>()));

            ProfileManager manager = new ProfileManager(profileDataMapperMock.Object);

            // Act
            manager.Insert(GetDummyDataProfiles().First());

            // Assert
            profileDataMapperMock.Verify(dataMapper => dataMapper.Insert(It.IsAny<CourseProfile>()));
        }

        [TestMethod]
        public void Update_CourseProfile()
        {
            // Arrange
            var profileDataMapperMock = new Mock<IDataMapper<CourseProfile>>(MockBehavior.Strict);
            profileDataMapperMock.Setup(dataMapper => dataMapper.Update(It.IsAny<CourseProfile>()));

            ProfileManager manager = new ProfileManager(profileDataMapperMock.Object);

            // Act
            manager.Update(GetDummyDataProfiles().First());

            // Assert
            profileDataMapperMock.Verify(dataMapper => dataMapper.Update(It.IsAny<CourseProfile>()));
        }

        [TestMethod]
        public void Delete_CourseProfile()
        {
            // Arrange
            var profileDataMapperMock = new Mock<IDataMapper<CourseProfile>>(MockBehavior.Strict);
            profileDataMapperMock.Setup(dataMapper => dataMapper.Delete(It.IsAny<CourseProfile>()));

            ProfileManager manager = new ProfileManager(profileDataMapperMock.Object);

            // Act
            manager.Delete(GetDummyDataProfiles().First());

            // Assert
            profileDataMapperMock.Verify(dataMapper => dataMapper.Delete(It.IsAny<CourseProfile>()));
        }
    }
}
