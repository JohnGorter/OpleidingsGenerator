using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.infosupport.afstuderen.opleidingsplan.generator.tests
{
    [TestClass]
    public class ConfigurationTest
    {
        [TestMethod]
        public void GetEducationPlanFileDirPathFromConfig()
        {
            //Arrange
            GeneratorConfiguration config = GeneratorConfiguration.Configuration;

            //Act
            var result = config.EducationPlanFileDirPath;
            result = config.EducationPlanFileDirPath;

            //Assert
            Assert.AreEqual("Data/EducationPlanFiles", result);
        }
    }
}
