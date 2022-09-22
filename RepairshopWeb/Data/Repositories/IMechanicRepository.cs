using Microsoft.AspNetCore.Mvc.Rendering;
using RepairshopWeb.Data.Entities;
using RepairshopWeb.Data.Repository;
using System.Collections.Generic;
using System.Linq;

namespace RepairshopWeb.Data.Repositories
{
    public interface IMechanicRepository : IGenericRepository<Mechanic>
    {
        public IQueryable GetAllWithUsers();

        IEnumerable<SelectListItem> GetComboMechanics();
    }
}
