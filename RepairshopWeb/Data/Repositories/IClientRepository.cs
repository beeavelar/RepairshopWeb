using Microsoft.AspNetCore.Mvc.Rendering;
using RepairshopWeb.Data.Entities;
using RepairshopWeb.Data.Repository;
using RepairshopWeb.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepairshopWeb.Data.Repositories
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        public IQueryable GetAllWithUsers();

        IEnumerable<SelectListItem> GetComboClients();

        Task<Client> GetClient(string email);

        Task<Client> GetClientByIdAsync(int id);  //Buscar os clients por id

        Task StatusClient(ClientStatusViewModel model);
    }
}
