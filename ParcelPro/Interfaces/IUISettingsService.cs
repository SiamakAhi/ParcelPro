using ParcelPro.Models;

namespace ParcelPro.Interfaces
{
    public interface IUISettingsService
    {
        Task<AppTheme> ThemeTogglerAsync(int id);
    }
}
