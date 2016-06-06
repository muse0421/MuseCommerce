using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseCommerce.Core.Exception_Handling
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ExceptionPolicyAttribute : Attribute
    {
        public string ExceptionPolicyName { get; private set; }
        public ExceptionPolicyAttribute(string exceptionPolicyName)
        {
            this.ExceptionPolicyName = exceptionPolicyName;
        }
    }
}
