using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ParcelPro.Areas.Treasury.TreasuryInterfaces;
using ParcelPro.Models;

namespace ParcelPro.Areas.Treasury.TreasuryServices
{
    public class TreasuryGeneralData : ITreasuryGeneralData
    {
        private readonly AppDbContext _db;

        public TreasuryGeneralData(AppDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<SelectList> SelectList_CurrenciesAsync()
        {
            var lst = await _db.Currencies.Select(n => new { Id = n.Id, Name = n.Name }).ToListAsync();
            return new SelectList(lst, "Id", "Name");
        }

        public async Task<SelectList> SelectList_BankAccountsAsync(long SellerId)
        {
            var accounts = await _db.BankAccounts.Where(x => x.SellerId == SellerId)
                .Select(n => new { Id = n.Id, Name = n.Bank.Name + " - " + n.AccountName + " " + n.AccountNumber })
                .ToListAsync();
            return new SelectList(accounts, "Id", "Name");
        }

        public async Task<SelectList> SelectList_PosDevicesAsync(long SellerId)
        {
            var PosDevices = await _db.BankPosUcs.Where(x => x.SellerId == SellerId)
                .Select(n => new { Id = n.Id, Name = n.Name })
                .ToListAsync();
            return new SelectList(PosDevices, "Id", "Name");
        }
        public async Task<SelectList> SelectList_BranchPOSesAsync(Guid branchId)
        {
            var PosDevices = await _db.BankPosUcs.Where(x => x.BranchId == branchId)
                .Select(n => new { Id = n.Id, Name = n.Name })
                .ToListAsync();
            return new SelectList(PosDevices, "Id", "Name");
        }
        public async Task<SelectList> SelectList_PaymentMethodsAsync()
        {
            var data = await _db.TreOperations
                .Where(n => !n.IsPay).Select(n => new { Id = n.Id, Name = n.OperationName }).ToListAsync();

            return new SelectList(data, "Id", "Name");
        }
    }
}
