using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Dto.DitributionDto
{
    public class AddDistriutionDto
    {

        public Guid Id { get; set; }
        public long SellerId { get; set; }

        [Display(Name = "شرکت صادرکننده")]
        public int BusinessPartnerId { get; set; }
        public short WayBillType { get; set; } = 2;

        [Display(Name = "شماره بارنامه")]
        public string? WaybillNumber { get; set; }

        [Display(Name = "شماره بارنامه مرحع")]
        public string? ReferenceCode { get; set; }

        [Display(Name = "تاریخ صدور")]
        [Required(ErrorMessage = "تاریخ بارنامه نامشخص است")]
        public string strIssuanceDate { get; set; } = DateTime.Now.LatinToPersian();

        [Display(Name = " مسیر")]
        [Required(ErrorMessage = "نعیین مسیر الزامی است")]
        public int? RouteId { get; set; }


        [Display(Name = " سرویس")]
        [Required(ErrorMessage = "نوع سرویس را مشخص کنید")]
        public int ServiceId { get; set; }

        [Display(Name = "شعبه صادرکننده")]
        public Guid OriginBranchId { get; set; }
        public Guid OriginHubId { get; set; }

        [Display(Name = "نماینده توزیع")]
        [Required(ErrorMessage = "هاب مبدأ شناسایی نشد")]
        public Guid? DistributerRepresentativeId { get; set; }


        [Display(Name = " فرستنده")]
        [Required(ErrorMessage = "تعیین فرستنده الزامی است")]
        public long SenderId { get; set; }

        [Display(Name = " آدرس فرستنده")]
        [Required(ErrorMessage = "نوشتن آدرس فرستنده الزامی است")]
        public string SenderAddress { get; set; }

        [Display(Name = " گیرنده")]
        [Required(ErrorMessage = "تعیین گیرنده الزامی است")]
        public long ReceiverId { get; set; }
        [Display(Name = " آدرس گیرنده")]
        [Required(ErrorMessage = "نوشتن آدرس گیرنده الزامی است")]
        public string ReceiverAddress { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }


        [Display(Name = "مبدأ")]
        public string? OriginCity { get; set; }

        [Display(Name = "مقصد")]
        public string? DestinationCity { get; set; }

        public int? WeightRangeId { get; set; }
        public string? Dimensions { get; set; }
        public short NatureId { get; set; }

        // 1- نقدی
        // 2- اعتباری
        // 3- پسکرایه
        [Display(Name = " نوع پرداخت")]
        public short? SettelmentType { get; set; }
        public string? LastStatusDescription { get; set; }


        [Display(Name = "کاربر ثبت کننده")]
        [Required(ErrorMessage = " کاربر ثبت کننده شناسس نشد")]
        public string CreatedBy { get; set; }

        public bool Setteled { get; set; } = false;

        public string? SenderName { get; set; }
        public string? SenderPhone { get; set; }
        public string? SenderNationalCode { get; set; }
        public string? ReciverName { get; set; }
        public string? ReciverPhone { get; set; }
        public string? ServiceName { get; set; }

        public string userId { get; set; } = "";



        public long? PartyId { get; set; }
        public long? BusinessPartyPersonId { get; set; }


        public int qty { get; set; } = 1;
        public float Weight { get; set; } = 0.5f;

        public long BillPrice { get; set; } = 0;
        public long SharePrice { get; set; } = 0;
        public long OtherCost { get; set; } = 0;

        public long TotalPrice => BillPrice + SharePrice + OtherCost;

        public string strBillPrice { get; set; }
        public string strSharePrice { get; set; }
        public string strOtherCost { get; set; }
    }
}
