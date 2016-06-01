using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseCommerce.Data.Security.Identity
{
    public class SecurityDbContext : IdentityDbContext<ApplicationUser>
    {
        /*
         Enable-Migrations -ContextTypeName MuseCommerce.Data.Security.Identity.SecurityDbContext -MigrationsDirectory Security\Identity\Migrations
         * 
         add-migration Initial -ConfigurationTypeName IdentityConfiguration
         * 
         Update-DataBase -ConfigurationTypeName IdentityConfiguration
        **/
        public SecurityDbContext()
            : this("SeCommerce2")
        {
        }

        public SecurityDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString, false)
        {
            //Database.SetInitializer<SecurityDbContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .Property(t => t.PasswordHash).HasMaxLength(200);
        }

        public static SecurityDbContext Create()
        {
            return new SecurityDbContext();
        }
    }
}
