using FamilyTree.Core;
using FamilyTree.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace FamilyTree.BL.Services
{
    public class BLCommon<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        private readonly IBaseRepository<TEntity> _dbContext;
        public Response response = new Response();
        public TEntity _entity;

        public enmEntryType EntryType { get; set; }

        protected BLCommon(IBaseRepository<TEntity> dbContext)
        {
            _dbContext = dbContext;
        }
        public virtual bool EntityExists(int id)
        {
            return _dbContext.EntityExists(id);
        }


        // ---------- COMMON HELPER ----------
        public virtual Response GetAll(string[]? includeFields = null, string[]? excludeFields = null)
        {
            response.DataModel = _dbContext.GetAll(includeFields, excludeFields);
            return response;
        }

        public virtual Response ValidationBeforePreSave(TEntity entity)
        {
            return response;
        }

        public virtual Response GetById(int id)
        {
            var data = _dbContext.GetById(id);
            if (data == null)
            {
                response.IsError = true;
                response.MessageCode = MessageCode.E001.ToString();
            }
            else
            {
                response.DataModel = data;
            }
            return response;
        }

        public virtual Response AddOrUpdate()
        {
            if (EntryType == enmEntryType.A)
                response.DataModel = _dbContext.Add(_entity);
            else
            {
                response.DataModel = _dbContext.Update(_entity);
            }
            return response;
        }

        public virtual bool HasDuplicate(string tableName, TEntity entity, params string[] keyFields)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (keyFields == null || keyFields.Length == 0)
                throw new ArgumentException("At least one key field must be provided.", nameof(keyFields));

            return _dbContext.HasDuplicate(tableName, entity, keyFields);
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

        public virtual void Presave(TEntity entity)
        {
            _entity = entity;
        }
    }
}
