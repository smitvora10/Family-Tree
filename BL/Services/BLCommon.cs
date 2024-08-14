using FamilyTree.Core;
using FamilyTree.Data;
using FamilyTree.Data.Common;
using FamilyTree.Models.Common;
using FamilyTree.Models.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FamilyTree.BL.Services
{
    public class BLCommon<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        private readonly IBaseRepository<TEntity> _dbContext;
        Response response = new Response();

        public enmEntryType EntryType { get; set; }

        protected BLCommon(IBaseRepository<TEntity> dbContext)
        {
            _dbContext = dbContext;
        }
        public virtual bool EntityExists(int id)
        {
            return _dbContext.EntityExists(id);
        }

        public virtual Response GetAll()
        {
            response.DataModel = _dbContext.GetAll();
            return response;
        }

        public virtual Response ValidationBeforePreSave(TEntity entity)
        {
            return response;
        }

        public virtual Response GetById(int id)
        {
            if (id == 0 || !_dbContext.EntityExists(id))
            {
                response.IsError = true;
                response.MessageCode = MessageCode.E001.ToString();
            }
            else
            {
                response.DataModel = _dbContext.GetById(id);
            }
            return response;
        }

        public virtual Response AddOrUpdate(TEntity entity)
        {
            if (EntryType == enmEntryType.A)
                response.DataModel = _dbContext.Add(entity);
            else
            {
                response.DataModel = _dbContext.Update(entity);
            }
            return response;
        }

        public virtual Response Delete(int id)
        {
            if (EntityExists(id))
            {
                response.DataModel = _dbContext.Delete(id);
            }
            else
            {
                response.IsError = true;
                response.MessageCode = MessageCode.E001.ToString();
            }

            return response;
        }
    }
}
