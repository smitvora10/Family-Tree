using FamilyTree.BL.Services;
using System;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace FamilyTree.Data.Common
{
    public class DBCommon<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private static readonly ConcurrentDictionary<Type, IReadOnlyDictionary<string, PropertyInfo>> _propertyCache = new();
        private static readonly ConcurrentDictionary<Type, PropertyInfo?> _idPropertyCache = new();

        private static IReadOnlyDictionary<string, PropertyInfo> GetPropertyMap(Type type)
        {
            return _propertyCache.GetOrAdd(type, static t =>
                t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase));
        }

        private static PropertyInfo? GetIdProperty(Type type)
        {
            return _idPropertyCache.GetOrAdd(type, static t =>
            {
                var properties = GetPropertyMap(t);

                if (properties.TryGetValue("Id", out var idProp))
                {
                    return idProp;
                }

                properties.TryGetValue($"{t.Name}Id", out var namedIdProp);
                return namedIdProp;
            });
        }

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
        public virtual object GetAll(string[]? includeFields = null, string[]? excludeFields = null)
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



        public bool HasDuplicate(string tableName, TEntity entity, params string[] keyFields)
        {
            var entityType = typeof(TEntity);

            if (!string.Equals(tableName, entityType.Name, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException($"Table name '{tableName}' does not match entity type '{entityType.Name}'.", nameof(tableName));

            var propertyMap = GetPropertyMap(entityType);
            var comparisons = new List<string>();
            var parameters = new List<object?>();

            foreach (var field in keyFields)
            {
                if (string.IsNullOrWhiteSpace(field))
                    throw new ArgumentException("Key field names cannot be null or whitespace.", nameof(keyFields));

                if (!propertyMap.TryGetValue(field, out var property))
                    throw new ArgumentException($"Property '{field}' was not found on type '{entityType.Name}'.", nameof(keyFields));

                var value = property.GetValue(entity);

                if (value == null)
                {
                    comparisons.Add($"{property.Name} == null");
                }
                else
                {
                    comparisons.Add($"{property.Name} == @{parameters.Count}");
                    parameters.Add(value);
                }
            }

            var idProperty = GetIdProperty(entityType);
            if (idProperty != null)
            {
                var idValue = idProperty.GetValue(entity);
                if (!IsDefaultValue(idValue, idProperty.PropertyType))
                {
                    comparisons.Add($"{idProperty.Name} != @{parameters.Count}");
                    parameters.Add(idValue);
                }
            }

            if (comparisons.Count == 0)
                return false;

            var predicate = string.Join(" AND ", comparisons);
            return _dbSet.AsNoTracking().Where(predicate, parameters.ToArray()).Any();
        }

        private static bool IsDefaultValue(object? value, Type type)
        {
            if (value == null)
                return true;

            if (!type.IsValueType)
                return false;

            var underlying = Nullable.GetUnderlyingType(type) ?? type;
            var defaultValue = Activator.CreateInstance(underlying);

            return value.Equals(defaultValue);
        }

        public virtual object GetById(int id)
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
