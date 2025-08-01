using ParcelPro.ViewModels.Tax;

namespace ParcelPro.Interfaces
{
    public interface ICsrService
    {
        CsrResult GenerateCsrForHoghooghi(CsrInfoHoghooghi csrInfo);
        CsrResult GenerateCsrForHaghighi(CsrInfoHaghighi csrInfo);
        string LoadPrivateKey(string privateKeyPEM);
    }
}
