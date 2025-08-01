using Microsoft.AspNetCore.Mvc.Rendering;

namespace ParcelPro.Areas.Treasury.TreasuryInterfaces
{
    public interface ITreasuryGeneralData
    {
        Task<SelectList> SelectList_CurrenciesAsync();
        Task<SelectList> SelectList_BankAccountsAsync(long SellerId);
        Task<SelectList> SelectList_PosDevicesAsync(long SellerId);
        Task<SelectList> SelectList_BranchPOSesAsync(Guid branchId);
        Task<SelectList> SelectList_PaymentMethodsAsync();
    }
}
