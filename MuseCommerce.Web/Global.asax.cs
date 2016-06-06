using MuseCommerce.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MuseCommerce.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static TraceSource mySource = new TraceSource("TraceSourceApp");

        protected void Application_Start()
        {
            mySource.TraceEvent(TraceEventType.Error, 1, "Error message.");
            //mySource.TraceEvent(TraceEventType.Warning, 2, "Warning message.");
            //mySource.TraceEvent(TraceEventType.Critical, 3, "Critical message.");
           
            //mySource.TraceEvent(TraceEventType.Error, 4, "Error message.");
            //mySource.TraceInformation("Informational message.");
            //mySource.TraceInformation("Informational message.");
            //mySource.Close();


            //try
            //{

            //    using (ApplicationDbContext context = new ApplicationDbContext())
            //    {
            //        DBInit.Seed(context);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MvcApplication.mySource.TraceEvent(TraceEventType.Error, 1, ex.ToString());
            //    MvcApplication.mySource.TraceInformation("Informational message.");
            //}
            
            

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            mySource.TraceEvent(TraceEventType.Error, 1, exception.ToString());
        }

        protected void Application_Exit(Object sender, EventArgs e)
        {
            mySource.Close();
        }
    }
}
