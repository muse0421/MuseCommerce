using MuseCommerce.Core.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace MuseCommerce.Data.Model
{
    public class ItemCore : AuditableEntity
    {
        [MaxLength(100)]
        [Required]
        public string FNumber { get; set; }
        [MaxLength(100)]
        public string FShortNumber { get; set; }
        [MaxLength(100)]
        public string FHelpCode { get; set; }
        [MaxLength(100)]
        [Required]
        public string FName { get; set; }
        [MaxLength(100)]
        public string FModel { get; set; }

        public bool FForbidden { set; get; } 

        public bool FDeleted { get; set; }
    }
}
