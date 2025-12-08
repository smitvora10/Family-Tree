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
    using FamilyTree.Models.Common;

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
                IReadOnlyDictionary<string, PropertyInfo> properties = GetPropertyMap(t);

                if (properties.TryGetValue("Id", out PropertyInfo? idProp))
                {
                    return idProp;
                }

                properties.TryGetValue($"{t.Name}Id", out PropertyInfo? namedIdProp);
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
            List<string> allProps = typeof(TEntity).GetProperties()
                .Select(p => p.Name)
                .ToList();

            if (includeFields != null && includeFields.Length > 0)
                return allProps.Intersect(includeFields, StringComparer.OrdinalIgnoreCase).ToList();

            if (excludeFields != null && excludeFields.Length > 0)
                return allProps.Except(excludeFields, StringComparer.OrdinalIgnoreCase).ToList();

            return null;
        }

        private DataTable ListToDataTable(IEnumerable<dynamic> items)
        {
            var dataTable = new DataTable(typeof(TEntity).Name);
            var itemList = items.ToList();

            if (itemList.Count == 0) return dataTable;

            var firstItem = (object)itemList[0];
            var properties = firstItem.GetType().GetProperties();

            foreach (var prop in properties)
            {
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                dataTable.Columns.Add(prop.Name, type);
            }

            foreach (var item in itemList)
            {
                var values = new object?[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        private DataTable ListToDataTable<T>(IEnumerable<T> items)
        {
            var dataTable = new DataTable(typeof(TEntity).Name);
            // Get all properties of the Entity
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                // DataTable doesn't support some types, careful here
                dataTable.Columns.Add(prop.Name, type);
            }

            foreach (var item in items)
            {
                var values = new object?[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        // --------------------- GET ALL ---------------------
        // --------------------- GET DDL DATA ---------------------
        public virtual DataTable GetDDLData(CommonDDLRequest model)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            List<string>? fieldList = BuildFieldList(model?.IncludeFields, model?.ExcludeFields);

            // If no filtering, return everything
            if (fieldList == null)
            {
                // Dynamic list to datatable logic or generic
                // Use simple reflection if full entity
                return ListToDataTable(query.ToList());
            }

            if (fieldList.Count == 0)
                return new DataTable();

            // Build selector string for dynamic LINQ
            string selector = $"new({string.Join(",", fieldList)})";

            // Use System.Linq.Dynamic.Core extension
            var dynamicList = query.Select(selector).ToDynamicList();
            return ListToDataTable(dynamicList);
        }

        public virtual DataTable GetAll(CommonSearchModel model)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();

            // 1. Generic Filter List
            if (model.FilterList != null && model.FilterList.Count > 0)
            {
                IReadOnlyDictionary<string, PropertyInfo> propertyMap = GetPropertyMap(typeof(TEntity));
                int i = 0;
                List<object> values = new List<object>();
                string whereClause = "";

                foreach (KeyValuePair<string, string> filter in model.FilterList)
                {
                    if (propertyMap.ContainsKey(filter.Key))
                    {
                        // Use dynamic linq syntax: "Field == @0"
                        if (whereClause.Length > 0) whereClause += " && ";
                        whereClause += $"{filter.Key} == @{i}";
                        values.Add(filter.Value);
                        i++;
                    }
                }

                if (whereClause.Length > 0)
                {
                    query = query.Where(whereClause, values.ToArray());
                }
            }

            // 2. Search Value (Generic Attempt or Skip)
            // For now, base implementation does not guess fields for generic search. 
            // Subclasses should override this if they want specific text search (like DBPerson).

            return ListToDataTable(query.ToList());
        }



        public bool HasDuplicate(string tableName, TEntity entity, params string[] keyFields)
        {
            var entityType = typeof(TEntity);

            if (!string.Equals(tableName, entityType.Name, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException($"Table name '{tableName}' does not match entity type '{entityType.Name}'.", nameof(tableName));

            IReadOnlyDictionary<string, PropertyInfo> propertyMap = GetPropertyMap(entityType);
            List<string> comparisons = new List<string>();
            List<object?> parameters = new List<object?>();

            foreach (string field in keyFields)
            {
                if (string.IsNullOrWhiteSpace(field))
                    throw new ArgumentException("Key field names cannot be null or whitespace.", nameof(keyFields));

                if (!propertyMap.TryGetValue(field, out PropertyInfo? property))
                    throw new ArgumentException($"Property '{field}' was not found on type '{entityType.Name}'.", nameof(keyFields));

                object? value = property.GetValue(entity);

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

            PropertyInfo? idProperty = GetIdProperty(entityType);
            if (idProperty != null)
            {
                object? idValue = idProperty.GetValue(entity);
                if (!IsDefaultValue(idValue, idProperty.PropertyType))
                {
                    comparisons.Add($"{idProperty.Name} != @{parameters.Count}");
                    parameters.Add(idValue);
                }
            }

            if (comparisons.Count == 0)
                return false;

            string predicate = string.Join(" AND ", comparisons);
            return _dbSet.AsNoTracking().Where(predicate, parameters.ToArray()).Any();
        }

        private static bool IsDefaultValue(object? value, Type type)
        {
            if (value == null)
                return true;

            if (!type.IsValueType)
                return false;

            Type underlying = Nullable.GetUnderlyingType(type) ?? type;
            object? defaultValue = Activator.CreateInstance(underlying);

            return value.Equals(defaultValue);
        }

        public virtual DataTable GetById(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null) return new DataTable(); // Or appropriate empty
            return ListToDataTable(new List<TEntity> { entity });
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
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> result = _dbSet.Add(entity);
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
            TEntity? entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                delOpt = _context.SaveChanges();
            }
            return delOpt;
        }


        public DataTable ExecuteSql(string sql, params object[] parameters)
        {
            DataTable dataTable = new DataTable();

            using (System.Data.Common.DbCommand command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                if (parameters != null)
                {
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        System.Data.Common.DbParameter parameter = command.CreateParameter();
                        parameter.ParameterName = $"@p{i}";
                        parameter.Value = parameters[i];
                        command.Parameters.Add(parameter);
                    }
                }

                if (_context.Database.GetDbConnection().State == ConnectionState.Closed)
                {
                    _context.Database.GetDbConnection().Open();
                }

                using (System.Data.Common.DbDataReader reader = command.ExecuteReader())
                {
                    dataTable.Load(reader);
                }

                _context.Database.GetDbConnection().Close();
            }

            return dataTable;
        }


    }
}
