using System;
using System.Collections.Generic;
using InfoSupport.KC.OpleidingsplanGenerator.Models;
using Novacode;
using System.Linq;
using System.Globalization;
using InfoSupport.KC.OpleidingsplanGenerator.Dal.Mappers;
using System.IO;
using log4net;
using System.Drawing;

namespace InfoSupport.KC.OpleidingsplanGenerator.Generator
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
        private readonly FontFamily _fontFamily = new FontFamily("Verdana");
        private readonly int _fontSizeHeader = 10;
        private readonly int _fontSizeTable = 8;

        public EducationPlanConverter(string managementPropertiesPath, string path)
        {
            managementPropertiesDataMapper = new ManagementPropertiesJsonDataMapper(managementPropertiesPath);
            _path = path;
        }

        public string GenerateWord(EducationPlan educationPlan)
        {
            _logger.Debug("GenerateWord");

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
            _document.InsertParagraph("Op termijn te volgen").Bold().UnderlineStyle(UnderlineStyle.singleLine).FontSize(_fontSizeTable).Font(_fontFamily);
            InsertNewLine();
            InsertEducationPlanTable(_educationPlan.NotPlannedCourses.ToList(), false);
            InsertFooter();

            _document.Save();

            return fileName;
        }

        private void InsertFooter()
        {
            _logger.Debug("InsertFooter");
            string footer = managementPropertiesDataMapper.FindManagementProperties().Footer;
            footer = footer.Replace("<Naam>", _educationPlan.NameEmployee);
            _document.InsertParagraph(footer).FontSize(_fontSizeTable).Font(_fontFamily);
        }

        private void InsertEducationPlanTable(List<EducationPlanCourse> courses, bool planned)
        {
            _logger.Debug("InsertEducationPlanTable");
            Table table = _document.AddTable(courses.Count + 3, 7);
            var firstRow = table.Rows[0];

            for (int i = 0; i < firstRow.Cells.Count; i++)
            {
                firstRow.Cells[i].FillColor = ColorTranslator.FromHtml("#99CCFF");
            }

            firstRow.Cells[0].Paragraphs.First().Append("Week").Bold().FontSize(_fontSizeTable).Font(_fontFamily);
            firstRow.Cells[1].Paragraphs.First().Append("Datum").Bold().FontSize(_fontSizeTable).Font(_fontFamily);
            firstRow.Cells[2].Paragraphs.First().Append("Cursusnaam").Bold().FontSize(_fontSizeTable).Font(_fontFamily);
            firstRow.Cells[3].Paragraphs.First().Append("Dagen").Bold().FontSize(_fontSizeTable).Font(_fontFamily);
            firstRow.Cells[4].Paragraphs.First().Append("Opmerkingen").Bold().FontSize(_fontSizeTable).Font(_fontFamily);
            firstRow.Cells[5].Paragraphs.First().Append("Prijs per Training").Bold().FontSize(_fontSizeTable).Font(_fontFamily);
            firstRow.Cells[6].Paragraphs.First().Append("Prijs incl. personeelskorting").Bold().FontSize(_fontSizeTable).Font(_fontFamily);


            for (int i = 0; i < courses.Count; i++)
            {
                var course = courses.ElementAt(i);
                course.StaffDiscountInPercentage = managementPropertiesDataMapper.FindManagementProperties().StaffDiscount;

                if(course.Code.StartsWith("OLC"))
                {
                    course.StaffDiscountInPercentage = 100;
                }

                var row = table.Rows[i + 1];

                if (course.Week > 0)
                {
                    row.Cells[0].Paragraphs.First().Append(course.Week.ToString()).FontSize(_fontSizeTable).Font(_fontFamily);
                }

                if (course.Date.HasValue)
                {
                    row.Cells[1].Paragraphs.First().Append(course.Date.Value.ToString(_dateFromat)).FontSize(_fontSizeTable).Font(_fontFamily);
                }
                else
                {
                    row.Cells[1].Paragraphs.First().Append("ntb").FontSize(_fontSizeTable).Font(_fontFamily);
                }

                row.Cells[2].Paragraphs.First().Append(course.Name).FontSize(_fontSizeTable).Font(_fontFamily);
                row.Cells[3].Paragraphs.First().Append(course.Days.ToString()).FontSize(_fontSizeTable).Font(_fontFamily);
                row.Cells[4].Paragraphs.First().Append(course.Commentary).FontSize(_fontSizeTable).Font(_fontFamily);
                row.Cells[5].Paragraphs.First().Append("€ " + course.Price.ToString("N", _culture)).FontSize(_fontSizeTable).Font(_fontFamily);
                row.Cells[6].Paragraphs.First().Append("€ " + course.PriceWithDiscount.ToString("N", _culture)).FontSize(_fontSizeTable).Font(_fontFamily);
            }

            var lastRow = table.Rows[courses.Count + 2];
            lastRow.Cells[4].Paragraphs.First().Append("Totaal").Bold().FontSize(_fontSizeTable).Font(_fontFamily);
            if (planned)
            {
                lastRow.Cells[5].Paragraphs.First().Append("€ " + _educationPlan.PlannedCoursesTotalPrice.ToString("N", _culture)).Bold().FontSize(_fontSizeTable).Font(_fontFamily);
                lastRow.Cells[6].Paragraphs.First().Append("€ " + _educationPlan.PlannedCoursesTotalPriceWithDiscount.ToString("N", _culture)).Bold().FontSize(_fontSizeTable).Font(_fontFamily);
            }
            else
            {
                lastRow.Cells[5].Paragraphs.First().Append("€ " + _educationPlan.NotPlannedCoursesTotalPrice.ToString("N", _culture)).Bold().FontSize(_fontSizeTable).Font(_fontFamily);
                lastRow.Cells[6].Paragraphs.First().Append("€ " + _educationPlan.NotPlannedCoursesTotalPriceWithDiscount.ToString("N", _culture)).Bold().FontSize(_fontSizeTable).Font(_fontFamily);
            }
            _document.InsertTable(table);
        }

        private void GenerateHeader()
        {
            _logger.Debug("GenerateHeader");
            CreateNameValue("Opleidingsgesprek:\t\t", _educationPlan.NameEmployee);
            CreateNameValue("Datum:\t\t\t", _educationPlan.Created.ToString(_dateFromat));
            CreateNameValue("Datum in dienst:\t\t", _educationPlan.InPaymentFrom.ToString(_dateFromat));
            CreateNameValue("Begeleider KC:\t\t", _educationPlan.NameTeacher);
            CreateNameValue("Inzetbaar vanaf:\t\t", _educationPlan.EmployableFrom.ToString(_dateFromat));
            CreateNameValue("Reeds kennis van:\t\t", _educationPlan.KnowledgeOf);
            CreateNameValue("Geblokkeerde datums:\t", _educationPlan.BlockedDates);
        }


        private void InsertNewLine()
        {
            _document.InsertParagraph("\n").FontSize(_fontSizeTable);
        }

        private void CreateNameValue(string name, List<DateTime> blockedDates)
        {
            _logger.Debug(string.Format(_culture, "CreateNameValue with name {0}", name));
            if (blockedDates != null && blockedDates.Any())
            {
                var p = _document.InsertParagraph(name).FontSize(_fontSizeHeader).Font(_fontFamily);
                string dates = String.Join(", ", blockedDates.Select(date => date.ToString(_dateFromat)));
                p.Append(dates).FontSize(_fontSizeHeader).Font(_fontFamily);
            }
        }

        private void CreateNameValue(string name, string value)
        {
            _logger.Debug(string.Format(_culture, "CreateNameValue with name {0}", name));
            var p = _document.InsertParagraph(name).FontSize(_fontSizeHeader).Font(_fontFamily);
            p.Append(value).FontSize(_fontSizeHeader).Font(_fontFamily);
        }
    }
}