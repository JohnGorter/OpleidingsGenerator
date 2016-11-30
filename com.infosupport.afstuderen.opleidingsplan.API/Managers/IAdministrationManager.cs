﻿using com.infosupport.afstuderen.opleidingsplan.api.Models;
using com.infosupport.afstuderen.opleidingsplan.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.api.Managers
{
    public interface IAdministrationManager
    {
        IEnumerable<Profile> FindProfiles();
        Profile FindProfile(string profileName);
    }
}
