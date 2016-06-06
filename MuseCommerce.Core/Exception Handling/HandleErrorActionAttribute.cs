using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseCommerce.Core.Exception_Handling
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class HandleErrorActionAttribute : Attribute
    {
        public string HandleErrorAction { get; private set; }
        public HandleErrorActionAttribute(string handleErrorAction = "")
        {
            this.HandleErrorAction = handleErrorAction;
        }
    }
}
