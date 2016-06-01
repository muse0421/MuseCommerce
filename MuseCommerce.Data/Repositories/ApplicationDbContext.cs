using MuseCommerce.Data.Model;
using MuseCommerce.Data.Model.Security;
using MuseCommerce.Data.Repositories.EntityTypeConfiguration;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseCommerce.Data.Repositories
{
    public class ApplicationDbContext : DbContext
    { /*
         Enable-Migrations -ContextTypeName MuseCommerce.Data.Repositories.ApplicationDbContext -MigrationsDirectory Repositories\Migrations
         * 
         add-migration Initial -ConfigurationTypeName ApplicationDbConfiguration
         add-migration updatefuncname -ConfigurationTypeName ApplicationDbConfiguration
         * 
         Update-DataBase -ConfigurationTypeName ApplicationDbConfiguration
        **/

        public DbSet<MGAccount> MGAccount { set; get; }

        public DbSet<MGRole> MGRole { set; get; }

        public DbSet<MGPermission> MGPermission { set; get; }
        
        public DbSet<MGFunc> MGFunc { set; get; }

        public DbSet<ItemCore> ItemCore { set; get; }

        public DbSet<PORequest> PORequest { set; get; }

        public DbSet<PORequestEntry> PORequestEntry { set; get; }

        public ApplicationDbContext()
            : base("SeCommerce2")
        {
            //Database.SetInitializer<ApplicationDbContext>(new DBInitializer());           
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Conventions.Add<StoreGeneratedIdentityKeyConvention>();
            
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new MGAccountMap());
            modelBuilder.Configurations.Add(new MGRoleMap());
        }
    }
}
