﻿using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Dto
{
    public class ViewBillOfLadings
    {
        public Guid Id { get; set; }
        public long SellerId { get; set; }

        [Display(Name = "شماره بارنامه")]
        public string WaybillNumber { get; set; }

        [Display(Name = "شماره بارنامه مرجع")]
        public string? ReferenceNumber { get; set; }
        public string? CustomerKeyword { get; set; }

        [Display(Name = "تاریخ صدور")]
        public DateTime IssuanceDate { get; set; }
        public TimeSpan IssuanceTime { get; set; }

        [Display(Name = " مبدأ")]
        public string OriginCity { get; set; }
        [Display(Name = " مقصد")]
        public string DestinationCity { get; set; }
        [Display(Name = " سرویس")]
        public string ServiceName { get; set; }

        [Display(Name = "شعبه صادرکننده")]
        public string OriginBranchName { get; set; }


        [Display(Name = " فرستنده")]
        public long SenderId { get; set; }
        [Display(Name = " فرستنده")]
        public string SenderName { get; set; }
        [Display(Name = " آدرس فرستنده")]
        public string SenderAddress { get; set; }

        [Display(Name = " گیرنده")]
        public long ReceiverId { get; set; }
        [Display(Name = " گیرنده")]
        public string ReceiverName { get; set; }
        [Display(Name = " آدرس گیرنده")]
        public string ReceiverAddress { get; set; }
        public string? ReceiverPhone { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }
        public string? RouteName { get; set; }


        [Display(Name = "تعداد تحویل شده")]
        public int DeliveredCount { get; set; } = 0;

        [Display(Name = "تعداد")]
        public int ConsigmentCount { get; set; } = 0;

        // 1- نقدی
        // 2- اعتباری
        // 3- پسکرایه
        [Display(Name = " نوع پرداخت")]
        public string? SettelmentTypeName { get; set; }
        public short? SettelmentTypeId { get; set; }
        public bool IsSetteled { get; set; }
        public string? LastStatusDescription { get; set; }

        [Display(Name = " وضعیت بارنامه")]
        public short BillOfLadingStatusId { get; set; } = 1;

        public Guid OriginHubId { get; set; }
        [Display(Name = "هاب مبدأ")]
        public string OriginHubName { get; set; }

        [Display(Name = "هاب مقصد")]
        public string? DestinationHubName { get; set; }
        public Guid? DestinationHubId { get; set; }

        [Display(Name = "کاربر ثبت کننده")]
        public string CreatedBy { get; set; }

        [Display(Name = "کاربر ثبت کننده")]
        public string? CreatedByFullName { get; set; }

        [Display(Name = "تاریخ به روزرسانی")]
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "کاربر ادیت کننده")]
        public string? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public long TotalDiscount { get; set; } = 0;
        public long TotalCost { get; set; } = 0;
        public long? PayedAmount { get; set; } = 0;

        public DateTime? tg_DeliveryDate { get; set; }
        public string? tg_Name { get; set; }
        public string? tg_Phone { get; set; }
        public string? tg_NationalityCode { get; set; }
        public string? tg_Description { get; set; }
        public string? DeliveryErrorMessage { get; set; }
        public byte[]? tg_SignatureData { get; set; }
        public string? tg_CourierManUserName { get; set; }
        public bool Delivered { get; set; } = false;

        public long BillPrice
        {
            get => TotalCost - TotalDiscount;
        }
        public Guid? DistributerBranchId { get; set; }
        public string? DistributerBranch { get; set; }
        public float TotalWeight { get; set; }
        public List<ConsigmentDto> Parcels { get; set; } = new List<ConsigmentDto>();
    }
}
