using MuseCommerce.Core.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseCommerce.Data.Model.Security
{
    public class MGAccount : AuditableEntity
    {
        [MaxLength(100)]
        [Required]
        public string FUseName { set; get; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AccountState FAccountState { set; get; }

        public bool FIsAdministrator { set; get; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AccountType FUserType { set; get; }

        //MGRoleAssignment        
        public virtual List<MGRole> FRoles { get; set; }
    }
}
