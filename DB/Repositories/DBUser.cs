using FamilyTree.Data;
using FamilyTree.Models.Common;
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


        public override DataTable GetAll(CommonSearchModel model)
        {
            string sql = @"
SELECT
    u.*,
    ur.UserRoleDescription AS UserRoleName
FROM User u
LEFT JOIN UserRole ur ON u.UserRoleId = ur.UserRoleId
WHERE 1=1";

            List<object> parameters = new List<object>();

            if (!string.IsNullOrEmpty(model.SearchValue))
            {
                sql += " AND (u.Username LIKE @p" + parameters.Count + " OR u.Email LIKE @p" + parameters.Count + ")";
                parameters.Add($"%{model.SearchValue}%");
            }

            if (model.FilterList != null && model.FilterList.Count > 0)
            {
                if (model.FilterList != null && model.FilterList.Count > 0)
                {
                    foreach (KeyValuePair<string, string> filter in model.FilterList)
                    {
                        if (!filter.Key.All(char.IsLetterOrDigit)) continue;

                        sql += $" AND u.{filter.Key} = @p{parameters.Count}";
                        parameters.Add(filter.Value);
                    }
                }

                return ExecuteSql(sql, parameters.ToArray());
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
