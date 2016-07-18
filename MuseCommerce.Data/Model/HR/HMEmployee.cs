using MuseCommerce.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseCommerce.Data.Model.HR
{
    public class HMEmployee : AuditableEntity
    {
        [MaxLength(100)]
        [Required]
        public string FNumber { get; set; }

        [MaxLength(100)]
        [Required]
        public string FName { get; set; }

        [MaxLength(20)]
        public string FEmpGroup { get; set; }

        [MaxLength(20)]
        public string FEmpGroupID { get; set; }

        [MaxLength(100)]
        public string FDepartmentID { get; set; }

        [ForeignKey("FDepartmentID")]
        public ORGDepartment FDepartment { set; get; }

        [MaxLength(100)]
        public string FDutyID { get; set; }

        [ForeignKey("FDutyID")]
        public ORGPDescriptionDuty FORGDuty { set; get; }

        [MaxLength(10)]
        public string FJobTypeID { get; set; }

        [MaxLength(100)]
        public string FBankID { get; set; }

        [MaxLength(100)]
        public string FBankAccount { get; set; }

        public DateTime FHireDate { get; set; }

        public DateTime? FLeaveDate { get; set; }
                
        public bool FDeleted { get; set; }

    }
}
