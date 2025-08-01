using Microsoft.AspNetCore.Mvc.Rendering;
using ParcelPro.Areas.Courier.Dto.ContractDto;

namespace ParcelPro.Areas.Courier.CuurierInterfaces
{
    public interface ICu_SaleContractService
    {
        Task<SelectList> SelectList_ContractsAsync(long SellerId);
        Task<clsResult> AddContract(SaleContractDto dto);
        Task<clsResult> UpdateContract(SaleContractDto dto);
        Task<clsResult> DeleteContract(int id);
        Task<List<SaleContractDto>> GetContracts(long sellerId);
        Task<SaleContractDto?> GetContractById(int id);

        //----------------------------------------------------------------------------------
        //---------- Client User ------------------------------------- Client User ---------
        //----------------------------------------------------------------------------------
        Task<List<SaleContractUserDto>> ClientUsersAsync(long SellerId);
        Task<clsResult> AddClidentUserAsync(AddClientUserDto dto);
    }
}
