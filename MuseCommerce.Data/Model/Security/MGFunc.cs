using MuseCommerce.Core.Common;
using MuseCommerce.Data.Model.Security;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MuseCommerce.Data.Model
{
    public class MGFunc : AuditableEntity
    {
        [MaxLength(100)]
        [Required]
        public string FName { set; get; }

        [MaxLength(100)]
        [Required]
        public string FUrl { set; get; }
        
        public bool IsActive { set; get; }

        public int FPriority { set; get; }

        [MaxLength(100)]
        public string FParentID { set; get; }

        [ForeignKey("FParentID")]
        public MGFunc FParent { set; get; }

        [MaxLength(100)]
        public string FPermissionID { set; get; }

        [ForeignKey("FPermissionID")]
        public MGPermission MGPermission { set; get; }
    }
}
