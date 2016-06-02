using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MuseCommerce.Core.Exception_Handling
{
    public class ExtendedController : Controller
    {
        private static Dictionary<Type, ControllerDescriptor> controllerDescriptors = new Dictionary<Type, ControllerDescriptor>();
        private static object syncHelper = new object();

        protected override void OnException(ExceptionContext filterContext)
        {
            //或者当前的ExceptionPolicy，如果不存在，则直接调用基类OnException方法
            //string exceptionPolicyName = this.GetExceptionPolicyName();
            //if (string.IsNullOrEmpty(exceptionPolicyName))
            //{
            //    base.OnException(filterContext);
            //    return;
            //}

            //利用EntLib的EHAB进行异常处理，并获取错误消息和最后抛出的异常
            filterContext.ExceptionHandled = true;
            Exception exceptionToThrow;
            string errorMessage;
            try
            {

               // Muse ExceptionPolicy.HandleException(filterContext.Exception, exceptionPolicyName, out exceptionToThrow);
                errorMessage = System.Web.HttpContext.Current.GetErrorMessage();
            }
            finally
            {
                System.Web.HttpContext.Current.ClearErrorMessage();
            }
            
            // Muse  exceptionToThrow = exceptionToThrow ?? filterContext.Exception;
            exceptionToThrow =  filterContext.Exception;

            if (exceptionToThrow is DbEntityValidationException)
            {
                var temp = exceptionToThrow as DbEntityValidationException;
                 temp.EntityValidationErrors.ToList().ForEach(entiy => {
                     entiy.ValidationErrors.ToList().ForEach(err => {
                         errorMessage += err.PropertyName;
                         errorMessage += err.ErrorMessage;
                     });
                });
            }

            //对于Ajax请求，直接返回一个用于封装异常的JsonResult
            if (Request.IsAjaxRequest())
            {
                filterContext.Result = Json(new ExceptionDetail(exceptionToThrow, errorMessage));
                return;
            }

            //如果设置了匹配的HandleErrorAction，则调用之；
            //否则将Error View呈现出来
            string handleErrorAction = ControllerContext.RouteData.GetRequiredString("action");
                //this.GetHandleErrorActionName();
            string controllerName = ControllerContext.RouteData.GetRequiredString("controller");
            string actionName = ControllerContext.RouteData.GetRequiredString("action");

            

            errorMessage = string.IsNullOrEmpty(errorMessage) ? exceptionToThrow.Message : errorMessage;
            ActionDescriptor actionDescriptor = null;
            if (!string.IsNullOrEmpty(handleErrorAction))
            {
                actionDescriptor = Descriptor.FindAction(ControllerContext, handleErrorAction);
            }

            filterContext.Result = Json(new ExceptionDetail(exceptionToThrow, errorMessage));
            return;

            if (actionDescriptor == null)
            {
                filterContext.Result = View("Error", new ExtendedHandleErrorInfo(exceptionToThrow, controllerName, actionName, errorMessage));
            }
            else
            {
                ModelState.AddModelError("", errorMessage);
                filterContext.Result = this.HandleErrorActionInvoker.InvokeActionMethod(ControllerContext, actionDescriptor);
            }
        }

        public ControllerDescriptor Descriptor
        {
            get
            {
                ControllerDescriptor descriptor;
                if (controllerDescriptors.TryGetValue(this.GetType(), out descriptor))
                {
                    return descriptor;
                }
                lock (syncHelper)
                {
                    if (controllerDescriptors.TryGetValue(this.GetType(), out descriptor))
                    {
                        return descriptor;
                    }
                    else
                    {
                        descriptor = new ReflectedControllerDescriptor(this.GetType());
                        controllerDescriptors.Add(this.GetType(), descriptor);
                        return descriptor;
                    }
                }
            }
        }
        //public string GetExceptionPolicyName()
        //{
        //    string actionName = ControllerContext.RouteData.GetRequiredString("action");
        //    ActionDescriptor actionDescriptor = this.Descriptor.FindAction(ControllerContext, actionName);
        //    if (null == actionDescriptor)
        //    {
        //        return string.Empty;
        //    }
        //    ExceptionPolicyAttribute exceptionPolicyAttribute = actionDescriptor.GetCustomAttributes(true).OfType<ExceptionPolicyAttribute>().FirstOrDefault()
        //        ?? Descriptor.GetCustomAttributes(true).OfType<ExceptionPolicyAttribute>().FirstOrDefault()
        //        ?? new ExceptionPolicyAttribute("");
        //    return exceptionPolicyAttribute.ExceptionPolicyName;
        //}
        //public string GetHandleErrorActionName()
        //{
        //    string actionName = ControllerContext.RouteData.GetRequiredString("action");
        //    ActionDescriptor actionDescriptor = this.Descriptor.FindAction(ControllerContext, actionName);
        //    if (null == actionDescriptor)
        //    {
        //        return string.Empty;
        //    }
        //    HandleErrorActionAttribute handleErrorActionAttribute = actionDescriptor.GetCustomAttributes(true).OfType<HandleErrorActionAttribute>().FirstOrDefault()
        //        ?? Descriptor.GetCustomAttributes(true).OfType<HandleErrorActionAttribute>().FirstOrDefault()
        //        ?? new HandleErrorActionAttribute("");
        //    return handleErrorActionAttribute.HandleErrorAction;
        //}
        public HandleErrorActionInvoker HandleErrorActionInvoker { get; private set; }
        public ExtendedController()
        {
            this.HandleErrorActionInvoker = new HandleErrorActionInvoker();
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

    }
}
