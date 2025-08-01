using Microsoft.AspNetCore.Mvc.Rendering;
using ParcelPro.Areas.Client.Dto;
using ParcelPro.Areas.Courier.Dto;
using ParcelPro.Areas.Courier.Dto.ContractDto;

namespace ParcelPro.Areas.Client.ClientInterfacses
{
    public interface IClientPanelService
    {
        Task<SaleContractUserDto> GetClientUserInfoAsync(string userId);
        Task<SelectList> SelectList_ClientRoute(long partyId);
        Task<SelectList> SelectList_ClientCustomers(long partyId);
        Task<SelectList> SelectList_ClientUsedProvincesAsync(long partyId);
        Task<SelectList> SelectList_ClientUsedCitiesAsync(long partyId, int? provinceId = null);
        Task<List<AddressCityDto>> GetCitiesAsync(long partyId, int? provinceId = null);
        IQueryable<ViewBillOfLadings> GetClientWaybillsAsQuery(WaybillFilterDto filter);
    }
}
