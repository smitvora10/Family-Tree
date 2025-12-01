using System;
using System.Linq;
using System.Security.Cryptography;
using FamilyTree.Data;
using FamilyTree.Models.Master;
using Microsoft.EntityFrameworkCore;

namespace FamilyTree.BL.Services
{
    public class OtpService : IOtpService
    {
        private readonly DataContext _context;
        private readonly DbSet<OtpVerification> _otpSet;

        public OtpService(DataContext context)
        {
            _context = context;
            _otpSet = context.Set<OtpVerification>();
        }

        public string GenerateOtp(string email)
        {
            Console.WriteLine($"[OtpService] Generating OTP for {email}");
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email is required.", nameof(email));
            }

            string normalizedEmail = NormalizeEmail(email);
            string otpCode = RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");
            DateTime utcNow = DateTime.UtcNow;

            IQueryable<OtpVerification> existingOtps = _otpSet
                .Where(o => o.Email == normalizedEmail && !o.IsUsed);

            foreach (OtpVerification staleOtp in existingOtps)
            {
                staleOtp.IsUsed = true;
                staleOtp.UsedAt = utcNow;
            }

            OtpVerification otpEntity = new OtpVerification
            {
                Email = normalizedEmail,
                OtpCode = otpCode,
                CreatedAt = utcNow,
                ExpirationTime = utcNow.AddMinutes(5),
                IsUsed = false
            };

            _otpSet.Add(otpEntity);
            int changes = _context.SaveChanges();
            Console.WriteLine($"[OtpService] OTP generated and saved. Changes: {changes}. Code: {otpCode}");

            return otpCode;
        }

        public OtpVerification? GetLatestOtp(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            string normalizedEmail = NormalizeEmail(email);

            return _otpSet
                .Where(o => o.Email == normalizedEmail)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefault();
        }

        public void MarkOtpAsUsed(OtpVerification otpVerification)
        {
            if (otpVerification == null)
            {
                return;
            }

            otpVerification.IsUsed = true;
            otpVerification.UsedAt = DateTime.UtcNow;
            _context.SaveChanges();
        }

        private static string NormalizeEmail(string email)
        {
            return email.Trim().ToLowerInvariant();
        }
    }
}
