﻿using ParcelPro.Areas.Courier.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Warehouse.Models.Entities
{
    public class Wh_Warehouse
    {
        public long WarehouseId { get; set; }

        [Display(Name = "شناسه فروشنده")]
        public long SellerId { get; set; }

        [Display(Name = "کد انبار")]
        public string WarehouseCode { get; set; } = "";

        [Display(Name = "نام انبار")]
        public string WarehouseName { get; set; }

        [Display(Name = "آدرس")]
        public string? Address { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }

        public short WarehouseType { get; set; } = 0;

        public int? MoeinId { get; set; }
        public long? TafsilId { get; set; }

        public Guid? BranchId { get; set; }
        public virtual Cu_Branch? WarehouseBranch { get; set; }


        // Navigation Properties
        public ICollection<Wh_WarehouseLocation>? WarehouseLocations { get; set; }
        public ICollection<Wh_WarehouseDocument> WarehouseDocuments { get; set; }
    }

}
