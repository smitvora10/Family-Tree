using FamilyTree.Models.Master;

namespace FamilyTree.BL.Services
{
    public interface IOtpService
    {
        string GenerateOtp(string email);
        OtpVerification? GetLatestOtp(string email);
        void MarkOtpAsUsed(OtpVerification otpVerification);
    }
}
