using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RepairshopWeb.Data;
using RepairshopWeb.Data.Repositories;
using RepairshopWeb.Helpers;
using RepairshopWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepairshopWeb.Controllers
{
    public class MechanicsController : Controller
    {
        private readonly DataContext _context;
        private readonly IMechanicRepository _mechanicRepository;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        public MechanicsController(DataContext context, IMechanicRepository mechanicRepository,
            IUserHelper userHelper, IBlobHelper blobHelper, IConverterHelper converterHelper)
        {
            _context = context;
            _mechanicRepository = mechanicRepository;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
        }

        //[AllowAnonymous]
        // GET: Mechanics
        public IActionResult Index()
        {
            return View(_mechanicRepository.GetAll().OrderBy(m => m.FirstName));
        }

        // GET: Mechanics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("MechanicNotFound");

            var mechanic = await _mechanicRepository.GetByIdAsync(id.Value);

            if (mechanic == null)
                return new NotFoundViewResult("MechanicNotFound");

            return View(mechanic);
        }

        [Authorize(Roles = "ADMIN")]
        // GET: Mechanics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Mechanics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MechanicViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "photos");

                var mechanic = _converterHelper.ToMechanic(model, imageId, true);

                mechanic.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                await _mechanicRepository.CreateAsync(mechanic);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Roles = "ADMIN")]
        // GET: Mechanics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("MechanicNotFound");

            var mechanic = await _mechanicRepository.GetByIdAsync(id.Value);
            if (mechanic == null)
                return new NotFoundViewResult("MechanicNotFound");

            var model = _converterHelper.ToMechanicViewModel(mechanic);
            return View(model);
        }

        // POST: Mechanics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MechanicViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = model.ImageId;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "photos");

                    var mechanic = _converterHelper.ToMechanic(model, imageId, false);

                    mechanic.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    await _mechanicRepository.UpdateAsync(mechanic);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _mechanicRepository.ExistAsync(model.Id))
                        return new NotFoundViewResult("MechanicNotFound");
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Roles = "ADMIN")]
        // GET: Mechanics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("MechanicNotFound");

            var mechanic = await _mechanicRepository.GetByIdAsync(id.Value);

            if (mechanic == null)
                return new NotFoundViewResult("MechanicNotFound");

            return View(mechanic);
        }

        // POST: Mechanics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mechanic = await _mechanicRepository.GetByIdAsync(id);
            await _mechanicRepository.DeleteAsync(mechanic);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult MechanicNotFound()
        {
            return View();
        }
    }
}
