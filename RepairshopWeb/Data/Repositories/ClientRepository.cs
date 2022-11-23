using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RepairshopWeb.Data.Entities;
using RepairshopWeb.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepairshopWeb.Data.Repositories
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        private readonly DataContext _context;

        public ClientRepository(DataContext context) : base(context) 
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Clients.Include(c => c.User);
        }

        public IEnumerable<SelectListItem> GetComboClients()
        {
            var list = _context.Clients.Select(c => new SelectListItem
            {
                Text = c.FullName,
                Value = c.Id.ToString()
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a client...",
                Value = "0"
            });
            return list;
        }

        public async Task<Client> GetClient(string email)
        {
            return await _context.Clients
                .FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<Client> GetClientByIdAsync(int id)
        {
            return await _context.Clients.FindAsync(id);
        }

        public async Task StatusClient(ClientStatusViewModel model)
        {
            var client = await _context.Clients.FindAsync(model.Id);
            if (client == null)
                return;

            client.ClientStatus = model.ClientStatus;
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
        }
    }
}
