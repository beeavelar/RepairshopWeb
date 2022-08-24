using Microsoft.EntityFrameworkCore;
using RepairshopWeb.Data.Entities;
using System.Linq;

namespace RepairshopWeb.Data.Repositories
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        private readonly DataContext _context;

        public VehicleRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Vehicles.Include(o => o.User);
        }
    }
}
