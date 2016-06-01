namespace MuseCommerce.Data.Security.Identity.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class IdentityConfiguration : DbMigrationsConfiguration<MuseCommerce.Data.Security.Identity.SecurityDbContext>
    {
        public IdentityConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Security\Identity\Migrations";
        }

        protected override void Seed(MuseCommerce.Data.Security.Identity.SecurityDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
