using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepairshopWeb.Data.Repositories;
using RepairshopWeb.Helpers;
using RepairshopWeb.Models;

namespace RepairshopWeb.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IRepairOrderRepository _repairOrderRepository;

        public AppointmentsController(IAppointmentRepository appointmentRepository,
           IClientRepository clientRepository,
           IVehicleRepository vehicleRepository, IRepairOrderRepository repairOrderRepository)
        {
            _appointmentRepository = appointmentRepository;
            _clientRepository = clientRepository;
            _vehicleRepository = vehicleRepository;
            _repairOrderRepository = repairOrderRepository;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _appointmentRepository.GetAppointmentAsync(this.User.Identity.Name);
            return View(model);
        }

        //[Authorize(Roles = "Mechanic, Receptionist")]
        //Create do Repair Order
        public async Task<IActionResult> Create()
        {
            var model = await _appointmentRepository.GetDetailsTempsAsync(this.User.Identity.Name); //Busca o método GetDetailsTempsAsync
            return View(model);
        }

        //Add appointment do Create Appointment
        public IActionResult AddAppointment()
        {
            var model = new AddAppointmentViewModel
            {
                AppointmentDate = DateTime.Now,
                AlertDate = DateTime.Now,
                Clients = _clientRepository.GetComboClients(),
                Vehicles = _vehicleRepository.GetComboVehicles()
            };
            return View(model);
        }

        //Add appointment na Appointment Detail Temp
        [HttpPost]
        public async Task<IActionResult> AddAppointment(AddAppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _appointmentRepository.AddItemToAppointmentAsync(model, this.User.Identity.Name);
                return RedirectToAction("Create");
            }
            return View(model);
        }

        //Delete item from AppointmentDetailTemp --> Before confirm
        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("ItemNotFound");

            await _appointmentRepository.DeleteDetailTempAsync(id.Value);
            return RedirectToAction("Create");
        }

        [Authorize(Roles = "ADMIN")]
        //Delete Repair Order from RepoairOrders --> After confirm
        public async Task<IActionResult> DeleteAppointments(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("ItemNotFound");

            await _appointmentRepository.DeleteAppointmentAsync(id.Value);
            return RedirectToAction("Index");
        }

        //Confirm Appointment
        public async Task<IActionResult> ConfirmAppointment()
        {
            var response = await _appointmentRepository.ConfirmAppointmentAsync(this.User.Identity.Name);
            if (response)
                return RedirectToAction("Index");
            return RedirectToAction("Create");
        }

        //Get do Appointment Status - Faz aparecer a view
        public async Task<IActionResult> AppointmentStatus(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("ItemNotFound");

            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(id.Value);

            if (appointment == null)
                return new NotFoundViewResult("ItemNotFound");

            var model = new AppointmentStatusViewModel
            {
                Id = appointment.Id,
                AppointmentStatus = appointment.AppointmentStatus
            };

            return View(model);
        }

        //Post do Appointment Status - grava as informações
        [HttpPost]
        public async Task<IActionResult> AppointmentStatus(AppointmentStatusViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _appointmentRepository.StatusAppointment(model);
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
