using FamilyTree.Core;
using FamilyTree.Data;
using FamilyTree.DB.Interfaces;
using FamilyTree.Models.Common;
using FamilyTree.Models.Master;
using Microsoft.EntityFrameworkCore;

namespace FamilyTree.BL.Services
{
    public class BLOccupation : BLCommon<Occupation>, IOccupationService
    {
        private readonly IOccupationRepository _dbContext;
        private readonly DbSet<Occupation> _dbSet;
        private readonly DataContext _context;
        public BLOccupation(IOccupationRepository dbContext, DataContext context) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbSet = context.Set<Occupation>();
        }


        public override Response ValidationBeforePreSave(Occupation relationType)
        {
            Response objResponse = new Response();
            bool isDuplicate = _dbSet.HasDuplicate(
                relationType,
                rt => rt.OccupationDesc);

            if (isDuplicate)
            {
                objResponse.IsError = true;
                objResponse.MessageCode = MessageCode.E002.ToString().Replace("~{handler}~", "Occupation");
            }
            return objResponse;

        }


    }
}
