﻿using ParcelPro.Areas.Accounting.AccountingInterfaces;
using ParcelPro.Areas.Accounting.Dto;
using ParcelPro.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ParcelPro.Areas.Accounting.Controllers
{
    [Area("Accounting")]
    public class AccExportController : Controller
    {
        private readonly IAccExportService _export;
        private readonly IGeneralService _gs;

        public AccExportController(IAccExportService export, IGeneralService gs)
        {
            _export = export;
            _gs = gs;
        }

        [HttpGet]
        public async Task<IActionResult> ExportKol(DocFilterDto filter)
        {
            string userName = User.Identity.Name;
            var userSett = await _gs.GetUserSettingAsync(userName);
            if (userSett == null || userSett.ActiveSellerId == null || userSett.ActiveSellerPeriod == null) { return Ok(); }
            filter.SellerId = userSett.ActiveSellerId.Value;
            filter.PeriodId = userSett.ActiveSellerPeriod.Value;
            var fileContent = await _export.Export_browserKolAsync(filter);
            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Kol_Report.xlsx");
        }

        [HttpGet]
        public async Task<IActionResult> ExportMoein(DocFilterDto filter)
        {
            string userName = User.Identity.Name;
            var userSett = await _gs.GetUserSettingAsync(userName);
            if (userSett == null || userSett.ActiveSellerId == null || userSett.ActiveSellerPeriod == null) { return Ok(); }
            filter.SellerId = userSett.ActiveSellerId.Value;
            filter.PeriodId = userSett.ActiveSellerPeriod.Value;
            var fileContent = await _export.Export_browserMoeinAsync(filter);
            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Moein_Report.xlsx");
        }

        [HttpGet]
        public async Task<IActionResult> ExportTafsil(DocFilterDto filter)
        {
            string userName = User.Identity.Name;
            var userSett = await _gs.GetUserSettingAsync(userName);
            if (userSett == null || userSett.ActiveSellerId == null || userSett.ActiveSellerPeriod == null) { return Ok(); }
            filter.SellerId = userSett.ActiveSellerId.Value;
            filter.PeriodId = userSett.ActiveSellerPeriod.Value;
            var fileContent = await _export.Export_browserTafsilAsync(filter);
            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Tafsil_Report.xlsx");
        }
        [HttpGet]
        public async Task<IActionResult> ExportRooznameh(DocFilterDto filter)
        {
            string userName = User.Identity.Name;
            var userSett = await _gs.GetUserSettingAsync(userName);
            if (userSett == null || userSett.ActiveSellerId == null || userSett.ActiveSellerPeriod == null) { return Ok(); }
            filter.SellerId = userSett.ActiveSellerId.Value;
            filter.PeriodId = userSett.ActiveSellerPeriod.Value;
            var fileContent = await _export.Export_DocArticlesAsync(filter);
            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Rooznameh.xlsx");
        }
    }
}
