using MuseCommerce.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseCommerce.Data.Model.HR
{
    public class HMPerson : AuditableEntity
    {
        [MaxLength(20)]
        [Required]
        public string Code { get; set; }

        [MaxLength(20)]
        [Required]
        public string FName { get; set; }

        [MaxLength(30)]
        public string IDCardID { get; set; }

        [MaxLength(1)]
        public string SexType { get; set; }

        public DateTime Birthday { get; set; }

        [MaxLength(20)]
        public string Mobile { get; set; }

        [MaxLength(100)]
        public string EMail { get; set; }

        [MaxLength(1)]
        public string MarriedStatus { get; set; }

        public DateTime StartWordTime { get; set; }

        [MaxLength(1)]
        public string Status { get; set; }

    }
}
