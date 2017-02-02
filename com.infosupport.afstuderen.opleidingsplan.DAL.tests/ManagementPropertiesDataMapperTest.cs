using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using InfoSupport.KC.OpleidingsplanGenerator.Dal.Mappers;
using InfoSupport.KC.OpleidingsplanGenerator.Models;
using Newtonsoft.Json;

namespace InfoSupport.KC.OpleidingsplanGenerator.Dal.Tests
{
    [TestClass]
    public class ManagementPropertiesDataMapperTest
    {
        private string _managementPropertiesPath;

        public ManagementPropertiesDataMapperTest()
        {
            _managementPropertiesPath = DalConfiguration.Configuration.ManagementPropertiesPath;
        }

        [TestInitialize]
        public void Initialize()
        {
            var originalManagementProperties = File.ReadAllText("../../Data/ManagementPropertiesOriginal.json");
            File.WriteAllText(_managementPropertiesPath, originalManagementProperties);
        }


        [TestMethod]
        public void Update_ManagementProperties()
        {
            // Arrange
            IManagementPropertiesDataMapper dataMapper = new ManagementPropertiesJsonDataMapper(_managementPropertiesPath);
            ManagementProperties properties = new ManagementProperties
            {
                OlcPrice = 150,
                PeriodAfterLastCourseEmployableInDays = 2,
                PeriodBeforeStartNotifiable = 4,
                PeriodEducationPlanInDays = 100,
                Footer = "new footer"
            };

            // Act
            dataMapper.Update(properties);

            // Assert
            var propertiesResult = dataMapper.FindManagementProperties();

            Assert.AreEqual(150, propertiesResult.OlcPrice);
            Assert.AreEqual(2, propertiesResult.PeriodAfterLastCourseEmployableInDays);
            Assert.AreEqual(4, propertiesResult.PeriodBeforeStartNotifiable);
            Assert.AreEqual(100, propertiesResult.PeriodEducationPlanInDays);
            Assert.AreEqual("new footer", propertiesResult.Footer);
        }

        [TestMethod]
        public void FindManagementProperties_ManagementProperties()
        {
            // Arrange
            IManagementPropertiesDataMapper dataMapper = new ManagementPropertiesJsonDataMapper(_managementPropertiesPath);

            // Act
            var result = dataMapper.FindManagementProperties();

            // Assert
            Assert.AreEqual(125, result.OlcPrice);
            Assert.AreEqual(7, result.PeriodAfterLastCourseEmployableInDays);
            Assert.AreEqual(7, result.PeriodBeforeStartNotifiable);
            Assert.AreEqual(90, result.PeriodEducationPlanInDays);
            Assert.AreEqual("Conform de arbeidsvoorwaarden die van toepassing zijn op de arbeidsovereenkomst tussen <Naam> en Info Support is de studiekostenregeling bijlage 6 van toepassing. Concreet betekent dit dat iedere genoten opleiding in 36 maanden wordt afgeschreven ingaande de einddatum van de  opleiding.", result.Footer);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void Update_WithNotExistingPath_ManagementProperties_ExceptionThrowed()
        {
            // Arrange
            IManagementPropertiesDataMapper dataMapper = new ManagementPropertiesJsonDataMapper("noPath");
            ManagementProperties properties = new ManagementProperties
            {
                OlcPrice = 150,
                PeriodAfterLastCourseEmployableInDays = 2,
                PeriodBeforeStartNotifiable = 4,
                PeriodEducationPlanInDays = 100,
                Footer = "new footer",
                StaffDiscount = 80,
            };

            // Act
            dataMapper.Update(properties);

            // Assert FileNotFoundException
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void FindManagementProperties_WithNotExistingPath_ManagementProperties_ExceptionThrowed()
        {
            // Arrange
            IManagementPropertiesDataMapper dataMapper = new ManagementPropertiesJsonDataMapper("noPath");

            // Act
            var result = dataMapper.FindManagementProperties();

            // Assert FileNotFoundException
        }

        [TestMethod]
        [ExpectedException(typeof(JsonSerializationException))]
        public void FindManagementProperties_WithCorruptedFile_ManagementProperties_ExceptionThrowed()
        {
            // Arrange
            IManagementPropertiesDataMapper dataMapper = new ManagementPropertiesJsonDataMapper("../../Data/corrupted.json");

            // Act
            var result = dataMapper.FindManagementProperties();

            // Assert JsonSerializationException
        }
    }
}
