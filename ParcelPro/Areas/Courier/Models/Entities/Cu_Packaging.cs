using ParcelPro.Areas.Warehouse.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Models.Entities
{
    public class Cu_Packaging
    {
        public int Id { get; set; }
        public long SellerId { get; set; }

        [Display(Name = "کد بسته بندی")]
        public string? PackageCode { get; set; }

        [Display(Name = "بسته بندی")]
        public string Name { get; set; }

        [Display(Name = "قیمت")]
        public long Price { get; set; } = 0;

        [Display(Name = "برای بار خارجی")]
        public bool ForExport { get; set; } = false;

        [Display(Name = "دسته بندی در انبار")]
        public long? WarehouseProductCategoryId { get; set; }
        public Wh_ProductCategory? ProductCategory { get; set; }
    }
}
