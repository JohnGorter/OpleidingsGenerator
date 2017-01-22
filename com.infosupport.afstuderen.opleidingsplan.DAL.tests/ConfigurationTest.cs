using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.infosupport.afstuderen.opleidingsplan.dal.tests
{
    [TestClass]
    public class ConfigurationTest
    {
        [TestMethod]
        public void GetProfilePathFromConfig()
        {
            //Arrange
            DalConfiguration config = DalConfiguration.Configuration;

            //Act
            var result = config.ProfilePath;

            //Assert
            Assert.AreEqual("../../Data/Profiles.json", result);
        }

        [TestMethod]
        public void GetEducationPlanPathFromConfig()
        {
            //Arrange
            DalConfiguration config = DalConfiguration.Configuration;

            //Act
            var result = config.EducationPlanPath;

            //Assert
            Assert.AreEqual("../../Data/EducationPlans.json", result);
        }

        [TestMethod]
        public void GetManagementPropertiesPathFromConfig()
        {
            //Arrange
            DalConfiguration config = DalConfiguration.Configuration;

            //Act
            var result = config.ManagementPropertiesPath;

            //Assert
            Assert.AreEqual("../../Data/ManagementProperties.json", result);
        }

        [TestMethod]
        public void GetEducationPlanUpdatedPathFromConfig()
        {
            //Arrange
            DalConfiguration config = DalConfiguration.Configuration;

            //Act
            var result = config.EducationPlanUpdatedPath;

            //Assert
            Assert.AreEqual("../../Data/Updated", result);
        }
    }
}
