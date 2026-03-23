using HRM_v2.Data;
using HRM_v2.DTOs;
using HRM_v2.Models;
using HRM_v2.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
namespace HRM_v2.Services.Implementations
{
    public class EmailService : IEmailService
    {
        public void SendBirthdayEmail(string toEmail, string name)
        {
            var fromEmail = "nguyentrikhoi20112003@gmail.com";
            var password = "ybllcpeagdeljghm"; // 👈 nhớ thay

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(fromEmail, password);
                smtp.EnableSsl = true;

                var message = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = "Chúc mừng sinh nhật 🎉",
                    Body = $"Chúc mừng sinh nhật {name}! Chúc bạn một ngày tuyệt vời!",
                    IsBodyHtml = true
                };

                message.To.Add(toEmail);

                smtp.Send(message);
            }
        }
    }
}
