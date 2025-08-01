using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Models.Entities
{
    public class Cu_CargoManifest
    {
        public long Id { get; set; }
        public long SellerId { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime Date { get; set; }

        [Display(Name = "تاریخ رهسپاری")]
        public DateTime? TransportDate { get; set; }

        [Display(Name = "نوع رهسپاری")]
        public int TransportType { get; set; }


        [Display(Name = "هاب مبدا")]
        public Guid OriginHubId { get; set; }
        public virtual Cu_Hub OriginHub { get; set; }

        [Display(Name = "هاب مقصد")]
        public Guid DestinationHubId { get; set; }
        public virtual Cu_Hub DestinationHub { get; set; }

        [Display(Name = "جمع تعداد بسته‌ها")]
        public int TotalPackages { get; set; } = 0;
        public decimal TotalWeight { get; set; } = 0;

        [Display(Name = "تعداد بارنامه‌ها")]
        public int NumberOfWaybills { get; set; } = 0;

        [Display(Name = " خودروی حمل")]
        public int? VehicleId { get; set; }
        public virtual Cu_Vehicle? Vehicle { get; set; }

        [Display(Name = " راننده حمل")]
        public int? DriverId { get; set; }
        public virtual Cu_Driver? Driver { get; set; }

        // برای ارسال هوایی
        [Display(Name = "نام شرکت هواپیمایی")]
        public string? AirlineCompany { get; set; }

        [Display(Name = "شماره بارنامه هوایی")]
        public string? AirWaybillNumber { get; set; }

        [Display(Name = "مبلغ بارنامه هوایی")]
        public long? AirWaybillPrice { get; set; } = 0;

        //

        [Display(Name = "نام تحویل‌دهنده")]
        public string? TahvilDahandeh { get; set; }

        [Display(Name = "نام تحویل‌گیرنده")]
        public string? ReceiverName { get; set; }
        public short status { get; set; } = 1;

        [Display(Name = "تاریخ و زمان رسیدن به هاب مقصد")]
        public DateTime? ArrivalTime { get; set; }

        public virtual ICollection<Cu_Consignment> Parcels { get; set; }
    }

    public enum TransportType
    {
        [Display(Name = "زمینی")]
        Ground,

        [Display(Name = "هوایی")]
        Air,

        [Display(Name = "ریلی")]
        Rail,

        [Display(Name = "دریایی")]
        Sea
    }
}