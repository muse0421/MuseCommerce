using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MuseCommerce.Data.Security.Identity
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        //public ApplicationUserManager AppUserManager { get; set; }

        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
           // this.AppUserManager = userManager;
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }

        public override Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        {
            Task.Run<SignInStatus>(() =>
            {
                var user =UserManager.FindByNameAsync(userName);
                if (user == null)
                {
                    Debug.WriteLine("user is null");
                    return SignInStatus.Failure;
                }

                var CheckPassword = UserManager.CheckPasswordAsync(user.Result, password);
                if (user == null || CheckPassword.Result == false)
                {
                    Debug.WriteLine("CheckPassword Result eq false");
                    return SignInStatus.Failure;
                }

                SignInAsync(user.Result, isPersistent, true);

                return SignInStatus.Success;
            });

            //return SignInStatus.Success;
            return Task.Run<SignInStatus>(() => { return SignInStatus.Failure; });
           // return base.PasswordSignInAsync(userName, password, isPersistent, shouldLockout);
        }

      
        public override Task SignInAsync(ApplicationUser user, bool isPersistent, bool rememberBrowser)
        {
            //return Task.Run<SignInStatus>(() => { return SignInStatus.Success });
            return base.SignInAsync(user, isPersistent, rememberBrowser);
        }
    }
}
