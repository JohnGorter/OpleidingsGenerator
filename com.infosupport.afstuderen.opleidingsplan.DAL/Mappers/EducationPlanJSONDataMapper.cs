using System;
using com.infosupport.afstuderen.opleidingsplan.dal.mappers;
using com.infosupport.afstuderen.opleidingsplan.models;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace com.infosupport.afstuderen.opleidingsplan.dal.mappers
{
    public class EducationPlanJsonDataMapper : IEducationPlanDataMapper
    {
        private string _path;
        private string _updatedDirPath;
        private readonly CultureInfo _culture = new CultureInfo("nl-NL");

        public EducationPlanJsonDataMapper(string path, string updatedDirPath)
        {
            _path = path;
            _updatedDirPath = updatedDirPath;
        }

        public void Delete(EducationPlan educationPlan)
        {
            if (educationPlan == null)
            {
                throw new ArgumentNullException("educationPlan");
            }

            var educationPlans = GetAllEducationPlans();
            EducationPlan educationPlanToDelete = educationPlans.FirstOrDefault(ep => ep.Id == educationPlan.Id);

            if (educationPlanToDelete == null)
            {
                throw new ArgumentException(string.Format(_culture, "No education plan found with id {0}", educationPlan.Id));
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
                throw new ArgumentException(string.Format(_culture, "No education plan found with id {0}", id));
            }

            return educationPlan;
        }

        public long Insert(EducationPlan educationPlan)
        {
            if (educationPlan == null)
            {
                throw new ArgumentNullException("educationPlan");
            }

            var educationPlans = GetAllEducationPlans();
            educationPlan.Id = GenerateId(educationPlans);

            educationPlans.Add(educationPlan);
            WriteEducationPlansToFile(educationPlans);
            return educationPlan.Id;
        }

        public void Update(EducationPlan educationPlan)
        {
            if (educationPlan == null)
            {
                throw new ArgumentNullException("educationPlan");
            }

            var educationPlans = GetAllEducationPlans();
            EducationPlan educationPlanToUpdate = educationPlans.FirstOrDefault(ep => ep.Id == educationPlan.Id);

            if (educationPlanToUpdate == null)
            {
                throw new ArgumentException(string.Format(_culture, "No education plan found with id {0}", educationPlan.Id));
            }

            int index = educationPlans.IndexOf(educationPlanToUpdate);
            educationPlans[index] = educationPlan;

            SaveNewUpdatedEducationPlan(educationPlanToUpdate);
            WriteEducationPlansToFile(educationPlans);
        }

        public IEnumerable<EducationPlanCompare> FindAllUpdated()
        {
            List<EducationPlanCompare> educationPlansCompareList = new List<EducationPlanCompare>();
            var educationPlans = GetAllEducationPlans();

            foreach (var file in Directory.GetFiles(_updatedDirPath))
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

            return educationPlansCompareList;
        }

        private void SaveNewUpdatedEducationPlan(EducationPlan educationPlan)
        {
            if(!Directory.Exists(_updatedDirPath))
            {
                Directory.CreateDirectory(_updatedDirPath);
            }

            var convertedJson = JsonConvert.SerializeObject(educationPlan, Formatting.Indented);
            File.WriteAllText(Path.Combine(_updatedDirPath, educationPlan.Id + ".json"), convertedJson);
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
        private static int GenerateId(List<EducationPlan> allEducationPlans)
        {
            if(!allEducationPlans.Any())
            {
                return 1;
            }

            int newId = allEducationPlans.Max(educationPlan => educationPlan.Id) + 1;
            return newId;
        }
    }
}