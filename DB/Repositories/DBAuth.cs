using FamilyTree.Core;
using FamilyTree.Data;
using FamilyTree.Data.Common;
using FamilyTree.DB.Interfaces;
using FamilyTree.Models.Common;
using FamilyTree.Models.Master;
using Microsoft.EntityFrameworkCore;

namespace FamilyTree.BL.Services
{
    public class DBAuth : DBCommon<User>, IAuthRepository
    {
        private readonly DbSet<User> _dbSet;
        private readonly DataContext _context;
        Response response = new();
        public DBAuth(DataContext context) : base(context)
        {
            _dbSet = context.Set<User>();
        }

        public Response ValidateUser(LoginRequest request)
        {
            User objUser = _dbSet
             .Single(x => x.Username == request.Username);

            if (objUser != null && PasswordEncryptionDecryption.VerifyPassword(request.Password, objUser.PasswordHash))
            {
                response.Id = objUser.UserId;
            }
            else
            {
                response.IsError = true;
                response.MessageCode = MessageCode.E006.ToString();
            }
            return response;
        }

    }
}
