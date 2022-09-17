using RepairshopWeb.Data.Entities;
using RepairshopWeb.Data.Repository;
using System.Linq;

namespace RepairshopWeb.Data.Repositories
{
    public interface IServiceRepository : IGenericRepository<Service>
    {
        public IQueryable GetAllWithUsers();
    }
}
