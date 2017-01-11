using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.infosupport.afstuderen.opleidingsplan.generator.tests.helpers;

namespace com.infosupport.afstuderen.opleidingsplan.generator.tests
{
    [TestClass]
    public class EducationPlanConverterTest : EducationPlanTestHelper
    {

        [TestMethod]
        public void GenerateWord_EducationPlanConverter()
        {
            // Arrange
            EducationPlanConverter converter = new EducationPlanConverter(GetDummyEducationPlan(), "../../Data/ManagementProperties.json", "../../Data/");
            // Act
            converter.GenerateWord();

            // Assert

        }
    }

}
