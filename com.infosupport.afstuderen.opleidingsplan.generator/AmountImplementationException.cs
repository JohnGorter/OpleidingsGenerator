using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    [Serializable]
    public class AmountImplementationException : Exception
    {
        public AmountImplementationException(string message) 
            : base(message)
        {
        }
    }
}
