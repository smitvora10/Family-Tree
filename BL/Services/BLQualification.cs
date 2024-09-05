using FamilyTree.Core;
using FamilyTree.Data;
using FamilyTree.DB.Interfaces;
using FamilyTree.Models.Common;
using FamilyTree.Models.Master;
using Microsoft.EntityFrameworkCore;

namespace FamilyTree.BL.Services
{
    public class BLQualification : BLCommon<Qualification>, IQualificationService
    {
        private readonly IQualificationRepository _dbContext;
        private readonly DbSet<Qualification> _dbSet;
        private readonly DataContext _context;
        public BLQualification(IQualificationRepository dbContext, DataContext context) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbSet = context.Set<Qualification>();
        }


        public override Response ValidationBeforePreSave(Qualification qualification)
        {
            Response objResponse = new Response();
            //bool isDuplicate = _dbSet.HasDuplicate<Qualification>(
            //    rt => rt.Name,
            //    rt => rt.ShortDesc);

            //   if (isDuplicate)
            //{
            //    objResponse.IsError = true;
            //    objResponse.MessageCode = MessageCode.E002.ToString().Replace("~{handler}~", "Qualification");
            //}
            return objResponse;

        }


    }
}
