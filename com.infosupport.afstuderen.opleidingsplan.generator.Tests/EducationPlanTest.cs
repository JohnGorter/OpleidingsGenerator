using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.infosupport.afstuderen.opleidingsplan.model;

namespace com.infosupport.afstuderen.opleidingsplan.generator.Tests
{
    [TestClass]
    public class EducationPlanTest
    {

        [TestMethod]
        public void AddEducationPlanDataViaCreatorTest()
        {
            // Arrange
            EducationplanCreator creator = new EducationplanCreator();
            EducationPlanData data = DummyData.GetEducationPlanData();

            // Act
            creator.AddEducationPlanData(data);

            // Assert
            Assert.AreEqual(new DateTime(2016, 11, 15), creator.EducationPlan.Created);
            Assert.AreEqual(new DateTime(2016, 11, 21), creator.EducationPlan.InPaymentFrom);
            Assert.AreEqual(new DateTime(2017, 1, 30), creator.EducationPlan.EmployableFrom);
            Assert.AreEqual("MVC, DPAT, OOUML, SCRUMES", creator.EducationPlan.KnowledgeOf);
            Assert.AreEqual("Pim Verheij", creator.EducationPlan.NameEmployee);
            Assert.AreEqual("Felix Sedney", creator.EducationPlan.NameTeacher);
            Assert.AreEqual("NET_Developer", creator.EducationPlan.Profile);
        }
    }
}
