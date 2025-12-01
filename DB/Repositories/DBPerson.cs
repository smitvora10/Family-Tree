using FamilyTree.Data;
using FamilyTree.Data.Common;
using FamilyTree.DB.Interfaces;
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
        public override object GetAll(string[]? includeFields = null, string[]? excludeFields = null)
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
LEFT JOIN Person m ON p.MotherId = m.PersonId";

            return ExecuteSql(sql);
        }

        public override object GetById(int id)
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
