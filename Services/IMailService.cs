namespace DutchTreat.Services
{
    public interface IMailService
    {
        void SendMessage(string To, string subject, string body);
    }
}