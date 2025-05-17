using FamilyTree.Models.Common;
using FamilyTree.Models.Master;

namespace FamilyTree.BL.Services
{
    public interface IAuthService : IBaseService<User>
    {
        public Response ValidateUser(LoginRequest request);
    }
}
