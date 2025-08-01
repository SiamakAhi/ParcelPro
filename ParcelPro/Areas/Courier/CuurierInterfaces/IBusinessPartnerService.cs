using Microsoft.AspNetCore.Mvc.Rendering;
using ParcelPro.Areas.Courier.Dto.BusinessPartnersDto;

namespace ParcelPro.Areas.Courier.CuurierInterfaces
{
    public interface IBusinessPartnerService
    {

        Task<SelectList> SelectList_BusinessPartnersAsync(long SellerId);
        Task<BusinessPartnerDto> GetBusinessPartnerDtoAsync(int id);
    }
}
