using System;
using com.infosupport.afstuderen.opleidingsplan.dal.mappers;
using com.infosupport.afstuderen.opleidingsplan.models;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using log4net;

namespace com.infosupport.afstuderen.opleidingsplan.dal.mappers
{
    public class EducationPlanJsonDataMapper : IEducationPlanDataMapper
    {
        private string _path;
        private string _updatedDirPath;
        private readonly CultureInfo _culture = new CultureInfo("nl-NL");
        private static ILog _logger = LogManager.GetLogger(typeof(EducationPlanJsonDataMapper));

        public EducationPlanJsonDataMapper(string path, string updatedDirPath)
        {
            _path = path;
            _updatedDirPath = updatedDirPath;
        }

        public void Delete(long id)
        {
            _logger.Debug(string.Format(_culture, "Delete education plan with id {0}", id));

            var educationPlans = GetAllEducationPlans();
            EducationPlan educationPlanToDelete = educationPlans.FirstOrDefault(ep => ep.Id == id);

            if (educationPlanToDelete == null)
            {
                string errorMessage = string.Format(_culture, "No education plan found with id {0}", id);
                _logger.Error("ArgumentException: " + errorMessage);
                throw new ArgumentException(errorMessage);
            }

            educationPlans.Remove(educationPlanToDelete);

            WriteEducationPlansToFile(educationPlans);
            _logger.Debug(string.Format(_culture, "Education plan deleted with id {0} and name employee {1}", educationPlanToDelete.Id, educationPlanToDelete.NameEmployee));
        }

        public IEnumerable<EducationPlan> Find(Func<EducationPlan, bool> predicate)
        {
            _logger.Debug(string.Format(_culture, "Find education plan"));
            var educationPlans = GetAllEducationPlans();
            return educationPlans.Where(predicate);
        }

        public EducationPlan FindById(long id)
        {
            _logger.Debug(string.Format(_culture, "Find education plan by id {0}", id));
            var educationPlan = GetAllEducationPlans().FirstOrDefault(ep => ep.Id == id);

            if (educationPlan == null)
            {
                string errorMessage = string.Format(_culture, "No education plan found with id {0}", id);
                _logger.Error("ArgumentException: " + errorMessage);
                throw new ArgumentException(errorMessage);
            }

            _logger.Debug(string.Format(_culture, "Education plan found by id {0} with employee name {1}", educationPlan.Id, educationPlan.EmployableFrom));
            return educationPlan;
        }

        public long Insert(EducationPlan educationPlan)
        {
            if (educationPlan == null)
            {
                _logger.Error("ArgumentNullException educationPlan");
                throw new ArgumentNullException("educationPlan");
            }

            _logger.Debug(string.Format(_culture, "Insert education plan with name employee {0}", educationPlan.NameEmployee));

            var educationPlans = GetAllEducationPlans();
            educationPlan.Id = GenerateId(educationPlans);

            educationPlans.Add(educationPlan);
            WriteEducationPlansToFile(educationPlans);

            _logger.Debug(string.Format(_culture, "Education plan inserted with name employee {0} and generated id {1}", educationPlan.NameEmployee, educationPlan.Id));
            return educationPlan.Id;
        }

        public long Update(EducationPlan educationPlan)
        {
            if (educationPlan == null)
            {
                _logger.Error("ArgumentNullException educationPlan");
                throw new ArgumentNullException("educationPlan");
            }

            _logger.Debug(string.Format(_culture, "Update education plan with name employee {0}", educationPlan.NameEmployee));

            var educationPlans = GetAllEducationPlans();
            EducationPlan educationPlanToUpdate = educationPlans.FirstOrDefault(ep => ep.Id == educationPlan.Id);

            if (educationPlanToUpdate == null)
            {
                string errorMessage = string.Format(_culture, "No education plan found with id {0}", educationPlan.Id);
                _logger.Error("ArgumentException: " + errorMessage);
                throw new ArgumentException(errorMessage);
            }

            int index = educationPlans.IndexOf(educationPlanToUpdate);
            educationPlans[index] = educationPlan;

            SaveNewUpdatedEducationPlan(educationPlanToUpdate);
            WriteEducationPlansToFile(educationPlans);

            _logger.Debug(string.Format(_culture, "Education plan updated with name employee {0} and generated id {1}", educationPlan.NameEmployee, educationPlan.Id));

            return educationPlan.Id;
        }

        public IEnumerable<EducationPlanCompare> FindAllUpdated()
        {
            List<EducationPlanCompare> educationPlansCompareList = new List<EducationPlanCompare>();
            var educationPlans = GetAllEducationPlans();
            _logger.Debug(string.Format(_culture, "Find all updated: {0} education plans found", educationPlans.Count));

            foreach (var file in Directory.GetFiles(_updatedDirPath))
            {
                try
                {
                    string educationPlanJSON = File.ReadAllText(file);
                    var oldEducationPlan = JsonConvert.DeserializeObject<EducationPlan>(educationPlanJSON);
                    var newEducationPlan = educationPlans.FirstOrDefault(ep => ep.Id == oldEducationPlan.Id);
                    educationPlansCompareList.Add(new EducationPlanCompare
                    {
                        EducationPlanNew = newEducationPlan,
                        EducationPlanOld = oldEducationPlan,
                    });
                }
                catch (FileNotFoundException ex)
                {
                    _logger.Error(string.Format(_culture, "File {0} with updated education plan not found", file), ex);
                    throw;
                }
                catch (JsonReaderException ex)
                {
                    _logger.Error(string.Format(_culture, "Couldn't deserialize updated education plan in file {0}", file), ex);
                    throw;
                }
            }

            return educationPlansCompareList;
        }

        private void SaveNewUpdatedEducationPlan(EducationPlan educationPlan)
        {
            if(!Directory.Exists(_updatedDirPath))
            {
                _logger.Debug(string.Format(_culture, "Save new updated education plan. Directory does not exists with path {0}", _updatedDirPath));
                Directory.CreateDirectory(_updatedDirPath);
            }

            var convertedJson = JsonConvert.SerializeObject(educationPlan, Formatting.Indented);

            try
            {
                File.WriteAllText(Path.Combine(_updatedDirPath, educationPlan.Id + ".json"), convertedJson);
                _logger.Debug(string.Format(_culture, "Saved new updated education plan to directory {0} with id {1} and employee {2}", _updatedDirPath, educationPlan.Id, educationPlan.NameEmployee));
            }
            catch (DirectoryNotFoundException ex)
            {
                _logger.Error(string.Format(_culture, "Directory {0} to write updated educationplan not found", _updatedDirPath), ex);
                throw;
            }
        }

        private void WriteEducationPlansToFile(List<EducationPlan> educationPlan)
        {
            var convertedJson = JsonConvert.SerializeObject(educationPlan, Formatting.Indented);

            try
            {
                File.WriteAllText(_path, convertedJson);
                _logger.Debug(string.Format(_culture, "Saved all education plans to path {0}", _path));
            }
            catch (FileNotFoundException ex)
            {
                _logger.Error(string.Format(_culture, "File {0} to write updated educationplan not found", _path), ex);
                throw;
            }
        }
        private List<EducationPlan> GetAllEducationPlans()
        {
            try
            {
                string educationPlans = File.ReadAllText(_path);
                _logger.Debug(string.Format(_culture, "Get all educationplans with path {0}", _path));
                return JsonConvert.DeserializeObject<List<EducationPlan>>(educationPlans);
            }
            catch (FileNotFoundException ex)
            {
                _logger.Error(string.Format(_culture, "File {0} to get all education plans not found", _path), ex);
                throw;
            }
            catch (JsonReaderException ex)
            {
                _logger.Error(string.Format(_culture, "Couldn't deserialize education plan from path {0}", _path), ex);
                throw;
            }
        }

        private long GenerateId(List<EducationPlan> allEducationPlans)
        {
            _logger.Debug(string.Format(_culture, "Generate id for education plan"));

            long newId = 1;

            if (allEducationPlans.Any())
            {
                newId = allEducationPlans.Max(educationPlan => educationPlan.Id) + 1;
            }

            _logger.Debug(string.Format(_culture, "Generated id for education plan {0}", newId));
            return newId;
        }
    }
}