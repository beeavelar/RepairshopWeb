using Microsoft.AspNetCore.Mvc;

namespace RepairshopWeb.Controllers
{
    public class Dashboard : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
