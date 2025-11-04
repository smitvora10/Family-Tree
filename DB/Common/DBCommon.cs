using FamilyTree.BL.Services;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Dynamic.Core;

namespace FamilyTree.Data.Common
{
    public class DBCommon<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly DataContext _context;

        public DBCommon(DataContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        // ---------- COMMON HELPER ----------
        private List<string>? BuildFieldList(string[]? includeFields, string[]? excludeFields)
        {
            var allProps = typeof(TEntity).GetProperties()
                .Select(p => p.Name)
                .ToList();

            if (includeFields != null && includeFields.Length > 0)
                return allProps.Intersect(includeFields, StringComparer.OrdinalIgnoreCase).ToList();

            if (excludeFields != null && excludeFields.Length > 0)
                return allProps.Except(excludeFields, StringComparer.OrdinalIgnoreCase).ToList();

            return null;
        }

        //public List<TEntity> GetAll(string[]? includeFields = null, string[]? excludeFields = null)
        //{
        //    IQueryable<TEntity> query = _dbSet.AsNoTracking();
        //    var fieldList = BuildFieldList(includeFields, excludeFields);

        //    // If no filtering, return all
        //    if (fieldList == null || fieldList.Count == 0)
        //        return query.ToList();

        //    // Dynamic projection using System.Linq.Dynamic.Core
        //    string selector = $"new({string.Join(",", fieldList)})";
        //    // Fix: Use System.Linq.Dynamic.Core's Select extension method, which returns dynamic objects.
        //    // Cast result to List<dynamic> instead of List<TEntity>
        //    return _dbSet.Select(selector).ToDynamicList();
        //}

        // --------------------- GET ALL ---------------------
        public List<dynamic> GetAll(string[]? includeFields = null, string[]? excludeFields = null)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            var fieldList = BuildFieldList(includeFields, excludeFields);

            // If no filtering, return everything
            if (fieldList == null)
                return query.ToList().Cast<dynamic>().ToList();

            if (fieldList.Count == 0)
                return new List<dynamic>();

            // Build selector string for dynamic LINQ
            string selector = $"new({string.Join(",", fieldList)})";

            // ✅ Use System.Linq.Dynamic.Core extension
            return query.Select(selector).ToDynamicList();
        }

        public TEntity GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public TEntity CheckDuplicate(int id)
        {
            return _dbSet.Find(id);
        }

        public bool EntityExists(int id)
        {
            return _dbSet.Find(id) != null;
        }

        public TEntity Add(TEntity entity)
        {
            var result = _dbSet.Add(entity);
            _context.SaveChanges();
            return result.Entity;
        }

        public TEntity Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
            return entity;
        }

        public int Delete(int id)
        {
            int delOpt = 0;
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                delOpt = _context.SaveChanges();
            }
            return delOpt;
        }


        public DataTable ExecuteSql(string sql, params object[] parameters)
        {
            var dataTable = new DataTable();

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                if (parameters != null)
                {
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var parameter = command.CreateParameter();
                        parameter.ParameterName = $"@p{i}";
                        parameter.Value = parameters[i];
                        command.Parameters.Add(parameter);
                    }
                }

                if (_context.Database.GetDbConnection().State == ConnectionState.Closed)
                {
                    _context.Database.GetDbConnection().Open();
                }

                using (var reader = command.ExecuteReader())
                {
                    dataTable.Load(reader);
                }

                _context.Database.GetDbConnection().Close();
            }

            return dataTable;
        }


    }
}
