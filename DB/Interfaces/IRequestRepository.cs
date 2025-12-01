using System.Data;
using FamilyTree.BL.Services;
using FamilyTree.Models.Master;

namespace FamilyTree.DB.Interfaces
{
    public interface IRequestRepository : IBaseRepository<Request>
    {
        DataTable GetDetailedRequests(int? userId = null);
        DataTable GetDetailedRequestById(int id);
    }
}
