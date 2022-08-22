using Microsoft.EntityFrameworkCore;
using RepairshopWeb.Data.Entities;
using System.Linq;

namespace RepairshopWeb.Data.Repositories
{
    public class MechanicRepository : GenericRepository<Mechanic>, IMechanicRepository
    {
        private readonly DataContext _context;

        public MechanicRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Mechanics.Include(o => o.User);
        }
    }
}
