using ParcelPro.Areas.Courier.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Accounting.Dto.AccRepresentative
{
    public class AccRepresentativeInfoDto
    {

        public Guid Id { get; set; }
        public long SellerId { get; set; }

        [Display(Name = "کد شعبه")]
        public string? BranchCode { get; set; }

        [Display(Name = "نام")]
        public string BranchName { get; set; }

        [Display(Name = "شهر")]
        public int? CityId { get; set; }
        public string CityName { get; set; }

        [Display(Name = "نوع نمایندگی")]
        public bool IsOwnership { get; set; } = true; // مالکیت/شراکنی

        [Display(Name = "تاریخ شروع به کار")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "تاریخ افتتاح")]
        public DateTime? OpeningDate { get; set; }

        // ===================================================== New fields added as requested
        [Display(Name = "آدرس")]
        public string? Address { get; set; }

        [Display(Name = "مسئول شعبه")]
        public string? BranchManager { get; set; }

        [Display(Name = "شماره تماس مسئول شعبه")]
        public string? BranchManagerPhoneNumber { get; set; }

        [Display(Name = "طرف حساب در حسابداری")]
        public long? PartyId { get; set; }

        [Display(Name = "طرف حساب در حسابداری")]
        public string PartyName { get; set; }

        // ======================================================= فعالیت‌های جدید
        [Display(Name = "صادرکننده بارنامه")]
        public bool IsBillOfLadingIssuer { get; set; }

        [Display(Name = "ناوگان حمل درون شهری")]
        public bool IsUrbanFleet { get; set; }

        [Display(Name = "ناوگان حمل بین شهری زمینی")]
        public bool IsIntercityFleet { get; set; }

        [Display(Name = "تجزیه و مبادلات مرسولات")]
        public bool? IsHub { get; set; } = false;

        [Display(Name = "درصد پورسانت")]
        public decimal? CommissionPercentage { get; set; }

        public virtual ICollection<Cu_BranchUser> BranchUsers { get; set; }


        // ============================================= Fields with shorter names
        [Display(Name = "بارنامه خارجی")]
        public bool IsExternalBLIssuer { get; set; }

        [Display(Name = "بارنامه داخلی")]
        public bool IsInternalBLIssuer { get; set; }

        [Display(Name = "نام شعبه قدیم")]
        public string? OldBranchName { get; set; }

        [Display(Name = "نام نمایندگی قدیم")]
        public string? OldDistRepName { get; set; }

        //============================================== financial calcu
        [Display(Name = "درصد تخفیف مجاز برای هر بارنامه")]
        public decimal? AllowdDiscountRate { get; set; } = 10;

        [Display(Name = "سهم صدور")]
        public decimal? IssueShare { get; set; } = 0;

        [Display(Name = "محاسبه سهم فروش درصدی محاسبه شود یا ثابت ؟")]
        public bool IsIssueShareFixed { get; set; } = false;

        [Display(Name = "سهم توزیع از هر بارنامه")]
        public decimal? DistShare { get; set; } = 0;

        [Display(Name = "محاسبه سهم توزیع درصدی محاسبه شود یا ثابت ؟")]
        public bool IsDistShareFixed { get; set; } = false;
    }
}
