using com.infosupport.afstuderen.opleidingsplan.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.api.Managers
{
    public interface IEducationPlanManager
    {
        EducationPlan GenerateEducationPlan(string[] courses);
    }
}
