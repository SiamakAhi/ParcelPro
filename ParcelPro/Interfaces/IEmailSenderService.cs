namespace ParcelPro.Interfaces
{
    public interface IEmailSenderService
    {

        void Sender(string to, string subject, string body);
    }
}
