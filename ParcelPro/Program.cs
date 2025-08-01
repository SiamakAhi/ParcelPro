﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ParcelPro.Areas.Accounting.AccountingInterfaces;
using ParcelPro.Areas.Accounting.AccountingServices;
using ParcelPro.Areas.AvaRasta.Interfaces;
using ParcelPro.Areas.AvaRasta.Services;
using ParcelPro.Areas.Client.ClientInterfacses;
using ParcelPro.Areas.Client.ClientServices;
using ParcelPro.Areas.Commercial.ComercialInterfaces;
using ParcelPro.Areas.Commercial.ComercialServices;
using ParcelPro.Areas.Courier.CuurierInterfaces;
using ParcelPro.Areas.Courier.CuurierServices;
using ParcelPro.Areas.CustomerArea.CustomerInterfases;
using ParcelPro.Areas.CustomerArea.CustomerServices;
using ParcelPro.Areas.DataTransfer.DataTransferInterfaces;
using ParcelPro.Areas.DataTransfer.DataTransferServices;
using ParcelPro.Areas.Geolocation.GeolocationInterfaces;
using ParcelPro.Areas.Geolocation.GeolocationServices;
using ParcelPro.Areas.Projects.ProjectInterfaces;
using ParcelPro.Areas.Projects.ProjectServices;
using ParcelPro.Areas.Support.SuportInterfaces;
using ParcelPro.Areas.Support.SupportServices;
using ParcelPro.Areas.Treasury.TreasuryInterfaces;
using ParcelPro.Areas.Treasury.TreasuryServices;
using ParcelPro.Areas.Warehouse.WarehouseInterfaces;
using ParcelPro.Areas.Warehouse.WarehouseServices;
using ParcelPro.Interfaces;
using ParcelPro.Interfaces.CommercialInterfaces;
using ParcelPro.Interfaces.Identity;
using ParcelPro.Middlewares;
using ParcelPro.Models;
using ParcelPro.Models.Identity;
using ParcelPro.Services;
using ParcelPro.Services.CommercialService;
using ParcelPro.Services.Identity;
using Stimulsoft.Base;

var builder = WebApplication.CreateBuilder(args);
//IWebHostEnvironment env = builder.Environment;
//var contentRoot = env.WebRootPath;

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddMvc();

//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromHours(4);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//Identity Services
builder.Services.AddIdentity<AppIdentityUser, AppRole>(option =>
{
    option.Password.RequireDigit = false;
    option.Password.RequiredUniqueChars = 0;
    option.Password.RequireLowercase = false;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireUppercase = false;
    option.User.RequireUniqueEmail = false;
    option.Password.RequiredLength = 0;
    option.Password.RequireLowercase = false;

})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IAppIdentityUserManager, AppIdentityUserManager>();
builder.Services.AddScoped<IAppRoleManager, AppRoleManager>();
builder.Services.AddScoped<SignInManager<AppIdentityUser>>();
builder.Services.AddScoped<PersianIdentityError>();

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

//....................................................................................

//... App Services
builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();
builder.Services.AddScoped<ICsrService, CsrService>();
builder.Services.AddScoped<ILicenseService, LicenseService>();
builder.Services.AddScoped<IUISettingsService, UISettingsService>();
builder.Services.AddScoped<UserContextService>();
builder.Services.AddScoped<ISMSService, SMSService>();
builder.Services.AddScoped<SmsSenderPersiaFava>();

//Ava Rasta Services
builder.Services.AddScoped<ICostomerService, CostomerService>();
builder.Services.AddScoped<ISellerService, SellerService>();
builder.Services.AddScoped<IBuyerService, BuyerService>();
builder.Services.AddScoped<IGeneralService, GeneralService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ISupportService, SupportService>();
builder.Services.AddScoped<ISmsSenderService, SmsSenderService>();
builder.Services.AddScoped<IPersonService, PersonService>();
//..
builder.Services.AddSingleton<IConfigurationLoader, ConfigurationLoader>();

//Accounting Services
builder.Services.AddScoped<IAccBaseInfoService, AccBaseInfoService>();
builder.Services.AddScoped<IAccCodingService, AccCodingService>();
builder.Services.AddScoped<IAccOperationService, AccOperationService>();
builder.Services.AddScoped<IAccountingReportService, AccountingReportService>();
builder.Services.AddScoped<IAccImportService, AccImportService>();
builder.Services.AddScoped<IAccExportService, AccExportService>();
builder.Services.AddScoped<IAccEndOfPeriodService, AccEndOfPeriodService>();
builder.Services.AddScoped<IAccProfitMasterService, AccProfitMasterService>();
builder.Services.AddScoped<IAccGetBaseDataService, AccGetBaseDataService>();
builder.Services.AddScoped<IAccSettingService, AccSettingService>();
builder.Services.AddScoped<IAccAsistantsService, AccAsistantsService>();
builder.Services.AddScoped<IConProjectService, ConProjectService>();
builder.Services.AddScoped<IAccRepresentativeService, AccRepresentativeService>();

// Warehouse
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<IWhProductService, WhProductService>();

//Commercial
builder.Services.AddScoped<IInvoiceService, InvoiceService>();

// Geolocation
builder.Services.AddScoped<IGeoGeneralService, GeoGeneralService>();

//Courier
builder.Services.AddScoped<IOverallService, OverallService>();
builder.Services.AddScoped<ICourierServiceService, CourierServiceService>();
builder.Services.AddScoped<ICuPricingService, CuPricingService>();
builder.Services.AddScoped<IBranchUserService, BranchUserService>();
builder.Services.AddScoped<ICuBranchService, CuBranchService>();
builder.Services.AddScoped<ICuHubService, CuHubService>();
builder.Services.AddScoped<IKPDataTransferService, KPDataTransferService>();
builder.Services.AddScoped<ICuRepresentativeService, CuRepresentativeService>();
builder.Services.AddScoped<ICuGaeOldSys_SaleDataService, CuGaeOldSys_SaleDataService>();
builder.Services.AddScoped<IBillofladingService, BillofladingService>();
builder.Services.AddScoped<ITrachkingService, TrachkingService>();
builder.Services.AddScoped<ICourierFinancialService, CourierFinancialService>();
builder.Services.AddScoped<ICargoManifestService, CargoManifestService>();
builder.Services.AddScoped<ICu_SaleContractService, Cu_SaleContractService>();
builder.Services.AddScoped<IBusinessPartnerService, BusinessPartnerService>();


// client
builder.Services.AddScoped<IClientPanelService, ClientPanelService>();
//Treasury
builder.Services.AddScoped<BitPayService>();
builder.Services.AddScoped<IMoneyTransactionService, MoneyTransactionService>();
builder.Services.AddScoped<ITreCashBoxService, TreCashBoxService>();
builder.Services.AddScoped<ITreasuryGeneralData, TreasuryGeneralData>();
builder.Services.AddScoped<ITreBankImporterService, TreBankImporterService>();
builder.Services.AddScoped<ITreBaseService, TreBaseService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/SignOut";
        options.AccessDeniedPath = "/Home/Index";
        options.ExpireTimeSpan = TimeSpan.FromHours(4);
        options.SlidingExpiration = true;
    });

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.Lax;

});

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromMinutes(240);
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(4);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await AppDbContext.SeedData(services); // فراخوانی متد SeedData
    }
    catch (Exception ex)
    {
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Index");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCookiePolicy();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<UserContextMiddleware>();

app.MapControllerRoute(
     name: "areas",
     pattern: "{area:exists}/{controller=Account}/{action=login}/{id?}");

app.MapControllerRoute(
name: "default",
pattern: "{controller=Account}/{action=login}/{id?}");

//Add Fonts
//fonts
string fontsPath = Path.Combine(app.Environment.WebRootPath, "fonts");
foreach (var fontFile in Directory.GetFiles(fontsPath, "*.ttf"))
{
    StiFontCollection.AddFontFile(fontFile);
}

app.Run();
