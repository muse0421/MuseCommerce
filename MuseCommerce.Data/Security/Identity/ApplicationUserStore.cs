using Microsoft.AspNet.Identity.EntityFramework;
using MuseCommerce.Data.Model;
using MuseCommerce.Data.Model.Security;
using MuseCommerce.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseCommerce.Data.Security.Identity
{
    public class ApplicationUserStore : UserStore<ApplicationUser>
    {
        public ApplicationUserStore(SecurityDbContext context)
            : base(context)
        {

        }

        public override System.Threading.Tasks.Task<ApplicationUser> FindByNameAsync(string userName)
        {
            using (ApplicationDbContext appcontext = new ApplicationDbContext())
            {
                var oTemp = appcontext.Set<MGAccount>().Where(p => p.FUseName == userName).First();

                ApplicationUser user1 = new ApplicationUser
                {
                    Id = oTemp.Id,
                    UserName = oTemp.FUseName,
                    Email = oTemp.FEmail,
                    LockoutEnabled = oTemp.FLockoutEnabled,
                    TwoFactorEnabled = oTemp.FTwoFactorEnabled,
                    SecurityStamp = oTemp.FSecurityStamp,
                    PasswordHash = oTemp.FPasswordHash
                };
                return Task.Run<ApplicationUser>(() =>
                {
                    return user1;
                });
            }
        }

        public override System.Threading.Tasks.Task<ApplicationUser> FindByIdAsync(string userId)
        {
            using (ApplicationDbContext appcontext = new ApplicationDbContext())
            {
                var oTemp = appcontext.Set<MGAccount>().Where(p => p.Id == userId).First();

                ApplicationUser user1 = new ApplicationUser
                {
                    Id = oTemp.Id,
                    UserName = oTemp.FUseName,
                    Email = oTemp.FEmail,
                    LockoutEnabled = oTemp.FLockoutEnabled,
                    TwoFactorEnabled = oTemp.FTwoFactorEnabled,
                    SecurityStamp = oTemp.FSecurityStamp,
                    PasswordHash = oTemp.FPasswordHash
                };
                return Task.Run<ApplicationUser>(() =>
                {
                    return user1;
                });
            }
        }

        public override System.Threading.Tasks.Task<IList<Microsoft.AspNet.Identity.UserLoginInfo>> GetLoginsAsync(ApplicationUser user)
        {
            return base.GetLoginsAsync(user);
        }
    }
}
