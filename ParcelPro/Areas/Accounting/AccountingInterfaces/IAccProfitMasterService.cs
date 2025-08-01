﻿using ParcelPro.Areas.Accounting.Dto;

namespace ParcelPro.Areas.Accounting.AccountingInterfaces
{
    public interface IAccProfitMasterService
    {
        Task<List<ProfitMasterDto>> NetIncomeAsync(long sellerId, int periodId, List<int>? incomeGroups, string? startDate = null, string? endDate = null);
        Task<List<ProfitMasterDto>> ProfitReportAsync(ProfitReportSetting profitSetting);

    }
}
