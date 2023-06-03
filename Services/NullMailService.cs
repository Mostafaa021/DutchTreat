namespace DutchTreat.Services
{
    public class NullMailService : IMailService
    {
        private readonly ILogger<NullMailService> _logger;

        public NullMailService(ILogger<NullMailService> logger)
        {
            _logger = logger;
        }

        public void SendMessage(string To, string subject, string body)
        {
            // Just Log the Message 
            _logger.LogInformation($"To :{To} , Subject : {subject} , Body : {body}");
        }
    }
}
