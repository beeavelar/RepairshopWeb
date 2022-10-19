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
using RepairshopWeb.Models;

namespace RepairshopWeb.Controllers
{
    public class ReceptionistsController : Controller
    {
        private readonly IReceptionistRepository _receptionistRepository;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        public ReceptionistsController(IReceptionistRepository receptionistRepository,
            IUserHelper userHelper, IBlobHelper blobHelper, IConverterHelper converterHelper)
        {
            _receptionistRepository = receptionistRepository;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
        }

        // GET: Receptionists
        public IActionResult Index()
        {
            return View(_receptionistRepository.GetAll().OrderBy(r => r.FirstName));
        }

        // GET: Receptionists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("ReceptionistNotFound");

            var receptionist = await _receptionistRepository.GetByIdAsync(id.Value);

            if (receptionist == null)
                return new NotFoundViewResult("ReceptionistNotFound");

            return View(receptionist);
        }

        [Authorize(Roles = "ADMIN")]
        // GET: Receptionists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Receptionists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReceptionistViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "receptionist");

                var receptionist = _converterHelper.ToReceptionist(model, imageId, true);

                receptionist.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                await _receptionistRepository.CreateAsync(receptionist);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Roles = "ADMIN")]
        // GET: Receptionists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("ReceptionistNotFound");

            var receptionist = await _receptionistRepository.GetByIdAsync(id.Value);
            if (receptionist == null)
                return new NotFoundViewResult("ReceptionistNotFound");

            var model = _converterHelper.ToReceptionistViewModel(receptionist);
            return View(model);
        }

        // POST: Receptionists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ReceptionistViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = model.ImageId;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "receptionists");

                    var receptionist = _converterHelper.ToReceptionist(model, imageId, false);

                    receptionist.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    await _receptionistRepository.UpdateAsync(receptionist);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _receptionistRepository.ExistAsync(model.Id))
                        return new NotFoundViewResult("ReceptionistNotFound");
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Roles = "ADMIN")]
        // GET: Receptionists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("ReceptionistNotFound");

            var receptionist = await _receptionistRepository.GetByIdAsync(id.Value);

            if (receptionist == null)
                return new NotFoundViewResult("ReceptionistNotFound");

            return View(receptionist);
        }

        // POST: Receptionists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var receptionist = await _receptionistRepository.GetByIdAsync(id);
            await _receptionistRepository.DeleteAsync(receptionist);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ReceptionistNotFound()
        {
            return View();
        }
    }
}
