using FamilyTree.Models.Common;
using System.Threading.Tasks;

namespace FamilyTree.BL.Services
{
    public interface IAuthService
    {
        public Response ValidateUser(LoginRequest request);
        public Task<Response> RegisterUser(RegisterRequest request);
        public Response VerifyOtp(VerifyOtpRequest request);
        public Response UpdateProfile(UpdateProfileRequest request);
        public Task<Response> ChangePassword(ChangePasswordRequest request);
        public Task<Response> SendOtp(SendOtpRequest request);
    }
}
