using FamilyTree.Core;
using FamilyTree.Data;
using FamilyTree.Data.Common;
using FamilyTree.DB.Interfaces;
using FamilyTree.Models.Common;
using FamilyTree.Models.Master;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;
using System.Data;

namespace FamilyTree.BL.Services
{
    public class DBRelationType : DBCommon<RelationType>, IRelationTypeRepository
    {
        private readonly DbSet<RelationType> _dbSet;
        private readonly DataContext _context;
        public DBRelationType(DataContext context) : base(context)
        {
            _dbSet = context.Set<RelationType>();
        }
    }



}
