using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ParcelPro.Areas.Courier.CuurierInterfaces;
using ParcelPro.Areas.Courier.Dto.BusinessPartnersDto;
using ParcelPro.Models;

namespace ParcelPro.Areas.Courier.CuurierServices
{
    public class BusinessPartnerService : IBusinessPartnerService
    {
        private readonly AppDbContext _db;

        public BusinessPartnerService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<SelectList> SelectList_BusinessPartnersAsync(long SellerId)
        {
            var data = await _db.Cu_BusinessPartners.Where(n => n.SellerId == SellerId)
                 .OrderBy(n => n.Name).ToListAsync();

            return new SelectList(data, "Id", "Name");
        }

        public async Task<BusinessPartnerDto> GetBusinessPartnerDtoAsync(int id)
        {
            var partner = await _db.Cu_BusinessPartners.AsNoTracking().SingleOrDefaultAsync(n => n.Id == id);

            BusinessPartnerDto dto = new BusinessPartnerDto();
            dto.Id = partner.Id;
            dto.Address = partner.Address;
            dto.PersonId = partner.PersonId;
            dto.PhoneNumber = partner.PhoneNumber;
            dto.CityId = partner.CityId;
            dto.IsActive = partner.IsActive;

            return dto;

        }
    }
}
