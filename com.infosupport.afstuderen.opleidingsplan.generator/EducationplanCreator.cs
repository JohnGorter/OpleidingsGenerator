using com.infosupport.afstuderen.opleidingsplan.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public class EducationplanCreator
    {
        public EducationPlan EducationPlan { get; set; }

        public void AddEducationPlanData(EducationPlanData data)
        {
            EducationPlan = new EducationPlan();
            EducationPlan.Created = data.Created;
            EducationPlan.InPaymentFrom = data.InPaymentFrom;
            EducationPlan.EmployableFrom = data.EmployableFrom;
            EducationPlan.NameEmployee = data.NameEmployee;
            EducationPlan.NameTeacher = data.NameTeacher;
            EducationPlan.KnowledgeOf = data.KnowledgeOf;
            EducationPlan.Profile = data.Profile;
        }
    }
}
