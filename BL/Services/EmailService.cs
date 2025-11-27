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
            if (string.IsNullOrWhiteSpace(toEmail))
            {
                throw new ArgumentException("Recipient email is required.", nameof(toEmail));
            }

            MimeMessage emailMessage = BuildMessage(toEmail, otpCode, expiryMinutes);

            using SmtpClient smtpClient = new();
            SecureSocketOptions socketOptions = _emailSettings.UseSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTlsWhenAvailable;

            try
            {
                await smtpClient.ConnectAsync(_emailSettings.Host, _emailSettings.Port, socketOptions).ConfigureAwait(false);

                if (!string.IsNullOrEmpty(_emailSettings.Username))
                {
                    await smtpClient.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password).ConfigureAwait(false);
                }

                await smtpClient.SendAsync(emailMessage).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
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
