using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Dto
{
    public class InsuranceSettingsDto
    {
        [Key] // این فیلد کلید اصلی (Primary Key) است.
        public int Id { get; set; }
        public long SellerId { get; set; }

        [Display(Name = "مبلغ پایه حق بیمه (ریال)")]
        public decimal BaseCost { get; set; } // مبلغ پایه حق بیمه

        [Display(Name = "افزونده حق بیمه برای هر واحد (ریال)")]
        public decimal IncrementPerUnit { get; set; } // افزونده برای هر واحد

        [Display(Name = "حد آستانه ارزش اعلامی (تومان)")]
        public decimal ThresholdAmount { get; set; } // مقدار آستانه (مثال: 1 میلیون تومان)
    }
}
