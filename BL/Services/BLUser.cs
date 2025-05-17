using FamilyTree.Core;
using FamilyTree.Data;
using FamilyTree.DB.Interfaces;
using FamilyTree.Models.Common;
using FamilyTree.Models.Master;
using Microsoft.EntityFrameworkCore;

namespace FamilyTree.BL.Services
{
    public class BLUser : BLCommon<User>, IUserService
    {
        private readonly IUserRepository _dbContext;
        private readonly DbSet<User> _dbSet;
        private readonly DataContext _context;
        public BLUser(IUserRepository dbContext, DataContext context) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbSet = context.Set<User>();
        }


        public override void Presave(User entity)
        {
            base.Presave(entity);

            _entity.PasswordHash = PasswordEncryptionDecryption.HashPassword(entity.Password);
        }

        public override Response ValidationBeforePreSave(User user)
        {
            Response objResponse = new Response();
            //bool isDuplicate = _dbSet.HasDuplicate<User>(
            //    rt => rt.Name,
            //    rt => rt.ShortDesc);

            //   if (isDuplicate)
            //{
            //    objResponse.IsError = true;
            //    objResponse.MessageCode = MessageCode.E002.ToString().Replace("~{handler}~", "User");
            //}
            return objResponse;

        }


    }
}
