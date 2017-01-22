using System;
using System.Collections.Generic;
using com.infosupport.afstuderen.opleidingsplan.models;
using Novacode;
using System.Linq;
using System.Globalization;
using com.infosupport.afstuderen.opleidingsplan.dal.mappers;
using System.IO;
using log4net;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public class EducationPlanConverter : IEducationPlanConverter
    {
        private static ILog _logger = LogManager.GetLogger(typeof(EducationPlanConverter));
        private DocX _document;
        private readonly IManagementPropertiesDataMapper managementPropertiesDataMapper;
        private readonly CultureInfo _culture = new CultureInfo("nl-NL");
        private readonly string _dateFromat = "dd-MM-yyyy";
        private EducationPlan _educationPlan;
        private readonly string _path;

        public EducationPlanConverter(string managementPropertiesPath, string path)
        {
            managementPropertiesDataMapper = new ManagementPropertiesJSONDataMapper(managementPropertiesPath);
            _path = path;
        }

        public string GenerateWord(EducationPlan educationPlan)
        {
            _logger.Debug(string.Format(_culture, "GenerateWord"));

            if (!Directory.Exists(_path))
            {
                _logger.Debug(string.Format(_culture, "Path {0} for word files doesn't exists", _path));
                Directory.CreateDirectory(_path);
            }

            string fileName = @"\Opleidingsplan-" + educationPlan.NameEmployee + "-" + educationPlan.Created.ToString(_dateFromat) + ".docx";
            _document = DocX.Create(_path + fileName);

            _educationPlan = educationPlan;
            GenerateHeader();
            InsertNewLine();
            InsertEducationPlanTable(_educationPlan.PlannedCourses.ToList(), true);
            InsertNewLine();
            _document.InsertParagraph("Op termijn te volgen:");
            InsertEducationPlanTable(_educationPlan.NotPlannedCourses.ToList(), false);
            InsertFooter();

            _document.Save();

            return fileName;
        }

        private void InsertFooter()
        {
            _logger.Debug(string.Format(_culture, "InsertFooter"));
            string footer = managementPropertiesDataMapper.FindManagementProperties().Footer;
            footer = footer.Replace("<Naam>", _educationPlan.NameEmployee);
            _document.InsertParagraph(footer);
        }

        private void InsertEducationPlanTable(List<EducationPlanCourse> courses, bool planned)
        {
            _logger.Debug(string.Format(_culture, "InsertEducationPlanTable"));
            Table table = _document.AddTable(courses.Count + 3, 7);
            var firstRow = table.Rows[0];
            firstRow.Cells[0].Paragraphs.First().Append("Week");
            firstRow.Cells[1].Paragraphs.First().Append("Datum");
            firstRow.Cells[2].Paragraphs.First().Append("Cursusnaam");
            firstRow.Cells[3].Paragraphs.First().Append("Dagen");
            firstRow.Cells[4].Paragraphs.First().Append("Opmerkingen");
            firstRow.Cells[5].Paragraphs.First().Append("Prijs per Training");
            firstRow.Cells[6].Paragraphs.First().Append("Prijs incl. personeelskorting");

            
            for (int i = 0; i < courses.Count; i++)
            {
                var course = courses.ElementAt(i);
                var row = table.Rows[i + 1];

                if (course.Week > 0)
                {
                    row.Cells[0].Paragraphs.First().Append(course.Week.ToString());
                }

                if (course.Date.HasValue)
                {
                    row.Cells[1].Paragraphs.First().Append(course.Date.Value.ToString(_dateFromat));
                }

                row.Cells[2].Paragraphs.First().Append(course.Name);
                row.Cells[3].Paragraphs.First().Append(course.Days.ToString());
                row.Cells[4].Paragraphs.First().Append(course.Commentary);
                row.Cells[5].Paragraphs.First().Append("€ " + course.Price.ToString("N", _culture));
                row.Cells[6].Paragraphs.First().Append("€ " + course.PriceWithDiscount.ToString("N", _culture));
            }

            var lastRow = table.Rows[courses.Count + 2];
            lastRow.Cells[4].Paragraphs.First().Append("Totaal").Bold();
            if (planned)
            {
                lastRow.Cells[5].Paragraphs.First().Append("€ " + _educationPlan.PlannedCoursesTotalPrice.ToString("N", _culture)).Bold();
                lastRow.Cells[6].Paragraphs.First().Append("€ " + _educationPlan.PlannedCoursesTotalPriceWithDiscount.ToString("N", _culture)).Bold();
            }
            else
            {
                lastRow.Cells[5].Paragraphs.First().Append("€ " + _educationPlan.NotPlannedCoursesTotalPrice.ToString("N", _culture)).Bold();
                lastRow.Cells[6].Paragraphs.First().Append("€ " + _educationPlan.NotPlannedCoursesTotalPriceWithDiscount.ToString("N", _culture)).Bold();
            }
            _document.InsertTable(table);
        }

        private void GenerateHeader()
        {
            _logger.Debug(string.Format(_culture, "GenerateHeader"));
            CreateNameValue("Opleidingsgesprek:\t", _educationPlan.NameEmployee);
            CreateNameValue("Datum:\t\t\t", _educationPlan.Created.ToString(_dateFromat));
            CreateNameValue("Datum in dienst:\t", _educationPlan.InPaymentFrom.ToString(_dateFromat));
            CreateNameValue("Begeleider KC:\t\t", _educationPlan.NameTeacher);
            CreateNameValue("Inzetbaar vanaf:\t", _educationPlan.EmployableFrom.ToString(_dateFromat));
            CreateNameValue("Reeds kennis van:\t", _educationPlan.KnowledgeOf);
            CreateNameValue("Geblokkeerde datums:\t", _educationPlan.BlockedDates);
        }


        private void InsertNewLine()
        {
            _document.InsertParagraph("\n");
        }

        private void CreateNameValue(string name, List<DateTime> blockedDates)
        {
            _logger.Debug(string.Format(_culture, "CreateNameValue with name {0}", name));
            if (blockedDates != null && blockedDates.Any())
            {
                var p = _document.InsertParagraph(name);
                string dates = String.Join(", ", blockedDates.Select(date => date.ToString(_dateFromat)));
                p.Append(dates);
            }
        }

        private void CreateNameValue(string name, string value)
        {
            _logger.Debug(string.Format(_culture, "CreateNameValue with name {0}", name));
            var p = _document.InsertParagraph(name);
            p.Append(value);
        }
    }
}