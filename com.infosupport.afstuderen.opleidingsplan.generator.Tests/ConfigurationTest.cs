using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InfoSupport.KC.OpleidingsplanGenerator.Generator.Tests
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
