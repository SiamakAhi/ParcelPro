using ClosedXML.Excel;
using ParcelPro.Areas.Accounting.AccountingInterfaces;
using ParcelPro.Areas.Accounting.Dto;
using ParcelPro.Interfaces;

namespace ParcelPro.Areas.Accounting.AccountingServices
{
    public class AccExportService : IAccExportService
    {
        private readonly IAccountingReportService _report;

        public AccExportService(IAccountingReportService report, IGeneralService gs)
        {
            _report = report;

        }

        public async Task<byte[]> Export_browserKolAsync(DocFilterDto filter)
        {
            // دریافت داده‌ها از گزارش
            var report = await _report.Report_KolAsync(filter);
            string rpStartDate = "ابتدای دوره ";
            string rpEndDate = "";
            if (!string.IsNullOrEmpty(filter.strStartDate))
                rpStartDate = filter.strStartDate.PersianToLatin().LatinToPersian();
            if (!string.IsNullOrEmpty(filter.strEndDate))
                rpEndDate = $" لغایت {filter.strEndDate.PersianToLatin().LatinToPersian()}";
            string reportTitle = $"گزارش حساب کل {rpStartDate} {rpEndDate}";
            using (var workbook = new XLWorkbook())
            {
                workbook.RightToLeft = true;
                var worksheet = workbook.Worksheets.Add("Account Browser Kol");

                // مخفی کردن Gridlines
                worksheet.ShowGridLines = false;
                worksheet.Style.Font.FontName = "Tahoma";
                // تنظیم Layout راست‌چین برای شیت
                worksheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                // اضافه کردن عنوان گزارش
                worksheet.Cell(1, 1).Value = reportTitle;
                worksheet.Range(1, 1, 1, 7).Merge();
                worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 1).Style.Font.FontSize = 16;
                worksheet.Cell(1, 1).Style.Font.FontName = "B Nazanin";

                // تنظیم عنوان ستون‌ها (ردیف دوم)
                worksheet.Cell(2, 1).Value = "ردیف";
                worksheet.Cell(2, 2).Value = "حساب";
                worksheet.Cell(2, 3).Value = "بدهکار طی دوره";
                worksheet.Cell(2, 4).Value = "بستانکار طی دوره";
                worksheet.Cell(2, 5).Value = "بدهکار مانده";
                worksheet.Cell(2, 6).Value = "بستانکار مانده";
                worksheet.Cell(2, 7).Value = "ماهیت حساب";

                // تنظیم استایل هدر
                var headerRange = worksheet.Range(2, 1, 2, 7);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.DarkGray;
                headerRange.Style.Font.FontColor = XLColor.White;
                headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                headerRange.Style.Border.OutsideBorderColor = XLColor.Black;
                headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                headerRange.Style.Border.InsideBorderColor = XLColor.Black;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                headerRange.Style.Font.FontName = "B Nazanin"; // تنظیم فونت

                // پر کردن داده‌ها
                int row = 3; // ردیف از 3 شروع می‌شود چون ردیف اول برای عنوان گزارش و ردیف دوم برای عنوان ستون‌هاست
                int rowNumber = 1;
                decimal totalBed = 0, totalBes = 0, totalMandehBed = 0, totalMandehBes = 0;

                foreach (var x in report)
                {
                    worksheet.Cell(row, 1).Value = rowNumber;
                    worksheet.Cell(row, 2).Value = x.KolName;
                    worksheet.Cell(row, 3).Value = x.Bed > 0 ? x.Bed : 0;
                    worksheet.Cell(row, 4).Value = x.Bes > 0 ? x.Bes : 0;
                    worksheet.Cell(row, 5).Value = x.MandehNature == 1 && x.Mandeh > 0 ? x.Mandeh : 0;
                    worksheet.Cell(row, 6).Value = x.MandehNature == 2 && x.Mandeh > 0 ? x.Mandeh : 0;
                    worksheet.Cell(row, 7).Value = x.Nature.AccToNatureName();

                    // جمع مقادیر
                    totalBed += x.Bed;
                    totalBes += x.Bes;
                    if (x.MandehNature == 1)
                    {
                        totalMandehBed += x.Mandeh;
                    }
                    else if (x.MandehNature == 2)
                    {
                        totalMandehBes += x.Mandeh;
                    }

                    // تنظیم استایل و قالب بندی عددی
                    worksheet.Cell(row, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center; // ردیف‌ها وسط‌چین شوند
                    worksheet.Cell(row, 3).Style.NumberFormat.Format = "#,##0"; // فرمت عددی برای بدهکار
                    worksheet.Cell(row, 4).Style.NumberFormat.Format = "#,##0"; // فرمت عددی برای بستانکار
                    worksheet.Cell(row, 5).Style.NumberFormat.Format = "#,##0"; // فرمت عددی برای بدهکار مانده
                    worksheet.Cell(row, 6).Style.NumberFormat.Format = "#,##0"; // فرمت عددی برای بستانکار مانده

                    // چپ‌چین کردن ستون‌های عددی
                    worksheet.Cell(row, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    worksheet.Cell(row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    worksheet.Cell(row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    worksheet.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                    // اعمال خط‌کشی به تمامی سلول‌های داده
                    var dataRange = worksheet.Range(row, 1, row, 7);
                    dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Border.OutsideBorderColor = XLColor.Black;
                    dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Border.InsideBorderColor = XLColor.Black;
                    dataRange.Style.Font.FontName = "B Nazanin"; // تنظیم فونت به همه سلول‌ها

                    row++;
                    rowNumber++;
                }

                // اضافه کردن جمع‌ها به انتهای جدول
                worksheet.Cell(row, 2).Value = "جمع کل:";
                worksheet.Cell(row, 3).Value = totalBed;
                worksheet.Cell(row, 4).Value = totalBes;
                worksheet.Cell(row, 5).Value = totalMandehBed;
                worksheet.Cell(row, 6).Value = totalMandehBes;

                // تنظیم استایل و قالب بندی عددی برای جمع‌ها
                var totalRange = worksheet.Range(row, 1, row, 7);
                totalRange.Style.Font.Bold = true;
                totalRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                totalRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                totalRange.Style.Border.OutsideBorderColor = XLColor.Black;
                totalRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                totalRange.Style.Border.InsideBorderColor = XLColor.Black;
                worksheet.Cell(row, 3).Style.NumberFormat.Format = "#,##0"; // فرمت عددی برای جمع بدهکار
                worksheet.Cell(row, 4).Style.NumberFormat.Format = "#,##0"; // فرمت عددی برای جمع بستانکار
                worksheet.Cell(row, 5).Style.NumberFormat.Format = "#,##0"; // فرمت عددی برای جمع بدهکار مانده
                worksheet.Cell(row, 6).Style.NumberFormat.Format = "#,##0"; // فرمت عددی برای جمع بستانکار مانده

                // تنظیم خودکار عرض ستون‌ها
                worksheet.Columns().AdjustToContents();

                // راست‌چین کردن محتوای غیر عددی
                worksheet.Range(3, 1, row, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                // تنظیم Layout برای چاپ در A4
                worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                worksheet.PageSetup.PaperSize = XLPaperSize.A4Paper;
                worksheet.PageSetup.CenterHorizontally = true;
                worksheet.PageSetup.Margins.Top = 0.5;
                worksheet.PageSetup.Margins.Bottom = 0.5;
                worksheet.PageSetup.Margins.Left = 0.5;
                worksheet.PageSetup.Margins.Right = 0.5;

                // ذخیره فایل Excel در بایت آرایه
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
        public async Task<byte[]> Export_browserMoeinAsync(DocFilterDto filter)
        {
            // دریافت داده‌ها از گزارش
            var report = await _report.Report_MoeinAsync(filter);
            string rpStartDate = "ابتدای دوره ";
            string rpEndDate = "";
            if (!string.IsNullOrEmpty(filter.strStartDate))
                rpStartDate = filter.strStartDate.PersianToLatin().LatinToPersian();
            if (!string.IsNullOrEmpty(filter.strEndDate))
                rpEndDate = $" لغایت {filter.strEndDate.PersianToLatin().LatinToPersian()}";
            string reportTitle = $"گزارش حساب معین {rpStartDate} {rpEndDate}";

            using (var workbook = new XLWorkbook())
            {
                workbook.RightToLeft = true;
                var worksheet = workbook.Worksheets.Add("Account Browser Moein");

                // مخفی کردن Gridlines
                worksheet.ShowGridLines = false;

                // تنظیم Layout راست‌چین برای شیت
                worksheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                worksheet.Style.Font.FontName = "Tahoma";
                // اضافه کردن عنوان گزارش
                worksheet.Cell(1, 1).Value = reportTitle;
                worksheet.Range(1, 1, 1, 7).Merge();
                worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 1).Style.Font.FontSize = 16;
                worksheet.Cell(1, 1).Style.Font.FontName = "B Nazanin";

                // تنظیم عنوان ستون‌ها (ردیف دوم)
                worksheet.Cell(2, 1).Value = "ردیف";
                worksheet.Cell(2, 2).Value = "حساب";
                worksheet.Cell(2, 3).Value = "بدهکار طی دوره";
                worksheet.Cell(2, 4).Value = "بستانکار طی دوره";
                worksheet.Cell(2, 5).Value = "بدهکار مانده";
                worksheet.Cell(2, 6).Value = "بستانکار مانده";
                worksheet.Cell(2, 7).Value = "ماهیت حساب";

                // تنظیم استایل هدر
                var headerRange = worksheet.Range(2, 1, 2, 7);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.DarkGray;
                headerRange.Style.Font.FontColor = XLColor.White;
                headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                headerRange.Style.Border.OutsideBorderColor = XLColor.Black;
                headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                headerRange.Style.Border.InsideBorderColor = XLColor.Black;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                headerRange.Style.Font.FontName = "B Nazanin"; // تنظیم فونت

                // پر کردن داده‌ها
                int row = 3; // ردیف از 3 شروع می‌شود چون ردیف اول برای عنوان گزارش و ردیف دوم برای عنوان ستون‌هاست
                int rowNumber = 1;
                decimal totalBed = 0, totalBes = 0, totalMandehBed = 0, totalMandehBes = 0;

                foreach (var x in report)
                {
                    worksheet.Cell(row, 1).Value = rowNumber;
                    worksheet.Cell(row, 2).Value = x.MoeinName;
                    worksheet.Cell(row, 3).Value = x.Bed > 0 ? x.Bed : 0;
                    worksheet.Cell(row, 4).Value = x.Bes > 0 ? x.Bes : 0;
                    worksheet.Cell(row, 5).Value = x.MandehNature == 1 && x.Mandeh > 0 ? x.Mandeh : 0;
                    worksheet.Cell(row, 6).Value = x.MandehNature == 2 && x.Mandeh > 0 ? x.Mandeh : 0;
                    worksheet.Cell(row, 7).Value = x.Nature.AccToNatureName();

                    // جمع مقادیر
                    totalBed += x.Bed;
                    totalBes += x.Bes;
                    if (x.MandehNature == 1)
                    {
                        totalMandehBed += x.Mandeh;
                    }
                    else if (x.MandehNature == 2)
                    {
                        totalMandehBes += x.Mandeh;
                    }

                    // تنظیم استایل و قالب بندی عددی
                    worksheet.Cell(row, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center; // ردیف‌ها وسط‌چین شوند
                    worksheet.Cell(row, 3).Style.NumberFormat.Format = "#,##0"; // فرمت عددی برای بدهکار
                    worksheet.Cell(row, 4).Style.NumberFormat.Format = "#,##0"; // فرمت عددی برای بستانکار
                    worksheet.Cell(row, 5).Style.NumberFormat.Format = "#,##0"; // فرمت عددی برای بدهکار مانده
                    worksheet.Cell(row, 6).Style.NumberFormat.Format = "#,##0"; // فرمت عددی برای بستانکار مانده

                    // چپ‌چین کردن ستون‌های عددی
                    worksheet.Cell(row, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    worksheet.Cell(row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    worksheet.Cell(row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    worksheet.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                    // اعمال خط‌کشی به تمامی سلول‌های داده
                    var dataRange = worksheet.Range(row, 1, row, 7);
                    dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Border.OutsideBorderColor = XLColor.Black;
                    dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Border.InsideBorderColor = XLColor.Black;
                    dataRange.Style.Font.FontName = "B Nazanin"; // تنظیم فونت به همه سلول‌ها

                    row++;
                    rowNumber++;
                }

                // اضافه کردن جمع‌ها به انتهای جدول
                worksheet.Cell(row, 2).Value = "جمع کل:";
                worksheet.Cell(row, 3).Value = totalBed;
                worksheet.Cell(row, 4).Value = totalBes;
                worksheet.Cell(row, 5).Value = totalMandehBed;
                worksheet.Cell(row, 6).Value = totalMandehBes;

                // تنظیم استایل و قالب بندی عددی برای جمع‌ها
                var totalRange = worksheet.Range(row, 1, row, 7);
                totalRange.Style.Font.Bold = true;
                totalRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                totalRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                totalRange.Style.Border.OutsideBorderColor = XLColor.Black;
                totalRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                totalRange.Style.Border.InsideBorderColor = XLColor.Black;
                worksheet.Cell(row, 3).Style.NumberFormat.Format = "#,##0"; // فرمت عددی برای جمع بدهکار
                worksheet.Cell(row, 4).Style.NumberFormat.Format = "#,##0"; // فرمت عددی برای جمع بستانکار
                worksheet.Cell(row, 5).Style.NumberFormat.Format = "#,##0"; // فرمت عددی برای جمع بدهکار مانده
                worksheet.Cell(row, 6).Style.NumberFormat.Format = "#,##0"; // فرمت عددی برای جمع بستانکار مانده

                // تنظیم خودکار عرض ستون‌ها
                worksheet.Columns().AdjustToContents();

                // راست‌چین کردن محتوای غیر عددی
                worksheet.Range(3, 1, row, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                // تنظیم Layout برای چاپ در A4
                worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                worksheet.PageSetup.PaperSize = XLPaperSize.A4Paper;
                worksheet.PageSetup.CenterHorizontally = true;
                worksheet.PageSetup.Margins.Top = 0.5;
                worksheet.PageSetup.Margins.Bottom = 0.5;
                worksheet.PageSetup.Margins.Left = 0.5;
                worksheet.PageSetup.Margins.Right = 0.5;

                // ذخیره فایل Excel در بایت آرایه
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        public async Task<byte[]> Export_browserTafsilAsync(DocFilterDto filter)
        {
            // دریافت داده‌ها از گزارش
            var report = await _report.Report_Tafsil4Async(filter);
            filter.ReportLevel = filter.ReportLevel;
            // تنظیم تاریخ‌های شروع و پایان گزارش
            string rpStartDate = "ابتدای دوره ";
            string rpEndDate = "";
            if (!string.IsNullOrEmpty(filter.strStartDate))
                rpStartDate = filter.strStartDate.PersianToLatin().LatinToPersian();
            if (!string.IsNullOrEmpty(filter.strEndDate))
                rpEndDate = $" لغایت {filter.strEndDate.PersianToLatin().LatinToPersian()}";
            string reportTitle = $"گزارش تفصیل {rpStartDate} {rpEndDate}";

            using (var workbook = new XLWorkbook())
            {
                workbook.RightToLeft = true;
                var worksheet = workbook.Worksheets.Add("Account Browser Tafsil");

                worksheet.ShowGridLines = false;
                worksheet.Style.Font.FontName = "Tahoma";
                worksheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                worksheet.Cell(1, 1).Value = reportTitle;
                worksheet.Range(1, 1, 1, 7).Merge();
                worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 1).Style.Font.FontSize = 16;
                worksheet.Cell(1, 1).Style.Font.FontName = "B Nazanin";

                int colcount = 3; // حداقل کل و معین
                if (filter.ReportLevel.HasValue && filter.ReportLevel > 3)
                {
                    colcount = filter.ReportLevel.Value - 1;
                }
                // تنظیم عنوان ستون‌ها
                worksheet.Cell(2, 1).Value = "ردیف";
                worksheet.Cell(2, 2).Value = "کل";
                worksheet.Cell(2, 3).Value = "معین";

                // افزودن ستون‌های تفصیل به صورت پویا بر اساس ReportLevel
                int colIndex = 4;
                if (filter.ReportLevel >= 4) worksheet.Cell(2, colIndex++).Value = "تفصیل 4";
                if (filter.ReportLevel >= 5) worksheet.Cell(2, colIndex++).Value = "تفصیل 5";
                if (filter.ReportLevel >= 6) worksheet.Cell(2, colIndex++).Value = "تفصیل 6";
                if (filter.ReportLevel >= 7) worksheet.Cell(2, colIndex++).Value = "تفصیل 7";
                if (filter.ReportLevel >= 8) worksheet.Cell(2, colIndex++).Value = "تفصیل 8";

                // افزودن ستون‌های ثابت
                worksheet.Cell(2, colIndex++).Value = "بدهکار طی دوره";
                worksheet.Cell(2, colIndex++).Value = "بستانکار طی دوره";
                worksheet.Cell(2, colIndex++).Value = "بدهکار مانده";
                worksheet.Cell(2, colIndex++).Value = "بستانکار مانده";
                worksheet.Cell(2, colIndex).Value = "ماهیت حساب";

                // تنظیم استایل هدر
                var headerRange = worksheet.Range(2, 1, 2, colIndex);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.DarkGray;
                headerRange.Style.Font.FontColor = XLColor.White;
                headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                headerRange.Style.Border.OutsideBorderColor = XLColor.Black;
                headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                headerRange.Style.Border.InsideBorderColor = XLColor.Black;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                headerRange.Style.Font.FontName = "B Nazanin"; // تنظیم فونت

                // پر کردن داده‌ها
                int row = 3; // ردیف از 3 شروع می‌شود چون ردیف اول برای عنوان گزارش و ردیف دوم برای عنوان ستون‌هاست
                int rowNumber = 1;
                decimal totalBed = 0, totalBes = 0, totalMandehBed = 0, totalMandehBes = 0;

                foreach (var x in report)
                {
                    colIndex = 1;
                    worksheet.Cell(row, colIndex++).Value = rowNumber;
                    worksheet.Cell(row, colIndex++).Value = x.KolName;
                    worksheet.Cell(row, colIndex++).Value = x.MoeinName;

                    // افزودن تفصیل‌ها به‌صورت پویا
                    if (filter.ReportLevel >= 4) worksheet.Cell(row, colIndex++).Value = x.Tafsil4Name;
                    if (filter.ReportLevel >= 5) worksheet.Cell(row, colIndex++).Value = x.Tafsil5Name;
                    if (filter.ReportLevel >= 6) worksheet.Cell(row, colIndex++).Value = x.Tafsil6Name;
                    if (filter.ReportLevel >= 7) worksheet.Cell(row, colIndex++).Value = x.Tafsil7Name;
                    if (filter.ReportLevel >= 8) worksheet.Cell(row, colIndex++).Value = x.Tafsil8Name;

                    worksheet.Cell(row, colIndex++).Value = x.Bed > 0 ? x.Bed.ToPrice() : 0;
                    worksheet.Cell(row, colIndex++).Value = x.Bes > 0 ? x.Bes.ToPrice() : 0;
                    worksheet.Cell(row, colIndex++).Value = x.MandehNature == 1 && x.Mandeh > 0 ? x.Mandeh.ToPrice() : 0;
                    worksheet.Cell(row, colIndex++).Value = x.MandehNature == 2 && x.Mandeh > 0 ? x.Mandeh.ToPrice() : 0;
                    worksheet.Cell(row, colIndex).Value = x.Nature.AccToNatureName();

                    // جمع مقادیر
                    totalBed += x.Bed;
                    totalBes += x.Bes;
                    if (x.MandehNature == 1)
                    {
                        totalMandehBed += x.Mandeh;
                    }
                    else if (x.MandehNature == 2)
                    {
                        totalMandehBes += x.Mandeh;
                    }

                    // اعمال استایل و خط‌کشی
                    var dataRange = worksheet.Range(row, 1, row, colIndex);
                    dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Border.OutsideBorderColor = XLColor.Black;
                    dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Border.InsideBorderColor = XLColor.Black;
                    dataRange.Style.Font.FontName = "B Nazanin";

                    row++;
                    rowNumber++;
                }

                // اضافه کردن جمع‌ها به انتهای جدول
                colIndex = 2 + colcount;
                worksheet.Cell(row, 2).Value = "جمع کل:";
                worksheet.Cell(row, colIndex++).Value = totalBed.ToPrice();
                worksheet.Cell(row, colIndex++).Value = totalBes.ToPrice();
                worksheet.Cell(row, colIndex++).Value = totalMandehBed.ToPrice();
                worksheet.Cell(row, colIndex++).Value = totalMandehBes.ToPrice();

                // استایل دادن به ردیف جمع
                var totalRange = worksheet.Range(row, 1, row, colIndex);
                totalRange.Style.Font.Bold = true;
                totalRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                totalRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                totalRange.Style.Border.OutsideBorderColor = XLColor.Black;
                totalRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                totalRange.Style.Border.InsideBorderColor = XLColor.Black;

                // تنظیم خودکار عرض ستون‌ها
                worksheet.Columns().AdjustToContents();

                // راست‌چین کردن محتوای غیر عددی
                worksheet.Range(3, 1, row, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                // تنظیم Layout برای چاپ در A4
                worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                worksheet.PageSetup.PaperSize = XLPaperSize.A4Paper;
                worksheet.PageSetup.CenterHorizontally = true;
                worksheet.PageSetup.Margins.Top = 0.5;
                worksheet.PageSetup.Margins.Bottom = 0.5;
                worksheet.PageSetup.Margins.Left = 0.5;
                worksheet.PageSetup.Margins.Right = 0.5;

                // ذخیره فایل Excel در بایت آرایه
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        public async Task<byte[]> Export_DocArticlesAsync(DocFilterDto filter)
        {
            var articles = await _report.GetArticlesAsync(filter);

            using (var workbook = new XLWorkbook())
            {
                workbook.RightToLeft = true;

                var worksheet = workbook.Worksheets.Add("روزنامه");

                // مخفی کردن Gridlines
                worksheet.ShowGridLines = false;
                worksheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                worksheet.Style.Font.FontName = "Tahoma";
                // اضافه کردن عنوان ستون‌ها
                worksheet.Cell(1, 1).Value = "ردیف";
                worksheet.Cell(1, 2).Value = "شماره سند";
                worksheet.Cell(1, 3).Value = "تاریخ سند";
                worksheet.Cell(1, 4).Value = "کد حساب کل";
                worksheet.Cell(1, 5).Value = "حساب کل";
                worksheet.Cell(1, 6).Value = "کد حساب معین";
                worksheet.Cell(1, 7).Value = "حساب معین";

                int colIndex = 7;

                // بررسی وجود تفصیلی‌ها و اضافه کردن ستون‌ها به صورت پویا
                bool hasTafsil4 = articles.Any(n => n.Tafsil4Id != null);
                bool hasTafsil5 = articles.Any(n => n.Tafsil5Id != null);
                bool hasTafsil6 = articles.Any(n => n.Tafsil6Id != null);
                bool hasTafsil7 = articles.Any(n => n.Tafsil7Id != null);
                bool hasTafsil8 = articles.Any(n => n.Tafsil8Id != null);

                if (hasTafsil4) worksheet.Cell(1, colIndex++).Value = "کد تفصیل 4";
                if (hasTafsil4) worksheet.Cell(1, colIndex++).Value = "تفصیل 4";

                if (hasTafsil5) worksheet.Cell(1, colIndex++).Value = "کد تفصیل 5";
                if (hasTafsil5) worksheet.Cell(1, colIndex++).Value = "تفصیل 5";
                if (hasTafsil6) worksheet.Cell(1, colIndex++).Value = "تفصیل 6";
                if (hasTafsil7) worksheet.Cell(1, colIndex++).Value = "تفصیل 7";
                if (hasTafsil8) worksheet.Cell(1, colIndex++).Value = "تفصیل 8";

                worksheet.Cell(1, colIndex++).Value = "بدهکار";
                worksheet.Cell(1, colIndex++).Value = "بستانکار";
                worksheet.Cell(1, colIndex).Value = "شرح";

                // تنظیم استایل هدر
                var headerRange = worksheet.Range(1, 1, 1, colIndex);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.DarkGray;
                headerRange.Style.Font.FontColor = XLColor.White;
                headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                headerRange.Style.Border.OutsideBorderColor = XLColor.Black;
                headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                headerRange.Style.Border.InsideBorderColor = XLColor.Black;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // پر کردن داده‌ها
                int row = 2; // از ردیف دوم شروع می‌شود چون ردیف اول برای عنوان ستون‌هاست
                int rowNumber = 1;

                foreach (var article in articles)
                {
                    colIndex = 1;
                    worksheet.Cell(row, colIndex++).Value = rowNumber;
                    worksheet.Cell(row, colIndex++).Value = article.DocNumber;
                    worksheet.Cell(row, colIndex++).Value = article.DocDate?.LatinToPersian();
                    worksheet.Cell(row, colIndex++).Value = article.KolCode;
                    worksheet.Cell(row, colIndex++).Value = article.KolName;
                    worksheet.Cell(row, colIndex++).Value = article.MoeinCode;
                    worksheet.Cell(row, colIndex++).Value = article.MoeinName;

                    if (hasTafsil4) worksheet.Cell(row, colIndex++).Value = article.Tafsil4Id;
                    if (hasTafsil4) worksheet.Cell(row, colIndex++).Value = article.Tafsil4Name;
                    if (hasTafsil5) worksheet.Cell(row, colIndex++).Value = article.Tafsil5Id;
                    if (hasTafsil5) worksheet.Cell(row, colIndex++).Value = article.Tafsil5Name;
                    if (hasTafsil6) worksheet.Cell(row, colIndex++).Value = article.Tafsil6Name;
                    if (hasTafsil7) worksheet.Cell(row, colIndex++).Value = article.Tafsil7Name;
                    if (hasTafsil8) worksheet.Cell(row, colIndex++).Value = article.Tafsil8Name;

                    worksheet.Cell(row, colIndex++).Value = article.Bed > 0 ? article.Bed.ToPrice() : "0";
                    worksheet.Cell(row, colIndex++).Value = article.Bes > 0 ? article.Bes.ToPrice() : "0";
                    worksheet.Cell(row, colIndex).Value = article.Comment;

                    // استایل دهی و خط کشی
                    var dataRange = worksheet.Range(row, 1, row, colIndex);
                    dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Border.OutsideBorderColor = XLColor.Gray;
                    dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Border.InsideBorderColor = XLColor.Gray;

                    row++;
                    rowNumber++;
                }

                // تنظیم خودکار عرض ستون‌ها
                worksheet.Columns().AdjustToContents();

                // راست‌چین کردن محتوای غیر عددی
                worksheet.Range(2, 1, row - 1, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                // تنظیم Layout برای چاپ در A4
                worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                worksheet.PageSetup.PaperSize = XLPaperSize.A4Paper;
                worksheet.PageSetup.CenterHorizontally = true;
                worksheet.PageSetup.Margins.Top = 0.5;
                worksheet.PageSetup.Margins.Bottom = 0.5;
                worksheet.PageSetup.Margins.Left = 0.5;
                worksheet.PageSetup.Margins.Right = 0.5;

                // ذخیره فایل Excel در بایت آرایه
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
