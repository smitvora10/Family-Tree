using FamilyTree.Models.Common;
using System.Threading.Tasks;

namespace FamilyTree.BL.Services
{
    public interface IAuthService
    {
        public Response ValidateUser(LoginRequest request);
        public Task<Response> RegisterUser(RegisterRequest request);
        public Response VerifyOtp(VerifyOtpRequest request);
    }
}
