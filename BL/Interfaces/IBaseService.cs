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
        /// Gets generic DDL data with restricted fields
        /// </summary>
        /// <param name="model">The DDL request model containing include/exclude fields</param>
        /// <returns></returns>
        Response GetDDLData(CommonDDLRequest model);

        /// <summary>
        /// Get All Records with Search and Filter
        /// </summary>
        /// <param name="model">Search and Filter Model</param>
        /// <returns></returns>
        Response GetAll(CommonSearchModel model);

        /// <summary>
        /// Checks whether the supplied entity has duplicate values for the provided fields
        /// within the corresponding table.
        /// </summary>
        /// <param name="tableName">Name of the model/table to validate against.</param>
        /// <param name="entity">Current entity instance whose values should be validated.</param>
        /// <param name="keyFields">Fields that must be unique individually or in combination.</param>
        /// <returns>True when a duplicate exists; otherwise false.</returns>
        bool HasDuplicate(string tableName, TEntity entity, params string[] keyFields);

        /// <summary>
        /// Get Data By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Response GetById(int id);

        //Add Or Update
        Response AddOrUpdate();

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
