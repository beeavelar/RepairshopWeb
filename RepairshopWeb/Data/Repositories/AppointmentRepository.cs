using Microsoft.EntityFrameworkCore;
using RepairshopWeb.Data.Entities;
using RepairshopWeb.Data.Repositories.DataTransferObjects;
using RepairshopWeb.Helpers;
using RepairshopWeb.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RepairshopWeb.Data.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public AppointmentRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task AddItemToAppointmentAsync(AddAppointmentViewModel model, string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
                return;

            var client = await _context.Clients.FindAsync(model.ClientId);
            if (client == null)
                return;

            var vehicle = await _context.Vehicles.FindAsync(model.VehicleId);
            if (vehicle == null)
                return;

            //Pedir o Appoint. Detail Temp
            var appointmentDetailTemp = await _context.AppointmentDetailsTemp
                .Where(adt => adt.User == user && adt.Client == client && adt.Vehicle == vehicle)
                .FirstOrDefaultAsync();

            if (appointmentDetailTemp == null) //Se for null é pq nao há nenhum
            {
                appointmentDetailTemp = new AppointmentDetailTemp //Se nao tem nenhum, criar um novo
                {
                    AppointmentDate = model.AppointmentDate,
                    AlertDate = model.AlertDate,
                    Vehicle = vehicle,
                    Client = client,
                    User = user
                };
                _context.AppointmentDetailsTemp.Add(appointmentDetailTemp);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<AppointmentDetailsDto> ConfirmAppointmentAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
                return new AppointmentDetailsDto { IsSuccess = false};

            var appointmentTemps = await _context.AppointmentDetailsTemp
                .Include(adt => adt.Client)
                .Include(adt => adt.Vehicle)
                .Where(adt => adt.User == user)
                .ToListAsync();

            if (appointmentTemps == null || appointmentTemps.Count == 0)
                return new AppointmentDetailsDto { IsSuccess = false };

            //Passa as informações do AppointmentDetailTemp para AppointmentDetail
            var details = appointmentTemps.Select(ad => new AppointmentDetail
            {
                AppointmentDate = ad.AppointmentDate,
                AlertDate = ad.AlertDate,
                Vehicle = ad.Vehicle,
                VehicleId = ad.VehicleId,
                Client = ad.Client,
                ClientId = ad.ClientId,
            }).ToList();

            //Passa as informações do AppointmentDetail para Appointment
            var appointment = new Appointment
            {
                Date = DateTime.Now,
                AppointmentDate = details[0].AppointmentDate,
                AlertDate = details[0].AlertDate,
                VehicleId = details[0].VehicleId,
                ClientId = details[0].ClientId,
                User = user,
                Items = details
            };

            appointment.IsActive = true;

            await CreateAsync(appointment);
            _context.AppointmentDetailsTemp.RemoveRange(appointmentTemps);
            await _context.SaveChangesAsync();
            return new AppointmentDetailsDto { IsSuccess = true, ClientName = details[0].Client.FullName, Email = details[0].Client.Email, VehiclePlate = details[0].Vehicle.LicensePlate, AppointmentDate = details[0].AppointmentDate };
        }

        public async Task DeleteDetailTempAsync(int id)
        {
            var appointmentDetailTemp = await _context.AppointmentDetailsTemp.FindAsync(id);
            if (appointmentDetailTemp == null)
                return;

            _context.AppointmentDetailsTemp.Remove(appointmentDetailTemp);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAppointmentAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
                return;

            appointment.IsActive = false;
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Appointments.Include(a => a.User);
        }

        public async Task<Appointment> GetAppointmentByIdAsync(int id)
        {
            return await _context.Appointments.FindAsync(id);
        }

        //Método que devolve todos os appointments que um determinado user fez
        public async Task<IQueryable<Appointment>> GetAppointmentAsync(string userName)
        {
            //Buscar o user
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null) //Se nao tiver user retorna nulo (lista vazia)
                return null;

            if (await _userHelper.IsUserInRoleAsync(user, "ADMIN")) //Se o user for o Admin, buscar todas os appointments de todos os users
            {
                return _context.Appointments
                    .Include(app => app.Items)
                    .Include(ro => ro.RepairOrder)
                    .Include(v => v.Vehicle)
                    .Include(c => c.Client)
                    .OrderByDescending(app => app.Date);
            }

            return _context.Appointments //Se não for Admin, buscar os Appointments do user que estiver logado
                .Include(app => app.Items)
                .Include(ro => ro.RepairOrder)
                .Include(c => c.Client)
                .Include(v => v.Vehicle)
                .Where(app => app.User == user) //onde o User for igual ao user
                .OrderByDescending(app => app.Date);
        }

        public async Task<IQueryable<AppointmentDetailTemp>> GetDetailsTempsAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
                return null;

            return _context.AppointmentDetailsTemp
               .Include(v => v.Vehicle)
               .Include(c => c.Client)
               .Where(app => app.User == user);
        }

        public async Task StatusAppointment(AppointmentStatusViewModel model)
        {
            var appointment = await _context.Appointments.FindAsync(model.Id);
            if (appointment == null)
                return;

            appointment.AppointmentStatus = model.AppointmentStatus;
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }
    }
}
