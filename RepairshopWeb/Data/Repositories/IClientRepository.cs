using System.Linq;
using RepairshopWeb.Data.Entities;
using RepairshopWeb.Data.Repository;

namespace RepairshopWeb.Data.Repositories
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        public IQueryable GetAllWithUsers();
    }
}
