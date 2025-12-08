using FamilyTree.Data;
using FamilyTree.Data.Common;
using FamilyTree.DB.Interfaces;
using FamilyTree.Models.Common;
using FamilyTree.Models.Master;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FamilyTree.BL.Services
{
    public class DBPerson : DBCommon<Person>, IPersonRepository
    {
        private readonly DbSet<Person> _dbSet;
        private readonly DataContext _context;
        public DBPerson(DataContext context) : base(context)
        {
            _dbSet = context.Set<Person>();
        }


        public override DataTable GetAll(CommonSearchModel model)
        {
            string sql = @"
SELECT
    p.*,
    CONCAT(COALESCE(f.FirstName, ''), ' ', COALESCE(f.LastName, '')) AS FatherName,
    CONCAT(COALESCE(m.FirstName, ''), ' ', COALESCE(m.LastName, '')) AS MotherName,
    CASE p.Gender
        WHEN 'M' THEN 'Male'
        WHEN 'F' THEN 'Female'
        ELSE p.Gender
    END AS GenderDesc,
    CASE p.MaritalStatus
        WHEN 'M' THEN 'Married'
        WHEN 'U' THEN 'Unmarried'
        WHEN 'W' THEN 'Widowed'
        WHEN 'D' THEN 'Divorced'
        ELSE p.MaritalStatus
    END AS MaritalStatusDesc
FROM Person p
LEFT JOIN Person f ON p.FatherId = f.PersonId
LEFT JOIN Person m ON p.MotherId = m.PersonId
WHERE 1=1";

            List<object> parameters = new List<object>();

            if (!string.IsNullOrEmpty(model.SearchValue))
            {
                sql += " AND (p.FirstName LIKE @p" + parameters.Count + " OR p.LastName LIKE @p" + parameters.Count + ")";
                parameters.Add($"%{model.SearchValue}%");
            }

            if (model.FilterList != null && model.FilterList.Count > 0)
            {
                foreach (KeyValuePair<string, string> filter in model.FilterList)
                {
                    // Basic SQL Injection prevention: Ensure key is alphanumeric
                    if (!filter.Key.All(char.IsLetterOrDigit)) continue;

                    // Mapping specific keys if needed, or direct column assumption. 
                    // Assuming Keys match Column names in Person table.
                    // Special handling can be added here.

                    sql += $" AND p.{filter.Key} = @p{parameters.Count}";
                    parameters.Add(filter.Value);
                }
            }

            return ExecuteSql(sql, parameters.ToArray());
        }

        public DataTable GetPersonDDL(CommonSearchModel model)
        {
            // Select relevant fields for DDL
            string sql = @"
SELECT 
    p.PersonId, 
    p.FirstName, 
    p.LastName, 
    p.Gender,
    CONCAT(COALESCE(p.FirstName, ''), ' ', COALESCE(p.LastName, '')) AS Name
FROM Person p
WHERE 1=1";

            List<object> parameters = new List<object>();

            if (!string.IsNullOrEmpty(model.SearchValue))
            {
                sql += " AND (p.FirstName LIKE @p" + parameters.Count + " OR p.LastName LIKE @p" + parameters.Count + ")";
                parameters.Add($"%{model.SearchValue}%");
            }

            if (model.FilterList != null)
            {
                if (model.FilterList.TryGetValue("Gender", out string? gender) && !string.IsNullOrEmpty(gender))
                {
                    sql += " AND p.Gender = @p" + parameters.Count;
                    parameters.Add(gender);

                    // Age Filter: Only applied if Gender is passed
                    // Default to true if not specified, or check value
                    bool filterWithAge = true;
                    if (model.FilterList.TryGetValue("FilterWithAge", out string? ageFilterStr))
                    {
                        bool.TryParse(ageFilterStr, out filterWithAge);
                    }

                    if (filterWithAge)
                    {
                        // Age > 18
                        sql += " AND (p.BirthDate IS NOT NULL AND DATE_ADD(p.BirthDate, INTERVAL 18 YEAR) <= CURDATE())";
                    }
                }
            }

            return ExecuteSql(sql, parameters.ToArray());
        }

        public override DataTable GetById(int id)
        {
            string sql = @"
SELECT
    p.*,
    CONCAT(COALESCE(f.FirstName, ''), ' ', COALESCE(f.LastName, '')) AS FatherName,
    CONCAT(COALESCE(m.FirstName, ''), ' ', COALESCE(m.LastName, '')) AS MotherName,
    CASE p.Gender
        WHEN 'M' THEN 'Male'
        WHEN 'F' THEN 'Female'
        ELSE p.Gender
    END AS GenderDesc,
    CASE p.MaritalStatus
        WHEN 'M' THEN 'Married'
        WHEN 'U' THEN 'Unmarried'
        WHEN 'W' THEN 'Widowed'
        WHEN 'D' THEN 'Divorced'
        ELSE p.MaritalStatus
    END AS MaritalStatusDesc
FROM Person p
LEFT JOIN Person f ON p.FatherId = f.PersonId
LEFT JOIN Person m ON p.MotherId = m.PersonId
WHERE p.PersonId = @p0";

            return ExecuteSql(sql, id);
        }

        public DataTable GetTree()
        {
            throw new NotImplementedException();
        }
    }



}
