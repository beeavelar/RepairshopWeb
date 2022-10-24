using RepairshopWeb.Data.Entities;
using RepairshopWeb.Data.Repository;
using RepairshopWeb.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RepairshopWeb.Data.Repositories
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        public IQueryable GetAllWithUsers();

        //Método que devolve todas os appointments de um determinado cliente
        Task<IQueryable<Appointment>> GetAppointmentAsync(string userName); //Buscar os appointments por user

        //Método que recebe um user e devolve o user temporario
        Task<IQueryable<AppointmentDetailTemp>> GetDetailsTempsAsync(string userName);

        //Método que adiciona o item a Appointment Detail Temp
        Task AddItemToAppointmentAsync(AddAppointmentViewModel model, string userName);

        //Método para deletar appointment da Appointment Detail Temp
        Task DeleteDetailTempAsync(int id);

        //Método para confirmar o Appointment
        Task<bool> ConfirmAppointmentAsync(string userName);

        Task<Appointment> GetAppointmentAsync(int id);  //Buscar os Appointments por id

        //Método para deletar o appointment 
        Task DeleteAppointmentAsync(int id);
    }
}
