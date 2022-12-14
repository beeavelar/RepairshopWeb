using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepairshopWeb.Data.Repositories;
using RepairshopWeb.Helpers;
using RepairshopWeb.Models;

namespace RepairshopWeb.Controllers
{
    public class ClientsController : Controller
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        public ClientsController(IClientRepository clientRepository,
            IUserHelper userHelper, IBlobHelper blobHelper, IConverterHelper converterHelper)
        {
            _clientRepository = clientRepository;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
        }

        // GET: Clients
        public IActionResult Index()
        {
            return View(_clientRepository.GetAll().OrderBy(c => c.FirstName));
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("ClientNotFound");

            var client = await _clientRepository.GetByIdAsync(id.Value);

            if (client == null)
                return new NotFoundViewResult("ClientNotFound");

            return View(client);
        }

        [Authorize(Roles = "MECHANIC, RECEPTIONIST")]
        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientViewModel model)
        {
            if (ModelState.IsValid)
            {
                var clientEmail = await _clientRepository.GetClient(model.Email);

                if (clientEmail is null)
                {
                    Guid imageId = Guid.Empty;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "photos");

                    var client = _converterHelper.ToClient(model, imageId, true);

                    client.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                    await _clientRepository.CreateAsync(client);
                }
                else
                {
                    ViewBag.Message = "User already exists.";
                    return View(model);
                }
                    
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        //[Authorize(Roles = "MECHANIC, RECEPTIONIST")]
        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("ClientNotFound");

            var client = await _clientRepository.GetByIdAsync(id.Value);
            
            if (client == null)
                return new NotFoundViewResult("ClientNotFound");

            var model = _converterHelper.ToClientViewModel(client);
            return View(model);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClientViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = model.ImageId;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "photos");

                    var client = _converterHelper.ToClient(model, imageId, false);

                    client.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    await _clientRepository.UpdateAsync(client);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _clientRepository.ExistAsync(model.Id))
                        return new NotFoundViewResult("ClientNotFound");
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("ClientNotFound");

            var client = await _clientRepository.GetByIdAsync(id.Value);

            if (client == null)
                return new NotFoundViewResult("ClientNotFound");

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            await _clientRepository.DeleteAsync(client);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ClientStatus(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("ClientNotFound");

            var client = await _clientRepository.GetClientByIdAsync(id.Value);

            if (client == null)
                return new NotFoundViewResult("ClientNotFound");

            var model = new ClientStatusViewModel
            {
                Id = client.Id,
                ClientStatus = client.ClientStatus
            };

            return View(model);
        }

        //Post do Appointment Status - grava as informações
        [HttpPost]
        public async Task<IActionResult> ClientStatus(ClientStatusViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _clientRepository.StatusClient(model);
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult ClientNotFound()
        {
            return View();
        }

    }
}
