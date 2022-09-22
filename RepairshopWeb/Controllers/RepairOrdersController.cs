using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepairshopWeb.Data.Repositories;
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
                Mechanics = _mechanicRepository.GetComboMechanics()
            };

            return View(model);
        }

       
    }
}
