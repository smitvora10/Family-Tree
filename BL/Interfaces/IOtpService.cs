using FamilyTree.Models.Master;

namespace FamilyTree.BL.Services
{
    public interface IOtpService
    {
        string GenerateOtp(string mobileNumber);
        OtpVerification? GetLatestOtp(string mobileNumber);
        void MarkOtpAsUsed(OtpVerification otpVerification);
    }
}
