using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseCommerce.Core.Common
{
    public abstract class AuditableEntity : Entity, IAuditable
    {
        public AuditableEntity()
        {
            //CreatedDate = DateTime.Now;
        }

        #region IAuditable Members

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
        
        [MaxLength(100)]
        public string CreatedBy { get; set; }

        [MaxLength(100)]
        public string ModifiedBy { get; set; }

        #endregion
    }
}
