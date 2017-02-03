using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace InfoSupport.KC.OpleidingsplanGenerator.Generator
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
