using System;
using System.Threading.Tasks;
using FamilyTree.Models.Common;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace FamilyTree.BL.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task SendOtpEmailAsync(string toEmail, string otpCode, int expiryMinutes)
        {
            Console.WriteLine($"[EmailService] Sending OTP email to {toEmail}");
            if (string.IsNullOrWhiteSpace(toEmail))
            {
                throw new ArgumentException("Recipient email is required.", nameof(toEmail));
            }

            if (string.IsNullOrWhiteSpace(_emailSettings.Host) || string.IsNullOrWhiteSpace(_emailSettings.FromEmail))
            {
                throw new InvalidOperationException("Email settings are not configured. Please check your appsettings.json.");
            }

            MimeMessage emailMessage = BuildMessage(toEmail, otpCode, expiryMinutes);

            using SmtpClient smtpClient = new();
            SecureSocketOptions socketOptions;
            if (_emailSettings.Port == 587)
            {
                socketOptions = SecureSocketOptions.StartTls;
            }
            else if (_emailSettings.UseSsl)
            {
                socketOptions = SecureSocketOptions.SslOnConnect;
            }
            else
            {
                socketOptions = SecureSocketOptions.StartTlsWhenAvailable;
            }

            try
            {
                Console.WriteLine($"[EmailService] Connecting to SMTP host: {_emailSettings.Host}:{_emailSettings.Port}");
                await smtpClient.ConnectAsync(_emailSettings.Host, _emailSettings.Port, socketOptions).ConfigureAwait(false);

                if (!string.IsNullOrEmpty(_emailSettings.Username))
                {
                    Console.WriteLine($"[EmailService] Authenticating as {_emailSettings.Username}");
                    await smtpClient.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password).ConfigureAwait(false);
                }

                Console.WriteLine("[EmailService] Sending message...");
                await smtpClient.SendAsync(emailMessage).ConfigureAwait(false);
                Console.WriteLine("[EmailService] Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EmailService] ERROR: {ex.Message}");
                _logger.LogError(ex, "Failed to send OTP email to {Email}", toEmail);
                throw;
            }
            finally
            {
                await smtpClient.DisconnectAsync(true).ConfigureAwait(false);
            }
        }

        private MimeMessage BuildMessage(string toEmail, string otpCode, int expiryMinutes)
        {
            MimeMessage message = new();
            message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = "OTP Code for Vora Family Tree Signup";

            string body = $"" +
                $"Your OTP code is: {otpCode}\n" +
                $"This code will expire in {expiryMinutes} minutes.\n\n" +
                "If you did not request this code, please ignore this email and do not share the code with anyone.";

            message.Body = new TextPart("plain") { Text = body };
            return message;
        }
    }
}
