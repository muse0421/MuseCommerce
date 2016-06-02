using MuseCommerce.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace MuseCommerce.Data.Model.Security
{
    public class MGPermission : AuditableEntity
    {
        [MaxLength(100)]
        [Required]
        public string FName { set; get; }

        [MaxLength(100)]
        [Required]
        public string FDescription { set; get; }

        [IgnoreDataMember]        
        public virtual List<MGRole> FRoles { get; set; }
    }
}
