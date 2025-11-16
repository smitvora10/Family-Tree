using FamilyTree.Models.Common;

namespace FamilyTree.BL.Services
{
    public interface IAuthService
    {
        public Response ValidateUser(LoginRequest request);
        public Response RegisterUser(RegisterRequest request);
        public Response VerifyOtp(VerifyOtpRequest request);
    }
}
