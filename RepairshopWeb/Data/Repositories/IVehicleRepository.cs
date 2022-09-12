using Microsoft.AspNetCore.Mvc.Rendering;
using RepairshopWeb.Data.Entities;
using RepairshopWeb.Data.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepairshopWeb.Data.Repositories
{
    public interface IVehicleRepository : IGenericRepository<Vehicle>
    {
        public IQueryable GetAllWithUsers();

    }
}
