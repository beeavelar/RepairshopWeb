using RepairshopWeb.Data.Entities;
using RepairshopWeb.Data.Repository;
using RepairshopWeb.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RepairshopWeb.Data.Repositories
{
    public interface IRepairOrderRepository : IGenericRepository<RepairOrder>
    {
        //Método que devolve todas as RepairsOrders de um determinado cliente
        Task<IQueryable<RepairOrder>> GetRepairOrderAsync(string userName);

        //Método que recebe um user e devolve o user temporario
        Task<IQueryable<RepairOrderDetailTemp>> GetDetailsTempsAsync(string userName);

        //Método que adiciona o item a Repair Order TDetail emp
        Task AddItemToOrderAsync(AddItemViewModel model, string userName);

        //Método para deletar Services da Repair Order Detail Temp
        Task DeleteDetailTempAsync(int id);

        //Método para confirmar a Repair Order
        Task<bool> ConfirmRepairOrderAsync(string userName);
    }
}
