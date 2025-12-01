using FamilyTree.Data;
using FamilyTree.Data.Common;
using FamilyTree.DB.Interfaces;
using FamilyTree.Models.Master;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FamilyTree.BL.Services
{
    public class DBUser : DBCommon<User>, IUserRepository
    {
        private readonly DbSet<User> _dbSet;
        private readonly DataContext _context;
        public DBUser(DataContext context) : base(context)
        {
            _dbSet = context.Set<User>();
        }
        public override object GetAll(string[]? includeFields = null, string[]? excludeFields = null)
        {
            string sql = @"
SELECT
    u.*,
    ur.UserRoleDescription AS UserRoleName
FROM User u
LEFT JOIN UserRole ur ON u.UserRoleId = ur.UserRoleId";

            return ExecuteSql(sql);
        }

        public override object GetById(int id)
        {
            string sql = @"
SELECT
    u.*,
    ur.UserRoleDescription AS UserRoleName
FROM User u
LEFT JOIN UserRole ur ON u.UserRoleId = ur.UserRoleId
WHERE u.UserId = @p0";

            return ExecuteSql(sql, id);
        }
    }



}
