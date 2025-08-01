using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Models.Entities
{
    public class Cu_SaleContract
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "شناسه فروشنده")]
        public long SellerId { get; set; }

        [Display(Name = "عنوان قرارداد")]
        public string Title { get; set; }

        [Display(Name = "شناسه طرف قرارداد")]
        public long PartyId { get; set; }

        [Display(Name = "طرف قرارداد")]
        public virtual Party ContractParty { get; set; }

        [Display(Name = "تاریخ شروع")]
        public DateTime StartDate { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "وضعیت فعال بودن حساب")]
        public bool AccounIstActive { get; set; } = true;

        [Display(Name = "کلید API")]
        public string? ApiKey { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.Now;

        public virtual ICollection<Cu_SaleContractUser> ClientUsers { get; set; }
        public virtual ICollection<Cu_BillOfLading> BillOfLadings { get; set; }
    }
}
