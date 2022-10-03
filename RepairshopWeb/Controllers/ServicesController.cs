using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepairshopWeb.Data.Entities;
using RepairshopWeb.Data.Repositories;
using RepairshopWeb.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RepairshopWeb.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IUserHelper _userHelper;

        public ServicesController(IServiceRepository serviceRepository, IUserHelper userHelper)
        {
            _serviceRepository = serviceRepository;
            _userHelper = userHelper;
        }

        // GET: Services
        public IActionResult Index()
        {
            return View(_serviceRepository.GetAll().OrderBy(s => s.Description));
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("ServiceNotFound");

            var service = await _serviceRepository.GetByIdAsync(id.Value);

            if (service == null)
                return new NotFoundViewResult("ServiceNotFound");

            return View(service);
        }

        // GET: Services/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service service)
        {
            if (ModelState.IsValid)
            {
                service.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                await _serviceRepository.CreateAsync(service);
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("ServiceNotFound");

            var service = await _serviceRepository.GetByIdAsync(id.Value);

            if (service == null)
                return new NotFoundViewResult("ServiceNotFound");

            return View(service);
        }

        // POST: services/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Service service)
        {
            if (id != service.Id)
                return new NotFoundViewResult("ServiceNotFound");

            if (ModelState.IsValid)
            {
                try
                {
                    service.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    await _serviceRepository.UpdateAsync(service);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _serviceRepository.ExistAsync(service.Id))
                        return new NotFoundViewResult("ServiceNotFound");
                    else

                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        // GET: services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("ServiceNotFound");

            var service = await _serviceRepository.GetByIdAsync(id.Value);

            if (service == null)
                return new NotFoundViewResult("ServiceNotFound");

            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);
            try
            {
                await _serviceRepository.DeleteAsync(service);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{service.Description} is probably being used.";
                    ViewBag.ErrorMessage = $"{service.Description} cannot be deleted because there are Repair Orders that were made with this service.</br></br>" +
                        $"Try to first delete all the Repair Orders that are using this service " +
                        $"and try again to delete the service.";
                }
                return View("Error");
            }
        }

        public IActionResult ServiceNotFound()
        {
            return View();
        }
    }

}
