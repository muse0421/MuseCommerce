using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MuseCommerce.Web.Controllers
{
    public class ManageController : MuseController
    {
        // GET: Manage
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Func()
        {
            return View();
        }
    }
}