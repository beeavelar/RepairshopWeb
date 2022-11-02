using Microsoft.EntityFrameworkCore;
using RepairshopWeb.Data.Entities;
using RepairshopWeb.Helpers;
using RepairshopWeb.Models;
using System;
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

        public async Task AddItemToRepairOrderAsync(AddItemViewModel model, string userName)
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
                    User = user
                };
                _context.RepairOrderDetailsTemp.Add(repairOrderDetailTemp);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ConfirmRepairOrderAsync(string userName, int appointmentId)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
                return false;

            var repairOrderTemps = await _context.RepairOrderDetailsTemp
                .Include(rodt => rodt.Service)
                .Include(rodt => rodt.Vehicle)
                .Include(rodt => rodt.Mechanic)
                .Where(rodt => rodt.User == user)
                .ToListAsync();

            if (repairOrderTemps == null || repairOrderTemps.Count == 0)
                return false;

            //Passa as informações do RepairOrderDetailTemp para RepairOrderDetail
            var details = repairOrderTemps.Select(rod => new RepairOrderDetail
            {
                Vehicle = rod.Vehicle,
                VehicleId = rod.VehicleId,
                Service = rod.Service,
                ServiceId = rod.ServiceId,
                RepairPrice = rod.RepairPrice,
                Mechanic = rod.Mechanic,
                MechanicId = rod.MechanicId
            }).ToList();

            //Passa as informações do RepairOrderDetail para RepairOrder
            var repairOrder = new RepairOrder
            {
                Date = DateTime.Now,
                VehicleId = details[0].VehicleId,
                AppointmentId = appointmentId,
                User = user,
                Items = details
            };

            await CreateAsync(repairOrder);
            _context.RepairOrderDetailsTemp.RemoveRange(repairOrderTemps);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteDetailTempAsync(int id)
        {
            var repairOrderDetailTemp = await _context.RepairOrderDetailsTemp.FindAsync(id);
            if (repairOrderDetailTemp == null)
                return;

            _context.RepairOrderDetailsTemp.Remove(repairOrderDetailTemp);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRepairOrderAsync(int id)
        {
            var repairOrderDetails = await _context.RepairOrderDetails.Where(x => x.RepairOrderId == id).ToListAsync();
            if (repairOrderDetails == null)
                return;

            var repairOrder = await _context.RepairOrders.FindAsync(id);
            if (repairOrder == null)
                return;

            _context.RepairOrderDetails.RemoveRange(repairOrderDetails);
            _context.RepairOrders.Remove(repairOrder);
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

            if (await _userHelper.IsUserInRoleAsync(user, "ADMIN")) //Se o user for o Admin, buscar todas as RepairOrders
            {
                return _context.RepairOrders
                    .Include(ro => ro.Items)
                    .ThenInclude(s => s.Service)
                    .Include(v => v.Vehicle)
                    .OrderByDescending(ro => ro.Date);
            }

            if (await _userHelper.IsUserInRoleAsync(user, "CLIENT"))
            {
                return _context.RepairOrders
                    .Include(ro => ro.Items)
                    .ThenInclude(s => s.Service)
                    .Include(v => v.Vehicle)
                    .OrderByDescending(ro => ro.Date);
            }

            return _context.RepairOrders //Se não for Admin, buscar as RepairOrders do user que estiver logado
                .Include(ro => ro.Items)
                .ThenInclude(s => s.Service)
                .Include(v => v.Vehicle)
                .Where(ro => ro.User == user) //onde o User for igual ao user
                .OrderByDescending(ro => ro.Date);
        }

        public async Task StatusRepairOrder(RepairOrderStatusViewModel model)
        {
            var repairOrder = await _context.RepairOrders.FindAsync(model.Id);
            if (repairOrder == null)
                return;

            repairOrder.RepairStatus = model.RepairStatus;
            _context.RepairOrders.Update(repairOrder);
            await _context.SaveChangesAsync();
        }

        public async Task<RepairOrder> GetRepairOrderByIdAsync(int id)
        {
            return await _context.RepairOrders.FindAsync(id);
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.RepairOrders.Include(ro => ro.User);
        }
    }
}
