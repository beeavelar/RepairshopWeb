using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RepairshopWeb.Data.Repositories;
using System.Threading.Tasks;

namespace RepairshopWeb.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepairOrderRepository _repairOrderRepository;

        public DashboardController(ILogger<HomeController> logger, IRepairOrderRepository repairOrderRepository)
        {
            _logger = logger;
            _repairOrderRepository = repairOrderRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _repairOrderRepository.GetAllRepairOrders());
        }
    }
}
