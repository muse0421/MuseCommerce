using MuseCommerce.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseCommerce.Data.Model.HR
{
    public class ORGPDescriptionDuty : AuditableEntity
    {
        [MaxLength(100)]
        [Required]
        public string SerialNo { get; set; }

        [MaxLength(100)]
        [Required]
        public string Description { get; set; }

    }
}
