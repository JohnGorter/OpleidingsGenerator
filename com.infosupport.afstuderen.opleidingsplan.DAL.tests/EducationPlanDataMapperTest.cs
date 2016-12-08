using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.infosupport.afstuderen.opleidingsplan.dal.mappers;
using com.infosupport.afstuderen.opleidingsplan.models;
using com.infosupport.afstuderen.opleidingsplan.dal.tests.helpers;
using System.IO;
using System.Linq;

namespace com.infosupport.afstuderen.opleidingsplan.dal.tests
{
    [TestClass]
    public class EducationPlanDataMapperTest : EducationPlanDataMapperTestHelper
    {
        private string _educationPlanPath;
        private string _updatedDirPath;

        public EducationPlanDataMapperTest()
        {
            _educationPlanPath = DALConfiguration.GetConfiguration().EducationPlanPath;
            _updatedDirPath = DALConfiguration.GetConfiguration().EducationPlanUpdatedPath;
        }

        [TestInitialize]
        public void Initialize()
        {
            var originalEducationPlans = File.ReadAllText("../../Data/EducationPlansOriginal.json");
            File.WriteAllText(_educationPlanPath, originalEducationPlans);

            if(Directory.Exists(_updatedDirPath))
            {
                Directory.Delete(_updatedDirPath, true);
                Directory.CreateDirectory(_updatedDirPath);
            }
        }


        [TestMethod]
        public void Find_OneEducationPlanFound()
        {
            // Arrange
            IEducationPlanDataMapper dataMapper = new EducationPlanJsonDataMapper(_educationPlanPath, _updatedDirPath);

            // Act
            var result = dataMapper.Find(educationPlan => educationPlan.NameEmployee == "Alex Verbeek");

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void Find_NoPlanFound()
        {
            // Arrange
            IEducationPlanDataMapper dataMapper = new EducationPlanJsonDataMapper(_educationPlanPath, _updatedDirPath);

            // Act
            var result = dataMapper.Find(educationPlan => educationPlan.NameEmployee == "Bram Aarts");

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void Insert_EducationPlanInserted()
        {
            // Arrange
            IEducationPlanDataMapper dataMapper = new EducationPlanJsonDataMapper(_educationPlanPath, _updatedDirPath);
            EducationPlan educationPlan = GetDummyEducationPlan();

            // Act
            dataMapper.Insert(educationPlan);

            // Assert
            var result = dataMapper.Find(ep => ep.NameEmployee == "Pim Verheij");

            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void Delete_EducationPlanDeleted()
        {
            // Arrange
            IEducationPlanDataMapper dataMapper = new EducationPlanJsonDataMapper(_educationPlanPath, _updatedDirPath);
            EducationPlan educationPlan = new EducationPlan
            {
                Id = 1,
            };

            // Act
            dataMapper.Delete(educationPlan);

            // Assert
            var result = dataMapper.Find(ep => ep.NameEmployee == "Alex Verbeek");
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Delete_WithNotExistingEducationPlan_ExceptionThrowed()
        {
            // Arrange
            IEducationPlanDataMapper dataMapper = new EducationPlanJsonDataMapper(_educationPlanPath, _updatedDirPath);
            EducationPlan educationPlan = new EducationPlan
            {
                Id = 100,
            };

            // Act
            dataMapper.Delete(educationPlan);

            // Assert ArgumentException
        }

        [TestMethod]
        public void Update_EducationPlanUpdated()
        {
            // Arrange
            IEducationPlanDataMapper dataMapper = new EducationPlanJsonDataMapper(_educationPlanPath, _updatedDirPath);
            EducationPlan educationPlan = GetDummyEducationPlan();
            educationPlan.Id = 1;

            var educationPlanBeforeUpdate = dataMapper.FindById(1);
            Assert.AreEqual("Alex Verbeek", educationPlanBeforeUpdate.NameEmployee);


            // Act
            dataMapper.Update(educationPlan);

            // Assert
            var result = dataMapper.FindById(1);
            Assert.AreEqual("Pim Verheij", result.NameEmployee);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Update_WithNotExistingEducationPlan_ExceptionThrowed()
        {
            // Arrange
            IEducationPlanDataMapper dataMapper = new EducationPlanJsonDataMapper(_educationPlanPath, _updatedDirPath);
            EducationPlan educationPlan = GetDummyEducationPlan();
            educationPlan.Id = 100;

            // Act
            dataMapper.Update(educationPlan);

            // Assert ArgumentException
        }

        [TestMethod]
        public void Update_CheckNewUpdateFileWithOldData()
        {
            // Arrange
            IEducationPlanDataMapper dataMapper = new EducationPlanJsonDataMapper(_educationPlanPath, _updatedDirPath);
            EducationPlan educationPlan = GetDummyEducationPlan();
            educationPlan.Id = 1;

            // Act
            dataMapper.Update(educationPlan);

            // Assert
            Assert.IsTrue(File.Exists("../../Data/Updated/1.json"));
        }



        [TestMethod]
        public void Update_CheckNewUpdatedDirIsCreated()
        {
            // Arrange
            IEducationPlanDataMapper dataMapper = new EducationPlanJsonDataMapper(_educationPlanPath, _updatedDirPath);
            EducationPlan educationPlan = GetDummyEducationPlan();
            educationPlan.Id = 1;
            if (Directory.Exists(_updatedDirPath))
            {
                Directory.Delete(_updatedDirPath, true);
            }

            // Act
            dataMapper.Update(educationPlan);

            // Assert no exception thrown
        }

        [TestMethod]
        public void FindById_EducationPlanFound()
        {
            // Arrange
            IEducationPlanDataMapper dataMapper = new EducationPlanJsonDataMapper(_educationPlanPath, _updatedDirPath);

            // Act
            var result = dataMapper.FindById(1);

            // Assert
            Assert.AreEqual("Alex Verbeek", result.NameEmployee);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FindByIdWitNotExistingId_ExceptionThrowed()
        {
            // Arrange
            IEducationPlanDataMapper dataMapper = new EducationPlanJsonDataMapper(_educationPlanPath, _updatedDirPath);

            // Act
            var result = dataMapper.FindById(100);

            // Assert ArgumentException
        }

        [TestMethod]
        public void FindAllUpdated_TwoUpdatedEducationPlansFound()
        {
            // Arrange
            IEducationPlanDataMapper dataMapper = new EducationPlanJsonDataMapper(_educationPlanPath, _updatedDirPath);
            EducationPlan educationPlan = GetDummyEducationPlan();
            educationPlan.Id = 1;
            dataMapper.Update(educationPlan);
            educationPlan.Id = 2;
            dataMapper.Update(educationPlan);

            // Act
            var result = dataMapper.FindAllUpdated();

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(1, result.ElementAt(0).EducationPlanOld.Id);
            Assert.AreEqual(1, result.ElementAt(0).EducationPlanNew.Id);
            Assert.AreEqual("Alex Verbeek", result.ElementAt(0).EducationPlanOld.NameEmployee);
            Assert.AreEqual("Pim Verheij", result.ElementAt(0).EducationPlanNew.NameEmployee);
            Assert.AreEqual(2, result.ElementAt(1).EducationPlanOld.Id);
            Assert.AreEqual(2, result.ElementAt(1).EducationPlanNew.Id);
            Assert.AreEqual("Jan Verstegen", result.ElementAt(1).EducationPlanOld.NameEmployee);
            Assert.AreEqual("Pim Verheij", result.ElementAt(1).EducationPlanNew.NameEmployee);
        }
    }
}
