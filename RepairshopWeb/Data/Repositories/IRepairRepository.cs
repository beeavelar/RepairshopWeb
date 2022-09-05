using RepairshopWeb.Data.Entities;
using RepairshopWeb.Data.Repository;
using System.Linq;

namespace RepairshopWeb.Data.Repositories
{
    public interface IRepairRepository : IGenericRepository<Repair>
    {
        public IQueryable GetAllWithUsers();
    }
}
