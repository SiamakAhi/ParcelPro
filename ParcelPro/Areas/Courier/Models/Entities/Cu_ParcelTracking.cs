using ParcelPro.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Models.Entities
{
    public class Cu_ParcelTracking
    {
        [Key]
        public long Id { get; set; }
        public Guid BillOfLadingId { get; set; }
        public Guid? ParcelId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public TimeSpan Time { get; set; } = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        public string? BillOfLadingNumber { get; set; }
        public string Description { get; set; }

        [Display(Name = "در صفحه رهگیری مشتری نمایش داده شود")]
        public bool ShowInCustomerTracking { get; set; } = false;

        [Display(Name = "در صفحه رهگیری عملیات نمایش داده شود")]
        public bool ShowInOperationsTracking { get; set; } = false;

        public string UserId { get; set; }
        public AppIdentityUser User { get; set; }

        public int StatusId { get; set; }
        public Cu_ParcelStatus Status { get; set; }

    }
}
