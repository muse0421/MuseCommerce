using MuseCommerce.Core.Common;
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

        public POTranType FTranType { set; get; }

        [MaxLength(100)]
        public string FStatus { set; get; }

        [MaxLength(200)]
        public string FNote { set; get; }

        public Address PoAddress { set; get; }
               
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
