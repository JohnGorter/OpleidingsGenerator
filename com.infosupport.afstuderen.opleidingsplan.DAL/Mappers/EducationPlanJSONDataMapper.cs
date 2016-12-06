using System;
using com.infosupport.afstuderen.opleidingsplan.dal.mappers;
using com.infosupport.afstuderen.opleidingsplan.models;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace com.infosupport.afstuderen.opleidingsplan.dal.mappers
{
    public class EducationPlanJSONDataMapper : IEducationPlanDataMapper
    {
        private string _path;
        private string _updatedDirPath;

        public EducationPlanJSONDataMapper(string path, string updatedDirPath)
        {
            _path = path;
            _updatedDirPath = updatedDirPath;
        }

        public void Delete(EducationPlan educationPlan)
        {
            var educationPlans = GetAllEducationPlans();
            EducationPlan educationPlanToDelete = educationPlans.FirstOrDefault(ep => ep.Id == educationPlan.Id);

            if (educationPlanToDelete == null)
            {
                throw new ArgumentException(string.Format("No education plan found with id {0}", educationPlan.Id));
            }

            educationPlans.Remove(educationPlanToDelete);

            WriteEducationPlansToFile(educationPlans);
        }

        public IEnumerable<EducationPlan> Find(Func<EducationPlan, bool> predicate)
        {
            var educationPlans = GetAllEducationPlans();
            return educationPlans.Where(predicate);
        }

        public EducationPlan FindById(long id)
        {
            var educationPlan = GetAllEducationPlans().FirstOrDefault(ep => ep.Id == id);

            if (educationPlan == null)
            {
                throw new ArgumentException(string.Format("No education plan found with id {0}", id));
            }

            return educationPlan;
        }

        public void Insert(EducationPlan educationPlan)
        {
            var educationPlans = GetAllEducationPlans();
            educationPlan.Id = GenerateId(educationPlans);

            educationPlans.Add(educationPlan);
            WriteEducationPlansToFile(educationPlans);
        }

        public void Update(EducationPlan educationPlan)
        {
            var educationPlans = GetAllEducationPlans();
            EducationPlan educationPlanToUpdate = educationPlans.FirstOrDefault(ep => ep.Id == educationPlan.Id);

            if (educationPlanToUpdate == null)
            {
                throw new ArgumentException(string.Format("No education plan found with id {0}", educationPlan.Id));
            }

            int index = educationPlans.IndexOf(educationPlanToUpdate);
            educationPlans[index] = educationPlan;

            SaveNewUpdatedEducationPlan(educationPlanToUpdate);
            WriteEducationPlansToFile(educationPlans);
        }

        public IEnumerable<EducationPlan> FindAllUpdated()
        {
            throw new NotImplementedException();
        }

        private void SaveNewUpdatedEducationPlan(EducationPlan educationPlan)
        {
            if(!Directory.Exists(_updatedDirPath))
            {
                Directory.CreateDirectory(_updatedDirPath);
            }

            var convertedJson = JsonConvert.SerializeObject(educationPlan, Formatting.Indented);
            File.WriteAllText(Path.Combine(_updatedDirPath, educationPlan.Id.ToString() + ".json"), convertedJson);
        }

        private void WriteEducationPlansToFile(List<EducationPlan> educationPlan)
        {
            var convertedJson = JsonConvert.SerializeObject(educationPlan, Formatting.Indented);
            File.WriteAllText(_path, convertedJson);
        }
        private List<EducationPlan> GetAllEducationPlans()
        {
            string educationPlans = File.ReadAllText(_path);
            return JsonConvert.DeserializeObject<List<EducationPlan>>(educationPlans);
        }
        private int GenerateId(List<EducationPlan> allEducationPlans)
        {
            int newId = allEducationPlans.Max(educationPlan => educationPlan.Id) + 1;
            return newId;
        }
    }
}