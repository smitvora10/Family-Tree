using FamilyTree.BL.Services;
using Microsoft.EntityFrameworkCore;
using System.Data;

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

        public List<TEntity> GetAll()
        {
            return _dbSet.ToList();
        }

        public TEntity GetById(int id)
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
