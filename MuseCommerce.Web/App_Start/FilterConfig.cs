using MuseCommerce.Core.Security;
using System.Web;
using System.Web.Mvc;

namespace MuseCommerce.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new CheckPermissionAttribute());
        }
    }
}
