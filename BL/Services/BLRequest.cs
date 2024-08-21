using FamilyTree.Data;
using FamilyTree.DB.Interfaces;
using FamilyTree.Models.Master;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FamilyTree.BL.Services
{
    public class BLRequest : BLCommon<Request>, IRequestService
    {
        private readonly IRequestRepository _dbContext;
        private readonly DbSet<Request> _dbSet;
        private readonly DataContext _context;
        public BLRequest(IRequestRepository dbContext, DataContext context) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbSet = context.Set<Request>();
        }

        public override void Presave(Request entity)
        {
            base.Presave(entity);
            entity.Person = JsonConvert.SerializeObject(entity.Person);
        }
    }
}
