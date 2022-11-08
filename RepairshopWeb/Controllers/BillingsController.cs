using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RepairshopWeb.Data;
using RepairshopWeb.Data.Entities;
using RepairshopWeb.Data.Repositories;
using RepairshopWeb.Helpers;

namespace RepairshopWeb.Controllers
{
    public class BillingsController : Controller
    {
        private readonly DataContext _context;
        private readonly IBillingRepository _billingRepository;
        private readonly IUserHelper _userHelper;

        public BillingsController(DataContext context,
            IBillingRepository billingRepository, IUserHelper userHelper)
        {
            _context = context;
            _billingRepository = billingRepository;
            _userHelper = userHelper;
        }

        // GET: Billings
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Billings.Include(b => b.Client).Include(b => b.RepairOrder).Include(b => b.Vehicle);
            return View(await dataContext.ToListAsync());
        }

        // GET: Billings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("BillingNotFound");

            var billing = await _context.Billings
                .Include(b => b.Client)
                .Include(b => b.RepairOrder)
                .Include(b => b.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (billing == null)
                return new NotFoundViewResult("BillingNotFound");

            return View(billing);
        }

        // GET: Billings/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FullName");
            ViewData["RepairOrderId"] = new SelectList(_context.RepairOrders, "Id", "Id");
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "LicensePlate");
            return View();
        }

        // POST: Billings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Billing billing)
        {
            if (ModelState.IsValid)
            {
                billing.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                await _billingRepository.CreateAsync(billing);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FullName", billing.ClientId);
            ViewData["RepairOrderId"] = new SelectList(_context.RepairOrders, "Id", "Id", billing.RepairOrderId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "LicensePlate", billing.VehicleId);
            return View(billing);
        }

        // GET: Billings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("BillingNotFound");

            var billing = await _billingRepository.GetByIdAsync(id.Value);

            if (billing == null)
                return new NotFoundViewResult("BillingNotFound");

            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FullName", billing.ClientId);
            ViewData["RepairOrderId"] = new SelectList(_context.RepairOrders, "Id", "Id", billing.RepairOrderId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "LicensePlate", billing.VehicleId);
            return View(billing);
        }

        // POST: Billings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Billing billing)
        {
            if (id != billing.Id)
                return new NotFoundViewResult("BillingNotFound");

            if (ModelState.IsValid)
            {
                try
                {
                    billing.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    await _billingRepository.UpdateAsync(billing);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _billingRepository.ExistAsync(billing.Id))
                        return new NotFoundViewResult("BillingNotFound");
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FullName", billing.ClientId);
            ViewData["RepairOrderId"] = new SelectList(_context.RepairOrders, "Id", "Id", billing.RepairOrderId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "LicensePlate", billing.VehicleId);
            return View(billing);
        }

        // GET: Billings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("BillingNotFound");

            var billing = await _context.Billings
                .Include(b => b.Client)
                .Include(b => b.RepairOrder)
                .Include(b => b.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (billing == null)
                return new NotFoundViewResult("BillingNotFound");

            return View(billing);
        }

        // POST: Billings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var billing = await _billingRepository.GetByIdAsync(id);
            await _billingRepository.DeleteAsync(billing);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult BillingNotFound()
        {
            return View();
        }
    }
}
