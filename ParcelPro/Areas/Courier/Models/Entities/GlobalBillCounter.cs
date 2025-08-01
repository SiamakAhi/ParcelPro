using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Models.Entities
{
    public class GlobalBillCounter
    {
        [Key]
        public long Id { get; set; }
        public long SellerId { get; set; }
        public long LastNumber { get; set; }
    }
}
