using Microsoft.AspNetCore.Mvc.Rendering;
using RepairshopWeb.Data.Entities;
using RepairshopWeb.Data.Repository;
using System.Collections.Generic;
using System.Linq;

namespace RepairshopWeb.Data.Repositories
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        public IQueryable GetAllWithUsers();

        IEnumerable<SelectListItem> GetComboClients();

    }
}
