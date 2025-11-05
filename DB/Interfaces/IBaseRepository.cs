using System.Data;

namespace FamilyTree.BL.Services
{
    public interface IBaseRepository<TEntity>
    {

        /// <summary>
        /// Check if the current entity exists or not
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool EntityExists(int id);

        /// <summary>
        /// Get All Records
        /// </summary>
        /// <returns></returns>
        List<dynamic> GetAll(string[]? includeFields = null, string[]? excludeFields = null);

        /// <summary>
        /// Checks if an entity with matching key fields already exists.
        /// </summary>
        /// <param name="tableName">Name of the table/model that should be queried.</param>
        /// <param name="entity">Entity whose values are used to search for duplicates.</param>
        /// <param name="keyFields">One or more fields whose combination must be unique.</param>
        /// <returns>True when a matching record exists; otherwise false.</returns>
        bool HasDuplicate(string tableName, TEntity entity, params string[] keyFields);

        /// <summary>
        /// Get Data By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetById(int id);

        //Add
        TEntity Add(TEntity entity);

        /// <summary>
        /// Update the entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity Update(TEntity entity);

        /// <summary>
        /// Delete the Entity by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Delete(int id);

        DataTable ExecuteSql(string sql, params object[] parameters);
    }
}
