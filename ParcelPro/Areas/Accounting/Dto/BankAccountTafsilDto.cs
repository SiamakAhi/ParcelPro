﻿using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Accounting.Dto
{
    public class BankAccountTafsilDto
    {
        public long Id { get; set; }

        [Display(Name = "کد تفصیلی")]
        public string? Code { get; set; }

        [Display(Name = "نام *")]
        [Required(ErrorMessage = "نام حساب تفصیلی را بنویسید ؟")]
        public string Name { get; set; }

        [Display(Name = "شرح")]
        public string? Description { get; set; }

        [Display(Name = "گروه *")]
        [Required(ErrorMessage = "حداقل یک گروه تفصیلی را انتحاب کنید")]
        public int[]? intGroupsId { get; set; }

        public string? strGroupsId { get; set; }
        public string[]? GroupsName { get; set; }
        public bool? IsPerson { get; set; }

        public long? SellerId { get; set; }
        public int? BankAccountId { get; set; }
    }
}
