using Microsoft.AspNetCore.Mvc.Rendering;
using RepairshopWeb.Data.Entities;
using RepairshopWeb.Data.Repository;
using System.Collections.Generic;
using System.Linq;

namespace RepairshopWeb.Data.Repositories
{
    public interface IServiceRepository : IGenericRepository<Service>
    {
        public IQueryable GetAllWithUsers();

        IEnumerable<SelectListItem> GetComboServices();
    }
}
