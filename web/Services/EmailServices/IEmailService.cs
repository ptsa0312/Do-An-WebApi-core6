using web.Models.DTO;

namespace web.Services.EmailServices
{
    public interface IEmailService
    {
        void SendEmail(Email request);
    }
}
