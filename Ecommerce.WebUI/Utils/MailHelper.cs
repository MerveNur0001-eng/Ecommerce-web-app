using System.Net;
using System.Net.Mail;
using Ecommerce.Core.Entities;

namespace Ecommerce.WebUI.Utils
{
    public class MailHelper
    {
        private readonly IConfiguration _config;

        public MailHelper(IConfiguration config)
        {
            _config = config;
        }

        public async Task<bool> SendMailAsync(Contact contact)
        {
            string body =
                $"Name: {contact.Name} - Surname: {contact.Surname} - Email: {contact.Email} - Phone: {contact.Phone} - Message: {contact.Message}";

            var fromEmail = _config["EmailSettings:FromEmail"];

            return await SendMailAsync(fromEmail, "New message received from site", body);
        }

        public async Task<bool> SendMailAsync(string email, string subject, string mailBody)
        {
            var fromEmail = _config["EmailSettings:FromEmail"];
            var password = _config["EmailSettings:AppPassword"];

            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.Credentials = new NetworkCredential(fromEmail, password);
                smtpClient.EnableSsl = true;

                MailMessage message = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = subject,
                    Body = mailBody,
                    IsBodyHtml = true
                };

                message.To.Add(email);

                try
                {
                    await smtpClient.SendMailAsync(message);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
        }
        public string GetBaseUrl()
        {
            return _config["AppSettings:BaseUrl"];
        }
    }
}