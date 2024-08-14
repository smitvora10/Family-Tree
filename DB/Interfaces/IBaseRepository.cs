using FamilyTree.Core;
using FamilyTree.Models.Common;
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
        List<TEntity> GetAll();

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
