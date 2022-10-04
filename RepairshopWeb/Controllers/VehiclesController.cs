using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RepairshopWeb.Data;
using RepairshopWeb.Data.Entities;
using RepairshopWeb.Data.Repositories;
using RepairshopWeb.Helpers;

namespace RepairshopWeb.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly DataContext _context;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IUserHelper _userHelper;

        public VehiclesController(DataContext context,
            IVehicleRepository vehicleRepository, IUserHelper userHelper)
        {
            _context = context;
            _vehicleRepository = vehicleRepository;
            _userHelper = userHelper;
        }

        // GET: Vehicles
        public IActionResult Index()
        {
            var vehicle = _context.Vehicles.Include(c => c.Client).ToList();
            return View(vehicle);
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("VehicleNotFound");

            var vehicle = await _context.Vehicles
                    .Include(c => c.Client)
                    .FirstOrDefaultAsync(v => v.Id == id);

            if (vehicle == null)
                return new NotFoundViewResult("VehicleNotFound");

            return View(vehicle);
        }

        //[Authorize(Roles = "Mechanic, Receptionist")]
        // GET: Vehicles/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FullName");
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                vehicle.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                await _vehicleRepository.CreateAsync(vehicle);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FullName", vehicle.ClientId);
            return View(vehicle);
        }

        //[Authorize(Roles = "Mechanic, Receptionist")]
        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("VehicleNotFound");

            var vehicle = await _vehicleRepository.GetByIdAsync(id.Value);

            if (vehicle == null)
                return new NotFoundViewResult("VehicleNotFound");

            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FullName", vehicle.ClientId);
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Vehicle vehicle)
        {
            if (id != vehicle.Id)
                return new NotFoundViewResult("VehicleNotFound");

            if (ModelState.IsValid)
            {
                try
                {
                    vehicle.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    await _vehicleRepository.UpdateAsync(vehicle);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _vehicleRepository.ExistAsync(vehicle.Id))
                        return new NotFoundViewResult("VehicleNotFound");
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FullName", vehicle.ClientId);
            return View(vehicle);
        }

        //[Authorize(Roles = "Mechanic, Receptionist")]
        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("VehicleNotFound");

            var vehicle = await _context.Vehicles
                   .Include(c => c.Client)
                   .FirstOrDefaultAsync(v => v.Id == id);

            if (vehicle == null)
                return new NotFoundViewResult("VehicleNotFound");

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehcile = await _vehicleRepository.GetByIdAsync(id);
            await _vehicleRepository.DeleteAsync(vehcile);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult VehicleNotFound()
        {
            return View();
        }
    }
}
