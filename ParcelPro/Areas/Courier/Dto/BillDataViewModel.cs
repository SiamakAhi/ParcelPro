using ParcelPro.Areas.Courier.Dto.FinancialDtos;
using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Dto
{
    public class BillDataViewModel
    {
        public ChangeSettelmentDto? ChangeSettelmentModel { get; set; }
        public Guid Id { get; set; }
        public long SellerId { get; set; }

        [Display(Name = "شماره بارنامه")]
        public string WaybillNumber { get; set; }

        [Display(Name = "تاریخ صدور")]
        [Required(ErrorMessage = "تاریخ بارنامه نامشخص است")]
        public DateTime IssuanceDate { get; set; } = DateTime.Now;

        [Display(Name = " مسیر")]
        [Required(ErrorMessage = "نعیین مسیر الزامی است")]
        public int RouteId { get; set; }
        public string? RouteName { get; set; }


        [Display(Name = " سرویس")]
        [Required(ErrorMessage = "نوع سرویس را مشخص کنید")]
        public int ServiceId { get; set; }
        public string? ServiceName { get; set; }


        [Display(Name = " فرستنده")]
        [Required(ErrorMessage = "تعیین فرستنده الزامی است")]
        public long SenderId { get; set; }
        [Display(Name = " آدرس فرستنده")]
        [Required(ErrorMessage = "نوشتن آدرس فرستنده الزامی است")]
        public string? SenderAddress { get; set; }
        public string? SenderName { get; set; }
        public string? SenderNationalId { get; set; }

        [Display(Name = " گیرنده")]
        [Required(ErrorMessage = "تعیین گیرنده الزامی است")]
        public long ReceiverId { get; set; }

        [Display(Name = " آدرس گیرنده")]
        [Required(ErrorMessage = "نوشتن آدرس گیرنده الزامی است")]
        public string ReceiverAddress { get; set; }
        public string ReciverName { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }


        [Display(Name = "تاریخ به روزرسانی")]
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "کاربر ادیت کننده")]
        public string? UpdatedBy { get; set; }

        public bool Setteled { get; set; } = false;
        public bool HasParcel { get; set; } = false;
        public bool HasFinancialRecord { get; set; } = false;
        public bool HasTreasuryRecord { get; set; } = false;
        public bool IsSetteled { get; set; } = false;
        public int ParcelQty { get; set; } = 0;
        public long TotalPrice { get; set; } = 0;
        public long PayedAmount { get; set; } = 0;
        public long RemindingAmout { get; set; } = 0;

        public int? SettelmentTYpeId { get; set; }
        public string? SettelmentTYpeName { get; set; }
        public short StatusId { get; set; }

    }
}
