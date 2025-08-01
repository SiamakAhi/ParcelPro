using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Dto.BusinessPartnersDto
{
    public class BusinessPartnerDto
    {
        [Key]
        public int Id { get; set; }

        public long SellerId { get; set; }

        [Required(ErrorMessage = "نام الزامی است")]
        public string Name { get; set; }

        [Display(Name = "کد اقتصادی")]
        public string EconomicCode { get; set; }

        [Display(Name = "شماره تلفن")]
        public string PhoneNumber { get; set; }

        public string? Address { get; set; }

        public int? CityId { get; set; }
        public long PersonId { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
    }
}
