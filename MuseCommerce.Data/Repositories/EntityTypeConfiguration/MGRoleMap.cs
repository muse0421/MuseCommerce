using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MuseCommerce.Data.Model.Security;

namespace MuseCommerce.Data.Repositories.EntityTypeConfiguration
{

    public class MGRoleMap : EntityTypeConfiguration<MGRole>
    {
        public MGRoleMap()
        {
            this.HasKey(t => t.Id);

            // Relationships
            this.HasMany(t => t.FPermissions)
                .WithMany(t => t.FRoles)
                .Map(m =>
                {
                    m.ToTable("MGRolePermission");
                    m.MapLeftKey("FRoleID");
                    m.MapRightKey("FPermissionID");
                });
        }
    }
}
