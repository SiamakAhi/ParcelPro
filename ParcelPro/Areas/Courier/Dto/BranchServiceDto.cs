using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Dto
{
    public class BranchServiceDto
    {
        public int Id { get; set; }
        public long SellerId { get; set; }

        [Display(Name = "شعبه")]
        public Guid BranchId { get; set; }
        public string? BranchName { get; set; }

        [Display(Name = "سرویس")]
        public int ServiceId { get; set; }

        [Display(Name = "سرویس")]
        public string? ServiceName { get; set; }
    }
}
