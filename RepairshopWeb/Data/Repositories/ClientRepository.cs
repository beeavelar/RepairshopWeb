using Microsoft.EntityFrameworkCore;
using RepairshopWeb.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace RepairshopWeb.Data.Repositories
{
    public class ClientRepository : GenericRepository<Client>, IClientsRepository
    {
        private readonly DataContext _context;

        public ClientRepository(DataContext context) : base(context) 
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Clients.Include(o => o.User);
        }
    }
}
