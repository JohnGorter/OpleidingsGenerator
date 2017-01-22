using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.infosupport.afstuderen.opleidingsplan.integration.tests
{
    [TestClass]
    public class ConfigurationTest
    {
        [TestMethod]
        public void GetInfoSupportTrainingURLFromConfig()
        {
            //Arrange
            IntegrationConfiguration config = IntegrationConfiguration.Configuration;

            //Act
            var result = config.InfoSupportTrainingURL;
            result = config.InfoSupportTrainingURL;

            //Assert
            Assert.AreEqual("http://services.infosupport.com/ISTraining.External/v2/nl/", result);
        }
    }
}
