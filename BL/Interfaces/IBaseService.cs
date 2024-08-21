using FamilyTree.Core;
using FamilyTree.Models.Common;

namespace FamilyTree.BL.Services
{
    public interface IBaseService<TEntity>
    {

        enmEntryType EntryType { get; set; }

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
        Response GetAll();

        /// <summary>
        /// Get Data By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Response GetById(int id);

        //Add Or Update
        Response AddOrUpdate(TEntity entity);

        /// <summary>
        /// Delete the entity by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Response Delete(int id);

        Response ValidationBeforePreSave(TEntity entity);

        void Presave(TEntity entity);
    }
}
