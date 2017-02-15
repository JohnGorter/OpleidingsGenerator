using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InfoSupport.KC.OpleidingsplanGenerator.FrontEnd
{
    public partial class config : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        
        }

        public string GetVersion()
        {
            System.Diagnostics.FileVersionInfo versionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Version version = new Version(versionInfo.FileVersion);
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.InvariantCulture;
            return string.Format(ci, "v{0}.{1}, build {2}", version.Major, version.Minor, version.Build);
        }
    }
}