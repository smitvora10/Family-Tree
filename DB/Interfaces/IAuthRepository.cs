using FamilyTree.BL.Services;
using FamilyTree.Models.Common;
using FamilyTree.Models.Master;

namespace FamilyTree.DB.Interfaces
{
    public interface IAuthRepository : IBaseRepository<User>
    {
        public Response ValidateUser(LoginRequest request);
    }
}
