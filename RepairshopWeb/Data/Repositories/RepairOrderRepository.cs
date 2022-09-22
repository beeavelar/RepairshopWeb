using Microsoft.EntityFrameworkCore;
using RepairshopWeb.Data.Entities;
using RepairshopWeb.Helpers;
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

        public async Task<IQueryable<RepairOrderDetailTemp>> GetDetailsTempsAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user==null)
                return null;

            return _context.RepairOrderDetailsTemp
                .Include(s => s.Service)
                .Where(ro => ro.User == user)
                .OrderBy(ro => ro.Service.Description);
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
