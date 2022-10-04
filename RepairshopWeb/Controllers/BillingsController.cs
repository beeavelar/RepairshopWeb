using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IBillingRepository _vehicleRepository;
        private readonly IUserHelper _userHelper;

        public BillingsController(DataContext context,
            IBillingRepository vehicleRepository, IUserHelper userHelper)
        {
            _context = context;
            _vehicleRepository = vehicleRepository;
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
            {
                return NotFound();
            }

            var billing = await _context.Billings
                .Include(b => b.Client)
                .Include(b => b.RepairOrder)
                .Include(b => b.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (billing == null)
            {
                return NotFound();
            }

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
        public async Task<IActionResult> Create([Bind("Id,IssueDate,RepairOrderId,ClientId,VehicleId,PaymentMethod")] Billing billing)
        {
            if (ModelState.IsValid)
            {
                _context.Add(billing);
                await _context.SaveChangesAsync();
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
            {
                return NotFound();
            }

            var billing = await _context.Billings.FindAsync(id);
            if (billing == null)
            {
                return NotFound();
            }
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,IssueDate,RepairOrderId,ClientId,VehicleId,PaymentMethod")] Billing billing)
        {
            if (id != billing.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(billing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillingExists(billing.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
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
            {
                return NotFound();
            }

            var billing = await _context.Billings
                .Include(b => b.Client)
                .Include(b => b.RepairOrder)
                .Include(b => b.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (billing == null)
            {
                return NotFound();
            }

            return View(billing);
        }

        // POST: Billings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var billing = await _context.Billings.FindAsync(id);
            _context.Billings.Remove(billing);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BillingExists(int id)
        {
            return _context.Billings.Any(e => e.Id == id);
        }
    }
}
