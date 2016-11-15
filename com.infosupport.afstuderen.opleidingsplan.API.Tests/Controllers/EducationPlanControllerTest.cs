using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.infosupport.afstuderen.opleidingsplan.api;

namespace com.infosupport.afstuderen.opleidingsplan.API.Tests.Controllers
{
    [TestClass]
    public class EducationPlanControllerTest
    {
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            AutoMapperConfiguration.Configure();
        }
    }
}
