using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using MuseCommerce.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseCommerce.Data.Security.Identity
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
            // Configure validation logic for usernames
            UserValidator = new UserValidator<ApplicationUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                //RequireUniqueEmail = true, //Cannot require emails because users can be created from wpf admin and username not enforced to be as email
            };

            // Configure validation logic for passwords
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 5,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is: {0}"
            });
            RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "SecurityCode",
                BodyFormat = "Your security code is {0}"
            });
            EmailService = new EmailService();
            SmsService = new SmsService();
            //var dataProtectionProvider = options.DataProtectionProvider;
            //if (dataProtectionProvider != null)
            //{
            //    UserTokenProvider =
            //        new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            //}
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {

            var manager = new ApplicationUserManager(new ApplicationUserStore(context.Get<SecurityDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        public override Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return base.FindByNameAsync(userName);
        }

        public override Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {

            return Task.Run<bool>(() =>
            {
                if (SupportsUserPassword)
                {
                    IUserPasswordStore<ApplicationUser, string> store2 = Store as IUserPasswordStore<ApplicationUser, string>;


                    Debug.WriteLine("user.PasswordHash=" + user.PasswordHash.ToString());
                    Debug.WriteLine("password=" + password.ToString());

                    var newpassword1=PasswordHasher.HashPassword(password);
                    var newpassword2=PasswordHasher.HashPassword(password);

                    Debug.WriteLine("newpassword1=" + newpassword1);
                    Debug.WriteLine("newpassword2=" + newpassword2);

                    Debug.WriteLine("newpassword1 pass=" + PasswordHasher.VerifyHashedPassword(newpassword1, password).ToString());
                    Debug.WriteLine("newpassword2 pass=" + PasswordHasher.VerifyHashedPassword(newpassword2, password).ToString());
                    
                    var pass = PasswordHasher.VerifyHashedPassword(user.PasswordHash, password);
                    Debug.WriteLine("pass=" + pass.ToString());
                    if (pass == PasswordVerificationResult.Failed)
                    {
                        return false;
                    }
                    //var hash2 = PasswordHasher.(user, password);

                    //return PasswordHasher.VerifyHashedPassword(user, hash, password);

                    //var result = base.VerifyPasswordAsync(Store, user, password);
                    //if (result == PasswordVerificationResult.SuccessRehashNeeded)
                    //{
                    //    await UpdatePasswordHash(passwordStore, user, password, validatePassword: false);
                    //    await UpdateUserAsync(user);
                    //}

                    //var success = result != PasswordVerificationResult.Failed;
                    //if (!success)
                    //{
                    //    //Logger.LogWarning(0, "Invalid password for user {userId}.", await GetUserIdAsync(user));
                    //}
                    //return success;
                    return true;
                }
                return true;
            });


            
            //var hash = user.PasswordHash;
            //if (hash == null)
            //{
            //    return false;
            //}

            

            //return base.CheckPasswordAsync(user, password);
        }

        //protected override Task<bool> VerifyPasswordAsync(IUserPasswordStore<ApplicationUser, string> store, ApplicationUser user, string password)
        //{
        //    var newPassword = PasswordHasher.HashPassword("xiaohui");

        //    //var hash = newPassword != null ? PasswordHasher.HashPassword(user, newPassword) : null;
        //await passwordStore.SetPasswordHashAsync(user, hash, CancellationToken);

        //    return base.VerifyPasswordAsync(store, user, password);
        //}

        //public override Task<System.Security.Claims.ClaimsIdentity> CreateIdentityAsync(ApplicationUser user, string authenticationType)
        //{
        //    return base.CreateIdentityAsync(user, authenticationType);
        //}
    }

}
