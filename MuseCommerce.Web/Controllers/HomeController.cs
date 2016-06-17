using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MuseCommerce.Core.Log;
using MuseCommerce.Data.Model;
using MuseCommerce.Data.Model.Security;
using MuseCommerce.Data.Repositories;
using MuseCommerce.Data.Security.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MuseCommerce.Web.Controllers
{    
    public class HomeController : MuseController
    {

        // GET: Default
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ILog _logging;
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

        public ApplicationSignInManager SignInManager
        {
            get
            {
                if (_signInManager == null)
                {
                    _signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                }
                return _signInManager;
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                if (_userManager == null)
                {
                    _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                }
                return _userManager;
            }
            private set
            {
                _userManager = value;
            }
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string username, string password, string returnUrl)
        {
            var result =await SignInManager.PasswordSignInAsync(username, password, false, shouldLockout: false);
            Debug.WriteLine("result=" + result.ToString());

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return RedirectToAction("UserLock"); 
               
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View();
            }
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult Index()
        {
            //var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //var SignInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            // HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            //for (int i = 0; i < 10; i++)
            //{

            //    ApplicationUser user2 = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "xpy" + i.ToString(), Email = "muse@hot.mail" };
            //    var result = await UserManager.CreateAsync(user2, "xiaohui");
            //    //await SignInManager.SignInAsync(user1, isPersistent: false, rememberBrowser: false);
            //}
            _logging.Information("注入消息");

            Debug.WriteLine("AuthenticationManager.User.Identity.Name in =" + AuthenticationManager.User.Identity.Name);

            //AuthenticationManager.SignOut();
            Debug.WriteLine("User.Identity.Name in =" + User.Identity.Name);


            //ApplicationUser user1 = new ApplicationUser { Id = "f377a22f-6ef8-464c-a221-c3e0e3d05106", UserName = "xpy2", Email = "muse@hot.mail" };
            //var result2 = await SignInManager.PasswordSignInAsync("xpy2", "xiaohui", false, shouldLockout: false);

            //Debug.WriteLine("User.Identity.Name out=" + User.Identity.Name);

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
                var MGFuncTemp = context.Set<MGFunc>().ToList();

                var Permissions = context.Set<MGAccount>().Where(u=>u.FUseName==User.Identity.Name)
                     .SelectMany(p => p.FRoles).SelectMany(r => r.FPermissions).ToList().Distinct()
                     .Select(x => x.Id).ToList();
                foreach (var item in MGFuncTemp)
                {
                    if (Permissions.Contains(item.FPermissionID))
                    {
                        oDataModel.Add(item);
                        if (!string.IsNullOrEmpty(item.FParentID) && !oDataModel.Exists(x => x.Id == item.FParentID))
                        {
                            foreach (var parent in MGFuncTemp)
                            {
                                if (item.FParentID == parent.Id)
                                {
                                    oDataModel.Add(parent);
                                }
                            }
                        }
                    }
                }

            }
            return PartialView("_Menu", oDataModel.OrderBy(x=>x.FPriority).ToList());
        }

        public ActionResult Dashboard()
        {
            return View();
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

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}