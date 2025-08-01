using Microsoft.AspNetCore.Identity;

namespace ParcelPro.Models.Identity
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual AppRole Role { get; set; }
        public virtual AppIdentityUser User { get; set; }
    }
}
