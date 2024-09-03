using FamilyTree.Data;
using FamilyTree.Data.Common;
using FamilyTree.DB.Interfaces;
using FamilyTree.Models.Master;
using Microsoft.EntityFrameworkCore;

namespace FamilyTree.BL.Services
{
    public class DBOccupation : DBCommon<Occupation>, IOccupationRepository
    {
        private readonly DbSet<Occupation> _dbSet;
        private readonly DataContext _context;
        public DBOccupation(DataContext context) : base(context)
        {
            _dbSet = context.Set<Occupation>();
        }
    }



}
