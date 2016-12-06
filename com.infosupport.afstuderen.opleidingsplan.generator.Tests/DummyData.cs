using com.infosupport.afstuderen.opleidingsplan.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.generator.tests
{
    internal static class DummyData
    {
        internal static EducationPlanData GetEducationPlanData()
        {
            return new EducationPlanData
            {
                Created = new DateTime(2016, 11, 15),
                InPaymentFrom = new DateTime(2016, 11, 21),
                EmployableFrom = new DateTime(2017, 1, 30),
                KnowledgeOf = "MVC, DPAT, OOUML, SCRUMES",
                NameEmployee = "Pim Verheij",
                NameTeacher = "Felix Sedney",
                Profile = "NET_Developer",
            };
        }


    }
}
