using FamilyTree.Core;
using FamilyTree.Data;
using FamilyTree.Data.Common;
using FamilyTree.DB.Interfaces;
using FamilyTree.Models.Common;
using FamilyTree.Models.Master;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;

namespace FamilyTree.BL.Services
{
    public class BLRelationType : BLCommon<RelationType>, IRelationTypeService
    {
        private readonly IRelationTypeRepository _dbContext;
        private readonly DbSet<RelationType> _dbSet;
        private readonly DataContext _context;
        public BLRelationType(IRelationTypeRepository dbContext, DataContext context) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbSet = context.Set<RelationType>();
        }


        public override Response ValidationBeforePreSave(RelationType relationType)
        {
            Response objResponse = new Response();
            bool isDuplicate = _dbSet.HasDuplicate(
                relationType,
                rt => rt.RelationTypeCode,
                rt => rt.RelationTypeDescription);

            if (isDuplicate)
            {
                objResponse.IsError = true;
                objResponse.MessageCode = MessageCode.E002.ToString().Replace("~{handler}~", "Relation Type");
            }
            return objResponse;

        }

        
    }
}
