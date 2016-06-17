using MuseCommerce.Core.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseCommerce.Data.Model
{
    public enum POTranType
    {
        Office = 1,
        WearHourse = 2
    }

    public class PORequest : AuditableEntity
    {
        [MaxLength(100)]
        public string FBillNo { set; get; }

        public DateTime FDate { set; get; }

        [JsonConverter(typeof(StringEnumConverter))]
        public POTranType FTranType { set; get; }

        [MaxLength(10)]
        public string FStatus { set; get; }

        [MaxLength(200)]
        public string FNote { set; get; }

        public Address PoAddress { set; get; }

        public bool FForbidden { set; get; }

        public bool FDeleted { get; set; }
               
        public virtual List<PORequestEntry> PORequestEntrys { set; get; }
    }

    [ComplexType]
    public class Address
    {
        [MaxLength(50)]
        public string StreetNumber { get; set; }

        [MaxLength(200)]
        public string StreetName { get; set; }
    }

    public class PORequestEntry : AuditableEntity
    {
        [MaxLength(100)]
        public string FInterID { set; get; }

        [ForeignKey("FInterID")]
        public PORequest PORequest { set; get; }

        [Required]
        [Range(0, 500)]
        public decimal FQty { set; get; }

        public decimal FPrice { set; get; }

        public decimal FSecQty { set; get; }

        [MaxLength(100)]
        public string FItemID { set; get; }

        [ForeignKey("FItemID")]
        public ItemCore FItem { set; get; }
    }
}
