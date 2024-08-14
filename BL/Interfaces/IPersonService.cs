using FamilyTree.Models.Common;
using FamilyTree.Models.Master;

namespace FamilyTree.BL.Services
{
    public interface IPersonService : IBaseService<Person>
    {
        Response GetWholeTree();

        List<Person> BuildFamilyTree(Person currentPerson = null);
    }
}
