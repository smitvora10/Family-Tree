using FamilyTree.BL.Services;
using FamilyTree.Models.Common;
using FamilyTree.Models.Master;
using System.Data;


namespace FamilyTree.DB.Interfaces
{
    public interface IPersonRepository : IBaseRepository<Person>
    {
        DataTable GetTree();

        /// <summary>
        /// Get Person DDL Data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        DataTable GetPersonDDL(CommonSearchModel model);
    }
}
