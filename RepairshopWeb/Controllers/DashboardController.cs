using Microsoft.AspNetCore.Mvc;

namespace RepairshopWeb.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
