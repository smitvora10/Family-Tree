using FamilyTree.Data;
using FamilyTree.Data.Common;
using FamilyTree.DB.Interfaces;
using FamilyTree.Models.Master;
using Microsoft.EntityFrameworkCore;

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
    }



}
