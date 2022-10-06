using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepairshopWeb.Data.Repositories;
using RepairshopWeb.Helpers;
using RepairshopWeb.Models;
using System;
using System.Threading.Tasks;

namespace RepairshopWeb.Controllers
{
    //[Authorize]
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

        //[Authorize(Roles = "Mechanic, Receptionist")]
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

        //Delete item from RepairOrderDetailTemp --> Before confirm
        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("ItemNotFound");

            await _repairOrderRepository.DeleteDetailTempAsync(id.Value);
            return RedirectToAction("Create");
        }

        //Delete Repair Order from RepoairOrders --> After confirm
        public async Task<IActionResult> DeleteRepairOrder(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("ItemNotFound");

            await _repairOrderRepository.DeleteRepairOrderAsync(id.Value);
            return RedirectToAction("Index");
        }

        //Confirm RepairOrder
        public async Task<IActionResult> ConfirmRepairOrder()
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

        //Get do Appointment - Faz aparecer a view
        public async Task<IActionResult> Appointment(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("ItemNotFound");

            var repairOrder = await _repairOrderRepository.GetRepairOrderAsync(id.Value);

            if (repairOrder == null)
                return new NotFoundViewResult("ItemNotFound");

            var model = new AppointmentViewModel
            {
                Id = repairOrder.Id,
                RepairDate = DateTime.Today,
                AlertDate = DateTime.Today
            };

            return View(model);
        }

        //Post do Appointment - grava as informações
        [HttpPost]
        public async Task<IActionResult> Appointment(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _repairOrderRepository.AppointementRepairOrder(model);
                return RedirectToAction("Index");
            }
            return View();
        }

        //Get do Repair Order Status - Faz aparecer a view
        public async Task<IActionResult> RepairStatus(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("ItemNotFound");

            var repairOrder = await _repairOrderRepository.GetRepairOrderAsync(id.Value);

            if (repairOrder == null)
                return new NotFoundViewResult("ItemNotFound");

            var model = new RepairOrderStatusViewModel
            {
                Id = repairOrder.Id,
                RepairStatus = repairOrder.RepairStatus
            };

            return View(model);
        }

        //Post do Repair Order Status - grava as informações
        [HttpPost]
        public async Task<IActionResult> RepairStatus(RepairOrderStatusViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _repairOrderRepository.StatusRepairOrder(model);
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult ItemNotFound()
        {
            return View();
        }
    }
}
