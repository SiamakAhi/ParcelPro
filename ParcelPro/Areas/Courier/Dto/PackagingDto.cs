using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Dto
{
    public class PackagingDto
    {
        public int Id { get; set; }
        public long SellerId { get; set; }

        [Display(Name = "کد بسته بندی")]
        public string? PackageCode { get; set; }

        [Display(Name = "بسته بندی")]
        [Required(ErrorMessage = "نام بسته بندی را وارد کنید")]
        public string Name { get; set; }

        [Display(Name = "قیمت")]
        public long Price { get; set; } = 0;

        [Display(Name = "برای بار خارجی")]
        public bool ForExport { get; set; } = false;

        [Display(Name = "دسته بندی در انبار")]
        public long? WarehouseProductCategoryId { get; set; }

        [Display(Name = "نام گروه کالایی درانبار")]
        public string? WarehouseProductCategoryName { get; set; }
    }
}
