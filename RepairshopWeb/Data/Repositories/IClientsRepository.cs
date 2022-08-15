using System.Linq;
using RepairshopWeb.Data.Entities;
using RepairshopWeb.Data.Repository;

namespace RepairshopWeb.Data.Repositories
{
    public interface IClientsRepository : IGenericRepository<Client>
    {
        public IQueryable GetAllWithUsers();
    }
}
