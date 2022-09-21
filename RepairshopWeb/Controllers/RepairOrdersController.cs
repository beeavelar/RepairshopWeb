using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepairshopWeb.Data.Repositories;
using System.Threading.Tasks;

namespace RepairshopWeb.Controllers
{
    [Authorize]
    public class RepairOrdersController : Controller
    {
        private readonly IRepairOrderRepository _repairOrderRepository;

        public RepairOrdersController(IRepairOrderRepository repairOrderRepository)
        {
            _repairOrderRepository = repairOrderRepository;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _repairOrderRepository.GetRepairOrderAsync(this.User.Identity.Name);
            return View(model);
        }
    }
}
