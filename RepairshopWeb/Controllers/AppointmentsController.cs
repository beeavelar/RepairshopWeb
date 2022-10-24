using Microsoft.AspNetCore.Mvc;
using RepairshopWeb.Data.Entities;
using RepairshopWeb.Data.Repositories;
using RepairshopWeb.Helpers;
using RepairshopWeb.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
           IVehicleRepository vehicleRepository,IRepairOrderRepository repairOrderRepository)
        {
            _appointmentRepository = appointmentRepository;
            _clientRepository = clientRepository;
            _vehicleRepository = vehicleRepository;
            _repairOrderRepository = repairOrderRepository;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _appointmentRepository.GetAppointmentAsync(this.User.Identity.Name);
            //var appointment = new List<Appointment>();
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

        //Confirm Appointment
        public async Task<IActionResult> ConfirmAppointment()
        {
            var response = await _appointmentRepository.ConfirmAppointmentAsync(this.User.Identity.Name);
            if (response)
                return RedirectToAction("Index");
            return RedirectToAction("Create");
        }

        public IActionResult ItemNotFound()
        {
            return View();
        }
    }
}
