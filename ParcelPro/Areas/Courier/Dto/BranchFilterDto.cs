using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Dto
{
    public class BranchFilterDto
    {
        public Guid? BranchId { get; set; }
        public long SellerId { get; set; }

        [Display(Name = "شهر")]
        public int? CityId { get; set; }

        [Display(Name = "نوع نمایندگی")]
        public bool? IsOwnership { get; set; }

        [Display(Name = "صادرکننده بارنامه")]
        public bool? IsBillOfLadingIssuer { get; set; }

        [Display(Name = "ناوگان حمل درون شهری")]
        public bool? IsUrbanFleet { get; set; }

        [Display(Name = "ناوگان حمل بین شهری زمینی")]
        public bool? IsIntercityFleet { get; set; }

        [Display(Name = "تجزیه و مبادلات مرسولات")]
        public bool? IsHub { get; set; } = false;
    }
}
