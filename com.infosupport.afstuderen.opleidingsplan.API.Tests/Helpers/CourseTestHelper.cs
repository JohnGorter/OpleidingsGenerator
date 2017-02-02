using InfoSupport.KC.OpleidingsplanGenerator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace InfoSupport.KC.OpleidingsplanGenerator.Api.Tests.Helpers
{
    public class CourseTestHelper
    {
        internal Integration.Course GetDummyDataIntegrationCourse()
        {
            return new Integration.Course
            {
                Code = "POLDEVEL",
                Name = "Developing Polymer Elements",
                Description = "HTML5 is aan het ontwikkelen. Een standaard set aan technieken en technologieen maken web componenten mogelijk, echter is het schrijven ervan nog behoorlijk wat werk. Polymer abstraheert dit werk en voegt functionaliteit toe zoals data-binding in een kleine intuitieve bibliotheek. In deze training leert u het maken van componenten die in combinatie gebruikt kunnen worden voor het bouwen van progressieve web applicaties.",
                Duration = "3 Dagen",
                CourseImplementations = GetDummyDataIntegrationCourseImplementations(),
                Price = 1725.00M,
                SupplierName = "Info Support",
            };
        }

        internal OpleidingsplanGenerator.Models.Course GetDummyDataModelCourse()
        {
            return new OpleidingsplanGenerator.Models.Course
            {
                Code = "POLDEVEL",
                Name = "Developing Polymer Elements",
                Description = "HTML5 is aan het ontwikkelen. Een standaard set aan technieken en technologieen maken web componenten mogelijk, echter is het schrijven ervan nog behoorlijk wat werk. Polymer abstraheert dit werk en voegt functionaliteit toe zoals data-binding in een kleine intuitieve bibliotheek. In deze training leert u het maken van componenten die in combinatie gebruikt kunnen worden voor het bouwen van progressieve web applicaties.",
                Duration = "3 Dagen",
                CourseImplementations = GetDummyDataModelCourseImplementations(),
                Price = 1725.00M,
                SupplierName = "Info Support",
            };
        }

        internal Integration.Coursesummarycollection GetDummyDataIntegrationCourses()
        {
            return new Integration.Coursesummarycollection
            {
                Coursesummary = new Collection<Integration.Coursesummary>()
                {
                    new Integration.Coursesummary
                    {
                        Code = "2NETARCH",
                        Name = ".NET for Architects and Project Managers",
                        Suppliername = "Info Support",
                    },
                    new Integration.Coursesummary
                    {
                        Code = "ADCSB",
                        Name = "Advanced C#",
                        Suppliername = "Info Support",
                    },
                    new Integration.Coursesummary
                    {
                        Code = "ADMTFS",
                        Name = "Administering Team Foundation Server",
                        Suppliername = "Accentient",
                    },
                }
            };
        }

        internal CoursePriority GetDummyCourse()
        {
            return new CoursePriority
            {
                Id = 1,
                Code = "WINDEV",
                Priority = 3,
                ProfileId = 1,
            };
        }

        private Collection<Integration.CourseImplementation> GetDummyDataIntegrationCourseImplementations()
        {
            return new Collection<Integration.CourseImplementation>
            {
                new Integration.CourseImplementation
                {
                    Days = new Collection<DateTime>
                    {
                        new DateTime(2017, 1, 16),
                        new DateTime(2017, 1, 17),
                        new DateTime(2017, 1, 18),
                    },
                    Location = "Veenendaal",
                },
                new Integration.CourseImplementation
                {
                    Days = new Collection<DateTime>
                    {
                        new DateTime(2017, 4, 3),
                        new DateTime(2017, 4, 4),
                        new DateTime(2017, 4, 5),

                    },
                    Location = "Veenendaal",
                }
            };
        }

        private IEnumerable<OpleidingsplanGenerator.Models.CourseImplementation> GetDummyDataModelCourseImplementations()
        {
            return new List<OpleidingsplanGenerator.Models.CourseImplementation>()
            {
                new OpleidingsplanGenerator.Models.CourseImplementation
                {
                    Days = new List<DateTime>()
                    {
                        new DateTime(2017, 16, 1),
                        new DateTime(2017, 17, 1),
                        new DateTime(2017, 18, 1),
                    },
                    Location = "Veenendaal",
                },
                new OpleidingsplanGenerator.Models.CourseImplementation
                {
                    Days = new List<DateTime>()
                    {
                        new DateTime(2017, 3, 4),
                        new DateTime(2017, 4, 4),
                        new DateTime(2017, 5, 4),

                    },
                    Location = "Veenendaal",
                }
            };
        }
    }
}