using Microsoft.EntityFrameworkCore;
using RepairshopWeb.Data.Entities;
using RepairshopWeb.Helpers;
using RepairshopWeb.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RepairshopWeb.Data.Repositories
{
    public class RepairOrderRepository : GenericRepository<RepairOrder>, IRepairOrderRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public RepairOrderRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task AddItemToOrderAsync(AddItemViewModel model, string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
                return;

            var service = await _context.Services.FindAsync(model.ServiceId);
            if(service == null)
                return;

            var vehicle = await _context.Vehicles.FindAsync(model.VehicleId);
            if (vehicle == null)
                return;

            var mechanic = await _context.Mechanics.FindAsync(model.MechanicId);
            if (mechanic == null)
                return;

            var repairOrderDetailTemp = await _context.RepairOrderDetailsTemp
                .Where(rodt => rodt.User == user && rodt.Service == service && rodt.Vehicle == vehicle && rodt.Mechanic == mechanic)
                .FirstOrDefaultAsync();

            if(repairOrderDetailTemp == null)
            {
                repairOrderDetailTemp = new RepairOrderDetailTemp
                {
                    Vehicle = vehicle,
                    Service = service,
                    RepairPrice = service.RepairPrice,
                    Mechanic = mechanic,
                    User = user,
                };
                _context.RepairOrderDetailsTemp.Add(repairOrderDetailTemp);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDetailTempAsync(int id)
        {
            var repairOrderDetailTemp = await _context.RepairOrderDetailsTemp.FindAsync(id);
            if (repairOrderDetailTemp == null)
                return;

            _context.RepairOrderDetailsTemp.Remove(repairOrderDetailTemp);
            await _context.SaveChangesAsync();
        }

        public async Task<IQueryable<RepairOrderDetailTemp>> GetDetailsTempsAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user==null)
                return null;

            return _context.RepairOrderDetailsTemp
               .Include(v => v.Vehicle)
               .Include(s => s.Service)
               .Include(m => m.Mechanic)
               .Where(ro => ro.User == user)
               .OrderBy(ro => ro.Vehicle.Id);
        }

        public async Task<IQueryable<RepairOrder>> GetRepairOrderAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
                return null;

            if (await _userHelper.IsUserInRoleAsync(user, "Admin")) //Se o user for o Admin, buscar todas as RepairOrders
            {
                return _context.RepairOrders
                    .Include(ro => ro.Items)
                    .ThenInclude(s => s.Service)
                    .OrderByDescending(ro => ro.RepairOrderDate);
            }

            return _context.RepairOrders //Se não for Admin, buscar as RepairOrders do user(cliente) que estiver logado
                .Include(ro => ro.Items)
                .ThenInclude(s => s.Service)
                .Where(ro => ro.User == user) //onde o User for igual ao user
                .OrderByDescending(ro => ro.RepairOrderDate);
        }
    }
}
