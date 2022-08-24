using RepairshopWeb.Data.Entities;
using RepairshopWeb.Data.Repository;
using System.Linq;

namespace RepairshopWeb.Data.Repositories
{
    public interface IVehicleRepository : IGenericRepository<Vehicle>
    {
        public IQueryable GetAllWithUsers();
    }
}
