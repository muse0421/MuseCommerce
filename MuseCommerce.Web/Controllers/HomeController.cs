using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using MuseCommerce.Data.Model;
using MuseCommerce.Data.Repositories;
using MuseCommerce.Data.Security.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MuseCommerce.Web.Controllers
{
    public class HomeController : MuseController
    {
        // GET: Default
        public async Task<ActionResult> Index()
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new SecurityDbContext()));
            var SignInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            // HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //ApplicationUser user1 = new ApplicationUser { Id = System.Guid.NewGuid().ToString(), UserName = "xpy", Email = "muse@hot.mail" };
            //var result = await UserManager.CreateAsync(user1, "xiaohui");            
            //await SignInManager.SignInAsync(user1, isPersistent: false, rememberBrowser: false);


            var result = await SignInManager.PasswordSignInAsync("xpy", "xiaohui", false, shouldLockout: false);

            return View();
        }

        public ActionResult Menu()
        {
            List<MGFunc> oDataModel=new List<MGFunc>();
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
               oDataModel= context.Set<MGFunc>().ToList();
            }
            return PartialView("_Menu", oDataModel);
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            return View("Index");
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult Lockout()
        {
            return View();
        }
    }
}