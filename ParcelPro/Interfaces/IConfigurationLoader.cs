using ParcelPro.Models;

namespace ParcelPro.Interfaces
{
    public interface IConfigurationLoader
    {
        AppSettings GetConfigurations();
    }
}
