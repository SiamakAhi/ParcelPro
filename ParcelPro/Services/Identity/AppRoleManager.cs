using ParcelPro.Interfaces.Identity;
using ParcelPro.Models.Identity;
using ParcelPro.ViewModels.IdentityViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ParcelPro.Services.Identity
{
    public class AppRoleManager : RoleManager<AppRole>, IAppRoleManager
    {
        private readonly IdentityErrorDescriber _error;
        private readonly ILookupNormalizer _Normalizer;
        private readonly ILogger<AppRoleManager> _Logger;
        private readonly IEnumerable<IRoleValidator<AppRole>> _validators;
        private readonly IRoleStore<AppRole> _store;

        public AppRoleManager(
              IdentityErrorDescriber error,
              ILookupNormalizer Normalizer,
              ILogger<AppRoleManager> Logger,
              IEnumerable<IRoleValidator<AppRole>> validators,
              IRoleStore<AppRole> store)
          : base(store, validators, Normalizer, error, Logger)
        {
            _error = error;
            _Normalizer = Normalizer;
            _Logger = Logger;
            _validators = validators;
            _store = store;

        }

        public List<AppRole> GetAllRole()
        {
            return Roles.ToList();
        }

        public SelectList SelectList_Roles()
        {
            var roles = Roles.Select(x => new
            {
                id = x.Name,
                name = x.Description
            }).ToList();

            return new SelectList(roles, "id", "name");

        }

        public IQueryable<AppRolViewModel> RolesViewModelList()
        {
            return Roles.Select(n => new AppRolViewModel
            {
                Description = n.Description,
                Id = n.Id,
                Name = n.Name,
                UsersCount = n.Users.Count
            }).AsQueryable();
        }



    }
}
