using Microsoft.AspNetCore.Mvc.Rendering;
using ParcelPro.Areas.Accounting.Dto;

namespace ParcelPro.Areas.Treasury.TreasuryInterfaces
{
    public interface ITreBaseService
    {
        Task<SelectList> SelectList_BanksAsync();
        Task<SelectList> SelectList_BankAccountsAsync();
        Task<List<BankAccountDto>> GetBankAccountsByBankIdAsync(int bankId);
    }
}
