using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Treasury.Models.Entities
{
    public class TreCashier
    {
        [Display(Name = "شناسه")]
        public Guid Id { get; set; }

        [Display(Name = "شناسه فروشنده")]
        public long SellerId { get; set; }
        public Guid? BranchId { get; set; }

        [Display(Name = "شناسه شخص")]
        public long PersonId { get; set; }


        [Display(Name = "واحد سازمانی")]
        public int? OrganizationUnit { get; set; }

        [Display(Name = "شناسه حساب معین")]
        public int? AccountId { get; set; }

        [Display(Name = "شناسه حساب تفصیلی")]
        public long TafsilId { get; set; }

        [Display(Name = "مجوز دریافت دارد")]
        public bool HasReceiptPermission { get; set; } = true;

        [Display(Name = "مجوز پرداخت دارد")]
        public bool HasPaymentPermission { get; set; } = true;

        [Display(Name = "مجوز صدور چک دارد")]
        public bool HasCheckIssuancePermission { get; set; } = false;

        [Display(Name = "مجوز ویرایش اطلاعات صندوق دارد")]
        public bool HasCashboxEditPermission { get; set; } = true;

        public Guid? CachboxId { get; set; }
        public virtual TreCashBox? CashBox { get; set; }
        public virtual Party Person { get; set; }


    }
}
