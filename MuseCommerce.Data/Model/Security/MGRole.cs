using MuseCommerce.Core.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MuseCommerce.Data.Model.Security
{
    public class MGRole:AuditableEntity
    {
        [MaxLength(100)]
        [Required]
        public string FName { set; get; }

        [MaxLength(100)]
        [Required]
        public string FDescription { set; get; }

        // MGRolePermission
        public virtual List<MGPermission>  FPermissions { get; set; }
                
        public virtual List<MGAccount>  FAccounts { get; set; }
    }
}
