using Microsoft.EntityFrameworkCore;
using RepairshopWeb.Data.Entities;
using System.Linq;

namespace RepairshopWeb.Data.Repositories
{
    public class BillingRepository : GenericRepository<Billing>, IBillingRepository
    {
        private readonly DataContext _context;
        public BillingRepository(DataContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable GetAllWithUsers()
        {
            return _context.Billings.Include(b => b.User);
        }
    }
}
