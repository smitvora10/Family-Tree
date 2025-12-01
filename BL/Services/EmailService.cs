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
            _logger.LogInformation("[EmailService] Sending OTP email to {ToEmail}", toEmail);
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
                _logger.LogInformation("[EmailService] Connecting to SMTP host: {Host}:{Port}", _emailSettings.Host, _emailSettings.Port);
                await smtpClient.ConnectAsync(_emailSettings.Host, _emailSettings.Port, socketOptions).ConfigureAwait(false);

                if (!string.IsNullOrEmpty(_emailSettings.Username))
                {
                    _logger.LogInformation("[EmailService] Authenticating as {Username}", _emailSettings.Username);
                    await smtpClient.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password).ConfigureAwait(false);
                }

                _logger.LogInformation("[EmailService] Sending message...");
                await smtpClient.SendAsync(emailMessage).ConfigureAwait(false);
                _logger.LogInformation("[EmailService] Email sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EmailService] ERROR: {Message}", ex.Message);
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
            message.Subject = "Welcome to Family Tree - Verify Your Email";

            var builder = new BodyBuilder();
            builder.HtmlBody = GetHtmlBody(otpCode, expiryMinutes);
            builder.TextBody = $"Your OTP code is: {otpCode}. It expires in {expiryMinutes} minutes.";

            message.Body = builder.ToMessageBody();
            return message;
        }

        private string GetHtmlBody(string otpCode, int expiryMinutes)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <style>
        body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; }}
        .container {{ max-width: 600px; margin: 40px auto; background-color: #ffffff; border-radius: 8px; box-shadow: 0 4px 8px rgba(0,0,0,0.05); overflow: hidden; }}
        .header {{ background-color: #2c3e50; padding: 30px; text-align: center; }}
        .header h1 {{ color: #ffffff; margin: 0; font-size: 24px; letter-spacing: 1px; }}
        .content {{ padding: 40px 30px; text-align: center; color: #333333; }}
        .welcome-text {{ font-size: 20px; margin-bottom: 20px; font-weight: 600; }}
        .otp-box {{ background-color: #f8f9fa; border: 2px dashed #dee2e6; border-radius: 8px; padding: 20px; margin: 30px 0; display: inline-block; }}
        .otp-code {{ font-size: 36px; font-weight: bold; color: #2c3e50; letter-spacing: 8px; margin: 0; font-family: monospace; }}
        .expiry-text {{ color: #6c757d; font-size: 14px; margin-top: 10px; }}
        .footer {{ background-color: #f8f9fa; padding: 20px; text-align: center; font-size: 12px; color: #999999; border-top: 1px solid #eeeeee; }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <!-- You can replace this text with an <img> tag pointing to a hosted logo URL -->
            <h1>FAMILY TREE</h1>
        </div>
        <div class=""content"">
            <div class=""welcome-text"">Welcome to the Family!</div>
            <p>Thank you for joining us. To complete your registration and verify your account, please use the One-Time Password (OTP) below.</p>
            
            <div class=""otp-box"">
                <div class=""otp-code"">{otpCode}</div>
            </div>

            <p>This code is valid for <strong>{expiryMinutes} minutes</strong>.</p>
            <p style=""margin-top: 30px; font-size: 13px; color: #666;"">If you did not create an account with Family Tree, you can safely ignore this email.</p>
        </div>
        <div class=""footer"">
            &copy; {DateTime.Now.Year} Family Tree Project. All rights reserved.<br>
            This is an automated message, please do not reply.
        </div>
    </div>
</body>
</html>";
        }
    }
}
