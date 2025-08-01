using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ParcelPro.Areas.Courier.CuurierInterfaces;
using ParcelPro.Areas.Courier.Dto.ContractDto;
using ParcelPro.Areas.Courier.Models.Entities;
using ParcelPro.Interfaces.Identity;
using ParcelPro.Models;
using ParcelPro.Models.Identity;

namespace ParcelPro.Areas.Courier.CuurierServices
{
    public class Cu_SaleContractService : ICu_SaleContractService
    {
        private readonly AppDbContext _db;
        private readonly IAppIdentityUserManager _userManager;
        private readonly IAppRoleManager _roleManager;
        public Cu_SaleContractService(AppDbContext appDbContext, IAppIdentityUserManager appIdentityUserManager, IAppRoleManager roleManager)
        {
            _db = appDbContext;
            _userManager = appIdentityUserManager;
            _roleManager = roleManager;
        }

        public async Task<SelectList> SelectList_ContractsAsync(long SellerId)
        {
            var list = await _db.Cu_SaleContracts.AsNoTracking().Include(n => n.ContractParty)
                .Where(n => n.SellerId == SellerId)
                .Select(n => new { id = n.Id, name = n.ContractParty.Name + "(" + n.Title + ")" }).OrderBy(n => n.name).ToListAsync();

            return new SelectList(list, "id", "name");
        }

        public async Task<clsResult> AddContract(SaleContractDto dto)
        {
            clsResult result = new clsResult();

            Cu_SaleContract contract = new Cu_SaleContract();
            contract.Title = dto.Title;
            contract.SellerId = dto.SellerId;
            contract.ApiKey = dto.ApiKey;
            contract.PartyId = dto.PartyId;
            contract.AccounIstActive = dto.AccounIstActive;
            contract.CreateAt = DateTime.Now;
            contract.StartDate = DateTime.Now;
            contract.IsActive = dto.IsActive;

            _db.Cu_SaleContracts.Add(contract);
            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "اطلاعات قرارداد با موقفیت ثبت شد";
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "مشکلی در ثبت اطلاعات قرارداد پیش آمده است";
            }

            return result;
        }

        public async Task<clsResult> UpdateContract(SaleContractDto dto)
        {
            clsResult result = new clsResult();

            var contract = await _db.Cu_SaleContracts.FindAsync(dto.Id);
            if (contract == null)
            {
                result.Success = false;
                result.Message = "قرارداد مورد نظر یافت نشد";
                return result;
            }

            contract.Title = dto.Title;
            contract.SellerId = dto.SellerId;
            contract.ApiKey = dto.ApiKey;
            contract.PartyId = dto.PartyId;
            contract.AccounIstActive = dto.AccounIstActive;
            contract.IsActive = dto.IsActive;
            contract.StartDate = dto.StartDate;

            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "اطلاعات قرارداد با موفقیت به‌روزرسانی شد";
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "خطا در به‌روزرسانی اطلاعات قرارداد";
            }

            return result;
        }

        public async Task<List<SaleContractDto>> GetContracts(long sellerId)
        {
            return await _db.Cu_SaleContracts
                .Where(x => x.SellerId == sellerId)
                .Select(x => new SaleContractDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    SellerId = x.SellerId,
                    PartyId = x.PartyId,
                    PartyName = x.ContractParty.Name,
                    StartDate = x.StartDate,
                    IsActive = x.IsActive,
                    AccounIstActive = x.AccounIstActive,
                    ApiKey = x.ApiKey
                }).ToListAsync();
        }

        public async Task<SaleContractDto?> GetContractById(int id)
        {
            var contract = await _db.Cu_SaleContracts
                .Include(x => x.ContractParty)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (contract == null)
                return null;

            return new SaleContractDto
            {
                Id = contract.Id,
                Title = contract.Title,
                SellerId = contract.SellerId,
                PartyId = contract.PartyId,
                PartyName = contract.ContractParty?.Name,
                StartDate = contract.StartDate,
                IsActive = contract.IsActive,
                AccounIstActive = contract.AccounIstActive,
                ApiKey = contract.ApiKey
            };
        }

        public async Task<clsResult> DeleteContract(int id)
        {
            clsResult result = new clsResult();

            var contract = await _db.Cu_SaleContracts.FindAsync(id);
            if (contract == null)
            {
                result.Success = false;
                result.Message = "قرارداد مورد نظر یافت نشد";
                return result;
            }

            _db.Cu_SaleContracts.Remove(contract);

            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "قرارداد با موفقیت حذف شد";
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "خطا در حذف قرارداد";
            }

            return result;
        }


        //-----------------------------------------------------------------------------------------------------
        //---------- Client User -------------------------------------------------------- Client User ---------
        //-----------------------------------------------------------------------------------------------------

        public async Task<List<SaleContractUserDto>> ClientUsersAsync(long SellerId)
        {
            var users = await _db.Cu_SaleContractUsers.AsNoTracking()
                .Include(n => n.Contract).ThenInclude(n => n.ContractParty)
                .Include(n => n.UserData)
                .Where(n => n.SellerId == SellerId).Select(n => new SaleContractUserDto
                {
                    Id = n.Id,
                    SellerId = n.SellerId,
                    Name = n.Name,
                    Email = n.UserData.Email,
                    Mobile = n.UserData.Mobile,
                    userName = n.UserData.UserName,
                    userId = n.userId,
                    PartyName = n.Contract.ContractParty.Name,
                    ContractTitle = n.Contract.Title,
                    CreateAt = n.CreateAt,
                    ContractId = n.ContractId,
                    Role = n.Role,

                }).ToListAsync();
            return users;
        }
        public async Task<clsResult> AddClidentUserAsync(AddClientUserDto dto)
        {

            clsResult result = new clsResult();

            AppIdentityUser user = new AppIdentityUser();
            user.UserName = dto.UserName;
            user.Email = dto.Email;
            user.PhoneNumber = dto.Mobile;
            user.Mobile = dto.Mobile;
            user.FName = dto.Name;
            user.Family = dto.Family;
            user.Gender = dto.Gender;
            user.Birthday = DateTime.Now.AddYears(-25);
            user.IsActive = true;
            user.DepartmentCode = dto.DepartmentCode;
            user.CustomerId = dto.CustomerId;

            var identityResult = await _userManager.CreateAsync(user, dto.Password);
            if (!identityResult.Succeeded)
            {
                result.Message = "در فرآیند ایجاد حساب کاربری خطایی رخ داده است";
                return result;
            }

            if (dto.IdentityRols?.Length > 0)
            {
                foreach (var userrole in dto.IdentityRols)
                {
                    if (!await _roleManager.RoleExistsAsync(userrole))
                    {
                        var role = new AppRole
                        {
                            Name = userrole,
                            Description = "",
                        };
                        await _roleManager.CreateAsync(role);
                    }
                    await _userManager.AddToRoleAsync(user, userrole);
                }
            }

            Cu_SaleContractUser clientUser = new Cu_SaleContractUser();
            clientUser.userId = user.Id;
            clientUser.Name = dto.Name + " " + dto.Family;
            clientUser.ContractId = dto.ContractId;
            clientUser.SellerId = dto.SellerId;
            clientUser.Role = dto.Role;
            clientUser.CreateAt = DateTime.Now;

            _db.Cu_SaleContractUsers.Add(clientUser);

            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "حساب کاربری با موفقیت ایجاد شد";
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "خطا ایجاد حساب کاربری";
            }

            return result;
        }


    }
}
