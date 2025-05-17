using FamilyTree.Data;
using FamilyTree.DB.Interfaces;
using FamilyTree.Models.Common;
using FamilyTree.Models.Master;
using Microsoft.EntityFrameworkCore;

namespace FamilyTree.BL.Services
{
    public class BLAuth : BLCommon<User>, IAuthService
    {
        private readonly IAuthRepository _dbContext;
        private readonly DbSet<User> _dbSet;
        private readonly DataContext _context;
        public BLAuth(IAuthRepository dbContext, DataContext context) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbSet = context.Set<User>();
        }

        public Response ValidateUser(LoginRequest request)
        {
           return _dbContext.ValidateUser(request);
        }
    }
}
