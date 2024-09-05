using FamilyTree.Data;
using FamilyTree.Data.Common;
using FamilyTree.DB.Interfaces;
using FamilyTree.Models.Master;
using Microsoft.EntityFrameworkCore;

namespace FamilyTree.BL.Services
{
    public class DBQualification : DBCommon<Qualification>, IQualificationRepository
    {
        private readonly DbSet<Qualification> _dbSet;
        private readonly DataContext _context;
        public DBQualification(DataContext context) : base(context)
        {
            _dbSet = context.Set<Qualification>();
        }
    }



}
