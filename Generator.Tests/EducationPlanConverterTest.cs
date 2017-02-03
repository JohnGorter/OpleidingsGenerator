using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InfoSupport.KC.OpleidingsplanGenerator.Generator.Tests.Helpers;

namespace InfoSupport.KC.OpleidingsplanGenerator.Generator.Tests
{
    [TestClass]
    public class EducationPlanConverterTest : EducationPlanTestHelper
    {

        [TestMethod]
        public void GenerateWord_EducationPlanConverter()
        {
            // Arrange
            EducationPlanConverter converter = new EducationPlanConverter("../../Data/ManagementProperties.json", "../../Data/");
            // Act
            converter.GenerateWord(GetDummyEducationPlan());

            // Assert

        }
    }

}
