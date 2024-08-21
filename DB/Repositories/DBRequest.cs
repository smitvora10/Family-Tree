using FamilyTree.Data;
using FamilyTree.Data.Common;
using FamilyTree.DB.Interfaces;
using FamilyTree.Models.Master;
using Microsoft.EntityFrameworkCore;

namespace FamilyTree.BL.Services
{
    public class DBRequest : DBCommon<Request>, IRequestRepository
    {
        private readonly DbSet<Request> _dbSet;
        private readonly DataContext _context;
        public DBRequest(DataContext context) : base(context)
        {
            _dbSet = context.Set<Request>();
        }
    }



}
