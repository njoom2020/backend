using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace ISPN.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<(bool Success, string ErrorMessage)> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                // قراءة البيانات ديناميكياً من appsettings.json
                var smtpServer = _config["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";
                var port = int.Parse(_config["EmailSettings:Port"] ?? "587");
                var senderName = _config["EmailSettings:SenderName"] ?? "EcommerceStore";
                var senderEmail = _config["EmailSettings:SenderEmail"] ?? "appmobile202cc@gmail.com";
                
                // تنظيف كلمة المرور من المسافات تلقائياً (pfetoxpcaounrqhe)
                var password = (_config["EmailSettings:Password"] ?? "").Replace(" ", "");

                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(senderName, senderEmail));
                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = subject;

                var builder = new BodyBuilder { HtmlBody = body };
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();

                // إعدادات تجاوز شهادات بيئة التطوير والـ Revocation على macOS
                smtp.CheckCertificateRevocation = false;
                smtp.ServerCertificateValidationCallback = (s, c, ch, e) => true;

                await smtp.ConnectAsync(smtpServer, port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(senderEmail, password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MailKit Error]: {ex.Message}");
                return (false, ex.Message);
            }
        }
    }
}