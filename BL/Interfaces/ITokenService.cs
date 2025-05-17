using FamilyTree.Models.Common;
using FamilyTree.Models.Master;

namespace FamilyTree.BL.Services
{
    public interface ITokenService
    {
        (int userId, int roleId) ValidateToken(string token);

        string GenerateToken(int userId, int roleId);
    }
}
