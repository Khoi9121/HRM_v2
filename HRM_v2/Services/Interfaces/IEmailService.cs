namespace HRM_v2.Services.Interfaces
{
    public interface IEmailService
    {
        void SendBirthdayEmail(string toEmail, string name);
    }
}
