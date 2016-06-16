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
        private ApplicationUserManager userManager;

        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
            this.userManager = userManager;           
 
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
            return Task.Run<SignInStatus>(() =>
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
                  Debug.WriteLine("SignInStatus1=" + SignInStatus.Success.ToString());
                  //SignInAsync(user.Result, isPersistent, true);


                  var userPrincipal = user.Result.GenerateUserIdentityAsync(userManager);
                  // Review: should we guard against CreateUserPrincipal returning null?
                  userPrincipal.Result.AddClaim(new Claim(ClaimTypes.AuthenticationMethod, ""));
                  AuthenticationManager.SignIn(new AuthenticationProperties(), userPrincipal.Result);
                  
                  Debug.WriteLine("SignInStatus2=" + SignInStatus.Success.ToString());
                  return SignInStatus.Success;
              });
        }
    }
}
