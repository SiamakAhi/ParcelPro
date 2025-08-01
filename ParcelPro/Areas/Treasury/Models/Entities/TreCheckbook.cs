﻿using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Treasury.Models.Entities
{
    public class TreCheckbook
    {
        [Display(Name = "شناسه")]
        public Guid Id { get; set; }

        [Display(Name = "شناسه فروشنده")]
        public long SellerId { get; set; }


        [Display(Name = "سریال دسته چک")]
        public string CheckbookNumber { get; set; }

        [Display(Name = "شناسه حساب بانکی")]
        public int BankAccountId { get; set; }

        [Display(Name = "اولین سریال چک")]
        public string FirstCheckSerial { get; set; }

        [Display(Name = "آخرین سریال چک")]
        public string LastCheckSerial { get; set; }

        [Display(Name = "تاریخ صدور")]
        public DateTime IssueDate { get; set; }

        [Display(Name = "تعداد چک‌ها")]
        public int NumberOfChecks { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual kh_BankAccount BankAccount { get; set; }

    }
}
