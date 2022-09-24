using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepairshopWeb.Data.Repositories;
using RepairshopWeb.Helpers;
using RepairshopWeb.Models;
using System.Threading.Tasks;

namespace RepairshopWeb.Controllers
{
    [Authorize]
    public class RepairOrdersController : Controller
    {
        private readonly IRepairOrderRepository _repairOrderRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IMechanicRepository _mechanicRepository;

        public RepairOrdersController(IRepairOrderRepository repairOrderRepository,
            IServiceRepository serviceRepository,
            IVehicleRepository vehicleRepository,
            IMechanicRepository mechanicRepository)
        {
            _repairOrderRepository = repairOrderRepository;
            _serviceRepository = serviceRepository;
            _vehicleRepository = vehicleRepository;
            _mechanicRepository = mechanicRepository;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _repairOrderRepository.GetRepairOrderAsync(this.User.Identity.Name);
            return View(model);
        }

        //Create do Repair Order
        public async Task<IActionResult> Create()
        {
            var model = await _repairOrderRepository.GetDetailsTempsAsync(this.User.Identity.Name); //Busca o método GetDetailsTempsAsync
            return View(model);
        }

        //Add Service do Create Repair Order
        public IActionResult AddService()
        {
            var model = new AddItemViewModel
            {
                Services = _serviceRepository.GetComboServices(),
                Vehicles = _vehicleRepository.GetComboVehicles(),
                Mechanics = _mechanicRepository.GetComboMechanics(),
            };
            return View(model);
        }

        //Add service na Repair Order Detail Temp
        [HttpPost]
        public async Task<IActionResult> AddService(AddItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _repairOrderRepository.AddItemToOrderAsync(model, this.User.Identity.Name);
                return RedirectToAction("Create");
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("ItemNotFound");

            await _repairOrderRepository.DeleteDetailTempAsync(id.Value);
            return RedirectToAction("Create");
        }

        //Confirm RepairOrder
        public async Task<IActionResult> ConfirmOrder()
        {
            try
            {
                var response = await _repairOrderRepository.ConfirmRepairOrderAsync(this.User.Identity.Name);
                if (response)
                    return RedirectToAction("Index");

                return RedirectToAction("Create");
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public IActionResult ItemNotFound()
        {
            return View();
        }
    }
}
