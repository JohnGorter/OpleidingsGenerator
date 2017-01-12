﻿using com.infosupport.afstuderen.opleidingsplan.api.models;
using com.infosupport.afstuderen.opleidingsplan.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.api.managers
{
    public interface IEducationPlanManager
    {
        EducationPlan GenerateEducationPlan(RestEducationPlan educationPlan);
        long SaveEducationPlan(RestEducationPlan educationPlan);
        EducationPlan FindEducationPlan(long id);
        List<EducationPlan> FindEducationPlans(EducationPlanSearch search);
    }
}
