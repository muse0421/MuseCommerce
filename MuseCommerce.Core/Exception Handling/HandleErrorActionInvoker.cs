using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MuseCommerce.Core.Exception_Handling
{
    public class HandleErrorActionInvoker : ControllerActionInvoker
    {
        public virtual ActionResult InvokeActionMethod(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            IDictionary<string, object> parameterValues = this.GetParameterValues(controllerContext, actionDescriptor);
            return base.InvokeActionMethod(controllerContext, actionDescriptor, parameterValues);
        }
    }
}
