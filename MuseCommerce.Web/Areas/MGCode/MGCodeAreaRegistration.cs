using System.Web.Mvc;

namespace MuseCommerce.Web.Areas.MGCode
{
    public class MGCodeAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "MGCode";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "MGCode_default",
                "MGCode/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}