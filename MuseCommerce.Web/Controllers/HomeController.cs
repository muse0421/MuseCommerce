using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MuseCommerce.Core.Log;
using MuseCommerce.Data.Model;
using MuseCommerce.Data.Repositories;
using MuseCommerce.Data.Security.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MuseCommerce.Web.Controllers
{
    public class HomeController : MuseController
    {
        // GET: Default
        ILog _logging;
        public HomeController(ILog logging)
        {
            _logging = logging;
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
       
        public async Task<ActionResult> Index()
        {
            var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var SignInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            // HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            //for (int i = 0; i < 10; i++)
            //{

            //    ApplicationUser user2 = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "xpy" + i.ToString(), Email = "muse@hot.mail" };
            //    var result = await UserManager.CreateAsync(user2, "xiaohui");
            //    //await SignInManager.SignInAsync(user1, isPersistent: false, rememberBrowser: false);
            //}
            _logging.Information("注入消息");

            Debug.WriteLine("AuthenticationManager.User.Identity.Name in =" + AuthenticationManager.User.Identity.Name);

            AuthenticationManager.SignOut();
            Debug.WriteLine("User.Identity.Name in =" + User.Identity.Name);


            ApplicationUser user1 = new ApplicationUser { Id = "f377a22f-6ef8-464c-a221-c3e0e3d05106", UserName = "xpy2", Email = "muse@hot.mail" };
            var result2 = await SignInManager.PasswordSignInAsync("xpy2", "xiaohui", false, shouldLockout: false);

            Debug.WriteLine("User.Identity.Name out=" + User.Identity.Name);

            //try
            //{

            //    await SignInManager.SignInAsync(user1, isPersistent: false, rememberBrowser: false);
            //}
            //catch (Exception ex)
            //{
            //    var temp = ex.ToString();
            //}

            return View();
        }

        public ActionResult UnAuthorized()
        {
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