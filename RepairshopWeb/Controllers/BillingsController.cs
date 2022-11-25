using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RepairshopWeb.Data;
using RepairshopWeb.Data.Entities;
using RepairshopWeb.Data.Repositories;
using RepairshopWeb.Helpers;
using RepairshopWeb.Models;
using Syncfusion.Pdf.Parsing;

namespace RepairshopWeb.Controllers
{
    public class BillingsController : Controller
    {
        private readonly DataContext _context;
        private readonly IBillingRepository _billingRepository;
        private readonly IUserHelper _userHelper;
        private readonly IRepairOrderRepository _repairOrderRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IHostEnvironment _environment;
        private readonly IEmailHelper _emailHelper;
        private readonly IAppointmentRepository _appointmentRepository;

        public BillingsController(DataContext context,
            IBillingRepository billingRepository, IUserHelper userHelper, 
            IRepairOrderRepository repairOrderRepository, IClientRepository clientRepository, 
            IServiceRepository serviceRepository, IHostEnvironment environment, IEmailHelper emailHelper,
            IAppointmentRepository appointmentRepository)
        {
            _context = context;
            _billingRepository = billingRepository;
            _userHelper = userHelper;
            _repairOrderRepository = repairOrderRepository;
            _clientRepository = clientRepository;
            _serviceRepository = serviceRepository;
            _environment = environment;
            _emailHelper = emailHelper;
            _appointmentRepository = appointmentRepository;
        }

        // GET: Billings
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Billings.Include(b => b.Client).Include(b => b.RepairOrder).Include(b => b.Vehicle);
            return View(await dataContext.ToListAsync());
        }

        // GET: Billings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("BillingNotFound");

            var billing = await _context.Billings
                .Include(b => b.Client)
                .Include(b => b.RepairOrder)
                .Include(b => b.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (billing == null)
                return new NotFoundViewResult("BillingNotFound");

            return View(billing);
        }

        // GET: Billings/Create
        public async Task<IActionResult> Create(int? id)
        {

            var list = _context.RepairOrders.Where(p => p.Id == id).Select(p => new SelectListItem
            {

                Text = $"{p.Vehicle.Brand} {p.Vehicle.VehicleModel}" + $"{p.Id}",
                Value = p.Id.ToString()

            }).ToList();

            IEnumerable<SelectListItem> combo = list;

            ViewData["RepairOrderId"] = combo;

            return View();
        }

        // POST: Billings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Billing billing)
        {

            if (ModelState.IsValid)
            {
                var repairs = await _repairOrderRepository.GetRepairOrderByIdAsync(billing.RepairOrderId);

                #region servicos
                var services = _context.Services;
                var details = _context.RepairOrderDetails;
                var ro = _context.RepairOrders;

                var servicesModel =
                    from service in services
                    join detail in details
                    on service.Id equals detail.ServiceId
                    join repairorder in ro
                    on detail.RepairOrderId equals repairorder.Id
                    where repairorder.Id == repairs.Id
                    select new ServiceViewModel
                    {
                        Id = service.Id,
                        Description = service.Description,
                        Price = service.RepairPrice

                    };
                #endregion

                var client = await _clientRepository.GetClientById(repairs.Vehicle.ClientId);

                //Create Billing

                billing.ClientId = repairs.Vehicle.ClientId;
                billing.Nif = client.Nif;
                billing.VehicleId = repairs.VehicleId;
                billing.TotalToPay = repairs.TotalToPay;
                billing.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                //Load template and forms
                FileStream pdfTemplate = new FileStream(Path.Combine(_environment.ContentRootPath, "wwwroot", "templatepdf", "InvoiceTemplate.pdf"), FileMode.Open, FileAccess.Read);
                PdfLoadedDocument loadedDocument = new PdfLoadedDocument(pdfTemplate);
                PdfLoadedForm form = loadedDocument.Form;

                form.ReadOnly = false;

                (form.Fields["invoiceNumber"] as PdfLoadedTextBoxField).Text = billing.Id.ToString();
                (form.Fields["invoiceDate"] as PdfLoadedTextBoxField).Text = DateTime.Now.ToShortDateString();
                (form.Fields["repairOrderNumber"] as PdfLoadedTextBoxField).Text = billing.RepairOrderId.ToString();

                ServiceViewModel[] List = servicesModel.ToArray();
                var aux = 0;
                do
                {
                    foreach (var item in List)
                    {

                        var textserv = "service" + $"{aux + 1}";
                        var textprice = "price" + $"{aux + 1}";
                        (form.Fields[textserv] as PdfLoadedTextBoxField).Text = item.Description;
                        (form.Fields[textprice] as PdfLoadedTextBoxField).Text = item.Price.ToString() + "€";
                        aux++;
                    }
                } while (aux < servicesModel.Count());



                (form.Fields["total"] as PdfLoadedTextBoxField).Text = billing.TotalToPay.ToString() + "€";

                form.ReadOnly = true;

                MemoryStream pdfStream = new MemoryStream();

                loadedDocument.Save(pdfStream);
                loadedDocument.Close(true);

                await _emailHelper.SendEmailWithAttachment(client.Email, "Repairshop - Invoice", "Mr./Ms." +
                    "<br/><br/>We are happy to choose Repairshop. <br/><br/>Attached, we send the invoice for the services we perform on your car." +
                      "<br/><br/>Best regards, " +
                      "<br/>RepairShop", pdfStream);

                repairs.PaymentState = "Payed";
                repairs.IsBilling = true;

                await _repairOrderRepository.UpdateAsync(repairs);

                repairs.Appointment.IsActive = false;

                await _appointmentRepository.UpdateAsync(repairs.Appointment);

                return RedirectToAction("Index", "Appointments");
            }

            return View();
        }

        // GET: Billings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("BillingNotFound");

            var billing = await _billingRepository.GetByIdAsync(id.Value);

            if (billing == null)
                return new NotFoundViewResult("BillingNotFound");

            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FullName", billing.ClientId);
            ViewData["RepairOrderId"] = new SelectList(_context.RepairOrders, "Id", "Id", billing.RepairOrderId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "LicensePlate", billing.VehicleId);
            return View(billing);
        }

        // POST: Billings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Billing billing)
        {
            if (id != billing.Id)
                return new NotFoundViewResult("BillingNotFound");

            if (ModelState.IsValid)
            {
                try
                {
                    billing.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    await _billingRepository.UpdateAsync(billing);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _billingRepository.ExistAsync(billing.Id))
                        return new NotFoundViewResult("BillingNotFound");
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FullName", billing.ClientId);
            ViewData["RepairOrderId"] = new SelectList(_context.RepairOrders, "Id", "Id", billing.RepairOrderId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "LicensePlate", billing.VehicleId);
            return View(billing);
        }

        // GET: Billings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("BillingNotFound");

            var billing = await _context.Billings
                .Include(b => b.Client)
                .Include(b => b.RepairOrder)
                .Include(b => b.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (billing == null)
                return new NotFoundViewResult("BillingNotFound");

            return View(billing);
        }

        // POST: Billings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var billing = await _billingRepository.GetByIdAsync(id);
            await _billingRepository.DeleteAsync(billing);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult BillingNotFound()
        {
            return View();
        }
    }
}
