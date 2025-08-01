using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Models.Commercial
{
    public class UserSeller
    {
        [Key]
        public Int64 Id { get; set; }
        public string Username { get; set; }
        public Int64 SellerId { get; set; }
        public int? CustomerId { get; set; }

    }
}
