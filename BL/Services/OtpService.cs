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

        public string GenerateOtp(string mobileNumber)
        {
            if (string.IsNullOrWhiteSpace(mobileNumber))
            {
                throw new ArgumentException("Mobile number is required.", nameof(mobileNumber));
            }

            string normalizedMobile = NormalizeMobile(mobileNumber);
            string otpCode = RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");
            DateTime utcNow = DateTime.UtcNow;

            OtpVerification otpEntity = new OtpVerification
            {
                MobileNumber = normalizedMobile,
                OtpCode = otpCode,
                CreatedAt = utcNow,
                ExpirationTime = utcNow.AddMinutes(5),
                IsUsed = false
            };

            _otpSet.Add(otpEntity);
            _context.SaveChanges();

            Console.WriteLine($"OTP for {normalizedMobile}: {otpCode}");

            return otpCode;
        }

        public OtpVerification? GetLatestOtp(string mobileNumber)
        {
            if (string.IsNullOrWhiteSpace(mobileNumber))
            {
                return null;
            }

            string normalizedMobile = NormalizeMobile(mobileNumber);

            return _otpSet
                .Where(o => o.MobileNumber == normalizedMobile)
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

        private static string NormalizeMobile(string mobileNumber)
        {
            return mobileNumber.Trim();
        }
    }
}
