using RepairshopWeb.Data.Entities;
using RepairshopWeb.Data.Repository;
using System.Linq;
using System.Threading.Tasks;

namespace RepairshopWeb.Data.Repositories
{
    public interface IRepairOrderRepository : IGenericRepository<RepairOrder>
    {
        //Método que devolve todas as RepairsOrders de um determinado cliente
        Task<IQueryable<RepairOrder>> GetRepairOrderAsync(string userName);
    }
}
