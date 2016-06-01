using MuseCommerce.Data.Model.Security;
using System.Data.Entity.ModelConfiguration;

namespace MuseCommerce.Data.Repositories.EntityTypeConfiguration
{
    public class MGAccountMap : EntityTypeConfiguration<MGAccount>
    {
        public MGAccountMap()
        {
            this.HasKey(t => t.Id);

            // Relationships
            this.HasMany(t => t.FRoles)
                .WithMany(t => t.FAccounts)
                .Map(m =>
                {
                    m.ToTable("MGRoleAssignment");
                    m.MapLeftKey("FAccountID");
                    m.MapRightKey("FRoleID");
                });
        }
    }
}
