using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using com.infosupport.afstuderen.opleidingsplan.dal.mappers;
using com.infosupport.afstuderen.opleidingsplan.models;

namespace com.infosupport.afstuderen.opleidingsplan.dal.tests
{
    [TestClass]
    public class ManagementPropertiesDataMapperTest
    {
        private string _managementPropertiesPath;

        public ManagementPropertiesDataMapperTest()
        {
            _managementPropertiesPath = DALConfiguration.Configuration.ManagementPropertiesPath;
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
            IManagementPropertiesDataMapper dataMapper = new ManagementPropertiesJSONDataMapper(_managementPropertiesPath);
            ManagementProperties properties = new ManagementProperties
            {
                OLCPrice = 150,
                PeriodAfterLastCourseEmployableInDays = 2,
                PeriodBeforeStartNotifiable = 4,
                PeriodEducationPlanInDays = 100,
            };

            // Act
            dataMapper.Update(properties);

            // Assert
            var propertiesResult = dataMapper.FindManagementProperties();

            Assert.AreEqual(150, propertiesResult.OLCPrice);
            Assert.AreEqual(2, propertiesResult.PeriodAfterLastCourseEmployableInDays);
            Assert.AreEqual(4, propertiesResult.PeriodBeforeStartNotifiable);
            Assert.AreEqual(100, propertiesResult.PeriodEducationPlanInDays);
        }

        [TestMethod]
        public void FindManagementProperties_ManagementProperties()
        {
            // Arrange
            IManagementPropertiesDataMapper dataMapper = new ManagementPropertiesJSONDataMapper(_managementPropertiesPath);

            // Act
            var result = dataMapper.FindManagementProperties();

            // Assert
            Assert.AreEqual(125, result.OLCPrice);
            Assert.AreEqual(7, result.PeriodAfterLastCourseEmployableInDays);
            Assert.AreEqual(7, result.PeriodBeforeStartNotifiable);
            Assert.AreEqual(90, result.PeriodEducationPlanInDays);
        }
    }
}
