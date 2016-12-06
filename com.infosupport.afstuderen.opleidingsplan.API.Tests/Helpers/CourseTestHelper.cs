﻿using System;
using System.Collections.Generic;

namespace com.infosupport.afstuderen.opleidingsplan.api.tests.helpers
{
    public class CourseTestHelper
    {
        internal static integration.Course GetDummyDataIntegrationCourse()
        {
            return new integration.Course
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

        internal static opleidingsplan.models.Course GetDummyDataModelCourse()
        {
            return new opleidingsplan.models.Course
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

        internal static integration.Coursesummarycollection GetDummyDataIntegrationCourses()
        {
            return new integration.Coursesummarycollection
            {
                Coursesummary = new List<integration.Coursesummary>()
                {
                    new integration.Coursesummary
                    {
                        Code = "2NETARCH",
                        Name = ".NET for Architects and Project Managers",
                        Suppliername = "Info Support",
                    },
                    new integration.Coursesummary
                    {
                        Code = "ADCSB",
                        Name = "Advanced C#",
                        Suppliername = "Info Support",
                    },
                    new integration.Coursesummary
                    {
                        Code = "ADMTFS",
                        Name = "Administering Team Foundation Server",
                        Suppliername = "Accentient",
                    },
                }
            };
        }

        private static integration.CourseImplementation[] GetDummyDataIntegrationCourseImplementations()
        {
            return new integration.CourseImplementation[]
            {
                new integration.CourseImplementation
                {
                    Days = new DateTime[]
                    {
                        new DateTime(2017, 1, 16),
                        new DateTime(2017, 1, 17),
                        new DateTime(2017, 1, 18),
                    },
                    Location = "Veenendaal",
                },
                new integration.CourseImplementation
                {
                    Days = new DateTime[]
                    {
                        new DateTime(2017, 4, 3),
                        new DateTime(2017, 4, 4),
                        new DateTime(2017, 4, 5),

                    },
                    Location = "Veenendaal",
                }
            };
        }

        private static IEnumerable<opleidingsplan.models.CourseImplementation> GetDummyDataModelCourseImplementations()
        {
            return new List<opleidingsplan.models.CourseImplementation>()
            {
                new opleidingsplan.models.CourseImplementation
                {
                    StartDay = new DateTime(2017, 16, 1),
                    Days = new List<DateTime>()
                    {
                        new DateTime(2017, 16, 1),
                        new DateTime(2017, 17, 1),
                        new DateTime(2017, 18, 1),
                    },
                    Location = "Veenendaal",
                },
                new opleidingsplan.models.CourseImplementation
                {
                    StartDay = new DateTime(2017, 3, 4),
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