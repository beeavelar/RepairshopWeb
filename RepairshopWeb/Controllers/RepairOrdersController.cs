using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepairshopWeb.Data;
using RepairshopWeb.Data.Entities;
using RepairshopWeb.Data.Repositories;
using RepairshopWeb.Helpers;
using RepairshopWeb.Models;
using System;
using System.IO;
using System.Linq;
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
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly DataContext _context;
        private readonly IEmailHelper _emailHelper;

        public RepairOrdersController(IRepairOrderRepository repairOrderRepository,
            IServiceRepository serviceRepository,
            IVehicleRepository vehicleRepository,
            IMechanicRepository mechanicRepository,
            IAppointmentRepository appointmentRepository,
            DataContext context, IEmailHelper emailHelper)
        {
            _repairOrderRepository = repairOrderRepository;
            _serviceRepository = serviceRepository;
            _vehicleRepository = vehicleRepository;
            _mechanicRepository = mechanicRepository;
            _appointmentRepository = appointmentRepository;
            _context = context;
            _emailHelper = emailHelper;
        }
        public async Task<IActionResult> Index(int? id)
        {
            var model = await _repairOrderRepository.GetRepairOrderAsync(this.User.Identity.Name);

            if (id is not null)
            {
                var appointment = await _appointmentRepository.GetAppointmentByIdAsync(id.Value);

                if (appointment == null)
                    return new NotFoundViewResult("ItemNotFound");
                ViewBag.Appointment = appointment.Id;
            }

            return View(model);
        }

        public async Task<IActionResult> IndexClient()
        {
            var model = await _repairOrderRepository.GetRepairOrderAsync(this.User.Identity.Name);
            return View(model);
        }

        //[Authorize(Roles = "Mechanic, Receptionist")]
        //Create do Repair Order
        public async Task<IActionResult> Create(int? id)
        {
            var model = await _repairOrderRepository.GetDetailsTempsAsync(this.User.Identity.Name); //Busca o método GetDetailsTempsAsync
            ViewBag.Appointment = id;
            return View(model);
        }

        //Add Service do Create Repair Order
        public IActionResult AddService(int? id)
        {
            ViewBag.Appointment = id;

            var model = new AddItemViewModel
            {
                Services = _serviceRepository.GetComboServices(),
                Vehicles = _vehicleRepository.GetComboVehicles(),
                Mechanics = _mechanicRepository.GetComboMechanics()
            };
            return View(model);
        }

        //Add service na Repair Order Detail Temp
        [HttpPost]
        public async Task<IActionResult> AddService(AddItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _repairOrderRepository.AddItemToRepairOrderAsync(model, this.User.Identity.Name);
                return RedirectToAction("Create", new { id = model.AppointmentId });
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

        [Authorize(Roles = "ADMIN")]
        //Delete Repair Order from RepoairOrders --> After confirm
        public async Task<IActionResult> DeleteRepairOrder(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("ItemNotFound");

            await _repairOrderRepository.DeleteRepairOrderAsync(id.Value);
            return RedirectToAction("Index");
        }

        //Confirm RepairOrder
        public async Task<IActionResult> ConfirmRepairOrder(int? id)
        {
            var response = await _repairOrderRepository.ConfirmRepairOrderAsync(this.User.Identity.Name, id.Value);
            if (response)
                return RedirectToAction("Index");

            ViewBag.Appointment = id;

            return RedirectToAction("Create");
        }

        //Get do Repair Order Status - Faz aparecer a view
        public async Task<IActionResult> RepairStatus(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("ItemNotFound");

            var repairOrder = await _repairOrderRepository.GetRepairOrderByIdAsync(id.Value);

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

        //Get do Repair Order Show Services - Faz aparecer a view
        public async Task<IActionResult> ShowServices(int? id)
        {
            //Verifica se o id da RO selecionada existe
            if (id == null)
                return new NotFoundViewResult("ItemNotFound");

            //Guarda o id da RO
            var repairOrder = await _repairOrderRepository.GetRepairOrderByIdAsync(id.Value);

            //Verifica se guardou o id da RO
            if (repairOrder == null)
                return new NotFoundViewResult("ItemNotFound");

            //Inner join da tabela Services com a tabela RepairOrderDetails e com a tabela repair orders--> selecionar os serviços 
            var services = _context.Services;
            var details = _context.RepairOrderDetails;
            var ro = _context.RepairOrders;

            var model =
                from service in services
                join detail in details
                on service.Id equals detail.ServiceId
                join repairorder in ro
                on detail.RepairOrderId equals repairorder.Id
                where repairorder.Id == id
                select new ServiceViewModel
                {
                    Id = service.Id,
                    Description = service.Description,
                };

            return View(model);
        }

        public async Task<IActionResult> ShowServicesIndexDash(int? id)
        {
            //Verifica se o id da RO selecionada existe
            if (id == null)
                return new NotFoundViewResult("ItemNotFound");

            //Guarda o id da RO
            var repairOrder = await _repairOrderRepository.GetRepairOrderByIdAsync(id.Value);

            //Verifica se guardou o id da RO
            if (repairOrder == null)
                return new NotFoundViewResult("ItemNotFound");

            //Inner join da tabela Services com a tabela RepairOrderDetails e com a tabela repair orders--> selecionar os serviços 
            var services = _context.Services;
            var details = _context.RepairOrderDetails;
            var ro = _context.RepairOrders;

            var model =
                from service in services
                join detail in details
                on service.Id equals detail.ServiceId
                join repairorder in ro
                on detail.RepairOrderId equals repairorder.Id
                where repairorder.Id == id
                select new ServiceViewModel
                {
                    Id = service.Id,
                    Description = service.Description,
                };

            return View(model);
        }

        public async Task<IActionResult> ShowServicesClient(int? id)
        {
            //Verifica se o id da RO selecionada existe
            if (id == null)
                return new NotFoundViewResult("ItemNotFound");

            //Guarda o id da RO
            var repairOrder = await _repairOrderRepository.GetRepairOrderByIdAsync(id.Value);

            //Verifica se guardou o id da RO
            if (repairOrder == null)
                return new NotFoundViewResult("ItemNotFound");

            //Inner join da tabela Services com a tabela RepairOrderDetails e com a tabela repair orders--> selecionar os serviços 
            var services = _context.Services;
            var details = _context.RepairOrderDetails;
            var ro = _context.RepairOrders;

            var model =
                from service in services
                join detail in details
                on service.Id equals detail.ServiceId
                join repairorder in ro
                on detail.RepairOrderId equals repairorder.Id
                where repairorder.Id == id
                select new ServiceViewModel
                {
                    Id = service.Id,
                    Description = service.Description,
                };

            return View(model);
        }

        public IActionResult ItemNotFound()
        {
            return View();
        }
    }
}
